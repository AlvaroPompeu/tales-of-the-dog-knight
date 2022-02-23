using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossDamagePopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    
    private float disappearRate = 2f;
    private float moveRate = 20f;

    private void Start()
    {
        // Get the text mesh pro component
        textMesh = GetComponent<TextMeshProUGUI>();
    }

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
        transform.Translate(Vector3.up * moveRate * Time.deltaTime, Space.World);
    }
}
