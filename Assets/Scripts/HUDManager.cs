using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject floatingDamageText;

    public static HUDManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetHealth(float current, float max)
    {
        // Health cannot be negative
        if (current < 0)
        {
            current = 0;
        }

        healthText.text = current.ToString() + " / " + max.ToString();
        healthBar.value = current / max;
    }

    public void CreateFloatingDamageText(Vector3 position, float value, bool critical)
    {
        // Fix the y position
        position.y = 1.5f;

        // Get the reference from the game manager and instantiate the popup with the correct values
        GameObject popup = Instantiate(floatingDamageText, position, floatingDamageText.transform.rotation);
        TextMeshPro text = popup.GetComponent<TextMeshPro>();
        text.text = (value.ToString());

        // The text is different in case of a critical hit
        if (critical)
        {
            text.text += "!";
            text.fontSize = 44;
            text.color = Color.red;
        }
    }
}
