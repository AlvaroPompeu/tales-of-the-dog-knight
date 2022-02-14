using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    
    private float disappearRate = 2f;
    private float moveRate = 1f;

    private void Start()
    {
        // Get the text mesh pro component
        textMesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Fade();
        MoveUp();
    }

    private void Fade()
    {
        textMesh.alpha -= disappearRate * Time.deltaTime;

        if (textMesh.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.forward * moveRate * Time.deltaTime, Space.World);
    }
}
