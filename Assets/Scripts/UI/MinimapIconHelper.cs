using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconHelper : MonoBehaviour
{
    void LateUpdate()
    {
        // Keep the icon always up
        transform.rotation = Quaternion.Euler(90f, 0, 0);
    }
}
