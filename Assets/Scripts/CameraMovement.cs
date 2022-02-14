using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform playerTransform;

    private float minCameraDistance = 5f;
    private float maxCameraDistance = 15f;

    public float CameraDistance { get; private set; }

    void Start()
    {
        // Initialize camera distance with the max value
        CameraDistance = maxCameraDistance;

        // Get the player transform
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        ZoomCamera();
    }

    void LateUpdate()
    {
        // Keep the camera right above the player
        transform.position = new Vector3(playerTransform.position.x, CameraDistance, playerTransform.position.z);
    }

    private void ZoomCamera()
    {
        // Zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && CameraDistance > minCameraDistance)
        {
            CameraDistance--;
        }
        // Zoom out
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && CameraDistance < maxCameraDistance)
        {
            CameraDistance++;
        }
    }
}
