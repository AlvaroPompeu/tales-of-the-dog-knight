using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    BackgroundSoundHelper backgroundSound;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject bossHealthBarContainer;
    [SerializeField] Slider bossHealthBar;
    [SerializeField] TextMeshProUGUI bossHealthText;
    [SerializeField] TextMeshProUGUI bossName;
    [SerializeField] GameObject floatingDamageText;
    [SerializeField] GameObject floatingBossDamageText;
    [SerializeField] GameObject minimap;

    public static HUDManager Instance;

    private void Awake()
    {
        Instance = this;
        backgroundSound = GameObject.Find("BackgroundSound").GetComponent<BackgroundSoundHelper>();
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

    public void SetBossHealth(float current, float max)
    {
        // Health cannot be negative
        if (current < 0)
        {
            current = 0;
        }

        bossHealthText.text = current.ToString() + " / " + max.ToString();
        bossHealthBar.value = current / max;
    }

    public void SetupBossFight(bool active, string name)
    {
        if (active)
        {
            bossName.text = name;
            minimap.SetActive(false);
            bossHealthBarContainer.SetActive(true);
            backgroundSound.SwitchAudioClip("Boss");
        }
        else
        {
            minimap.SetActive(true);
            bossHealthBarContainer.SetActive(false);
            backgroundSound.SwitchAudioClip("Environment");
        }
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

    public void CreateBossFloatingDamageText(float value, bool critical)
    {
        // Instantiate the popup as child of HUD canvas
        GameObject popup = Instantiate(floatingBossDamageText);
        popup.transform.SetParent(GameObject.Find("HUD").transform, false);
        TextMeshProUGUI text = popup.GetComponent<TextMeshProUGUI>();
        text.text = (value.ToString());

        // The text is different in case of a critical hit
        if (critical)
        {
            text.text += "!";
            text.fontSize = 36;
            text.color = Color.red;
        }
    }
}
