using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlight : MonoBehaviour {

    MeshRenderer mr;
    Color originalColor;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        originalColor = mr.material.color;
    }

    private void OnMouseDown()
    {
        mr.material.color = originalColor + new Color(.25f, .25f, .25f, 0f);
    }
    private void OnMouseUp()
    {
        mr.material.color = originalColor;
    }
}
