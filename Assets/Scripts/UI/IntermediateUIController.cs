using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateUIController : MonoBehaviour
{
    public void CallLoadNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void CallQuit()
    {
        GameManager.Instance.Quit();
    }
}
