using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] GameObject gameOverTextContainer;

    private bool textEnabled;

    private void Awake()
    {
        StartCoroutine(EnableText());
    }

    IEnumerator EnableText()
    {
        // Display the game over text after the background fade animation
        yield return new WaitForSeconds(2f);
        gameOverTextContainer.SetActive(true);
        textEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Wait for the game over text to be enabled to be able to restart the game
        if (textEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.Instance.RestartGame();
            }
        }
    }
}
