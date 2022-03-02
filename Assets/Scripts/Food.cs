using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int healthHealed;
    private float rotateSpeed = 50;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
