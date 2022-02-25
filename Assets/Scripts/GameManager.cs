using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject levelTransition;
    [SerializeField] GameObject gameOverContainer;
    [SerializeField] AudioClip menuButtonSFX;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            // Get self audio source
            audioSource = GetComponent<AudioSource>();

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel()
    {
        audioSource.PlayOneShot(menuButtonSFX);

        // Load next level by build index
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void RestartGame()
    {
        // Load main menu
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int buildIndex)
    {
        // Create and play transition animation
        GameObject levelTransitionObj = Instantiate(levelTransition);
        DontDestroyOnLoad(levelTransitionObj);

        // Destroy the transition object after the transition ends
        Destroy(levelTransitionObj, 2f);

        // Load level after one second (in the middle of transition)
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(buildIndex);
    }

    public void GameOver()
    {
        Instantiate(gameOverContainer);
    }

    public void Quit()
    {
        audioSource.PlayOneShot(menuButtonSFX);
        Application.Quit();
    }
}
