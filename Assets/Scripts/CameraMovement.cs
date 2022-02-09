using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform playerTransform;

    private float cameraDistance = 15f;
    public float CameraDistance
    {
        get
        {
            return cameraDistance;
        }
        private set
        {
            cameraDistance = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the player transform
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Keep the camera right above the player
        transform.position = new Vector3(playerTransform.position.x, CameraDistance, playerTransform.position.z);
    }
}
