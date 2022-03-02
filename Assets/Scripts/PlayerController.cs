using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    private Camera mainCamera;
    [SerializeField] Collider weaponHitbox;

    private AudioSource audioSource;
    [SerializeField] AudioClip normalFootstep;
    [SerializeField] AudioClip waterFootstep;
    [SerializeField] AudioClip weaponSwingSound;
    [SerializeField] AudioClip playerHitSound;
    [SerializeField] AudioClip playerHitImpactSound;
    [SerializeField] AudioClip playerDieSound;
    [SerializeField] AudioClip playerEatSound;

    private float moveSpeed = 9f;
    private float sprintModifier = 1.35f;
    private float waterModifier = 0.6f;
    private float attackSpeed = 0.55f;
    private float maxHealth = 200f;
    private float currentHealth;
    private float staggerTime = 0.3f;

    private bool isMoving;
    private bool isStaggered;
    private bool isSprinting;
    private bool isAttackReady = true;

    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player rigid body
        playerRigidbody = GetComponent<Rigidbody>();

        // Get the animator component
        playerAnimator = GetComponent<Animator>();

        // Get the audio source component
        audioSource = GetComponent<AudioSource>();

        // Get the main camera
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        // Set initial health
        currentHealth = maxHealth;
        HUDManager.Instance.SetHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        HandleSprint();

        // Get the user inputs
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        HandleFootstepSound(moveDir);

        // Attack if the player left clicks
        if (Input.GetMouseButton(0) && !isSprinting)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        // Rotate the player towards the mouse
        RotatePlayer();

        // The player can't move if he is staggered
        if (!isStaggered)
        {
            MovePlayer(moveDir);
        }
    }

    private void HandleSprint()
    {
        // Increase or decrease the speed if player is holding shift (sprinting)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
            moveSpeed *= sprintModifier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
            moveSpeed /= sprintModifier;
        }
    }

    private void HandleWater(bool isOnWater)
    {
        if (isOnWater)
        {
            // Set water footstep sound and slow the player
            audioSource.clip = waterFootstep;
            if (isMoving)
            {
                audioSource.Play();
            }
            moveSpeed *= waterModifier;
        }
        else
        {
            // Set normal footstep sound
            audioSource.clip = normalFootstep;
            if (isMoving)
            {
                audioSource.Play();
            }
            moveSpeed /= waterModifier;
        }
    }

    private void HandleFootstepSound(Vector3 movementInput)
    {
        // The player started to move
        if ((movementInput.x != 0 || movementInput.z != 0) && !isMoving)
        {
            // Play the footstep sound
            audioSource.Play();
            isMoving = true;
        }
        // The player stopped to move
        else if ((movementInput.x == 0 && movementInput.z == 0) && isMoving)
        {
            // Stop the footstep sound
            audioSource.Stop();
            isMoving = false;
        }
    }

    void MovePlayer(Vector3 direction)
    {
        // Play or stop the walking or sprint animation
        if ((direction.x != 0) || (direction.z != 0))
        {
            if (isSprinting)
            {
                playerAnimator.SetBool("bWalking", false);
                playerAnimator.SetBool("bSprinting", true);
            }
            else
            {
                playerAnimator.SetBool("bSprinting", false);
                playerAnimator.SetBool("bWalking", true);
            }
        }
        else
        {
            playerAnimator.SetBool("bWalking", false);
            playerAnimator.SetBool("bSprinting", false);
        }

        // Move the player
        playerRigidbody.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    void RotatePlayer()
    {
        Vector3 mousePos = Input.mousePosition;

        // Get the camera distance from the ground
        float cameraDistance = mainCamera.GetComponent<CameraMovement>().CameraDistance;
        mousePos.z = cameraDistance;

        // Get the mouse world position
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // Get the direction vector of the player to the mouse
        Vector3 lookDir = mouseWorldPos - transform.position;

        // Rotate the player towards the direction
        float angle = Mathf.Atan2(lookDir.z, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -angle + 90f, 0);
    }

    void Attack()
    {
        if (isAttackReady)
        {
            // Play the attack animation and sound
            playerAnimator.SetTrigger("tAttack");
            audioSource.PlayOneShot(weaponSwingSound);

            // Enable the weapon to hit enemies
            weaponHitbox.enabled = true;
            
            // Limits the attack speed
            isAttackReady = false;
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        isAttackReady = true;
        weaponHitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Collect the food
        if (other.gameObject.CompareTag("Food"))
        {
            PickUpFood(other.gameObject);
        }

        if (other.gameObject.CompareTag("Water"))
        {
            HandleWater(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            HandleWater(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // The player should only be hit if the enemy is in the "can deal damage" state
            if (collision.gameObject.GetComponent<EnemyBase>().CanDealDamage)
            {
                OnHit(collision.gameObject.GetComponent<EnemyBase>().Damage());
            }
        }
    }

    public void OnHit(int damage)
    {
        playerAnimator.SetTrigger("tHit");
        audioSource.PlayOneShot(playerHitImpactSound);
        audioSource.PlayOneShot(playerHitSound);
        isStaggered = true;
        
        // Apply the damage and check if the player dies
        currentHealth -= damage;

        // Update the HUD
        HUDManager.Instance.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(RemoveStagger());
    }

    IEnumerator RemoveStagger()
    {
        yield return new WaitForSeconds(staggerTime);
        isStaggered = false;
    }

    private void Die()
    {
        // Display the game over screen only once
        if (!playerRigidbody.isKinematic)
        {
            GameManager.Instance.GameOver();
        }

        // Play death animation and disable the player to be moved by attacks
        playerAnimator.SetBool("bDead", true);
        audioSource.PlayOneShot(playerDieSound);
        audioSource.enabled = false;
        HUDManager.Instance.StopAudio();
        playerRigidbody.isKinematic = true;

        // Disable the camera movement script
        mainCamera.GetComponent<CameraMovement>().enabled = false;

        // Disable this script
        this.enabled = false;
    }

    private void PickUpFood(GameObject food)
    {
        int healthToHeal = food.GetComponent<Food>().healthHealed;
        HealPlayer(healthToHeal);
        audioSource.PlayOneShot(playerEatSound);
        Destroy(food);
    }

    private void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        // Prevent overheal
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Update the HUD
        HUDManager.Instance.SetHealth(currentHealth, maxHealth);
    }
}
