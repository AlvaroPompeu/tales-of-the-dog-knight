using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject levelTransition;

    private void Awake()
    {
        if (Instance == null)
        {
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
        // Create and play transition animation
        GameObject levelTransitionObj = Instantiate(levelTransition);
        DontDestroyOnLoad(levelTransitionObj);
        Animator transitionAnimator = levelTransitionObj.GetComponentInChildren<Animator>();
        transitionAnimator.SetTrigger("tStartTransition");

        // Load next level by build index after one second (in the middle of transition)
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

        // Destroy the transition object after the transition ends
        Destroy(levelTransitionObj, 2f);
    }

    IEnumerator LoadLevel(int buildIndex)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
