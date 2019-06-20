using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    public float scaleUpSizeModifier = 1.1f;
    public void OnMouseEnter()
    {
        Debug.Log("here");
        transform.localScale += new Vector3(scaleUpSizeModifier, scaleUpSizeModifier, scaleUpSizeModifier); 
    }


    public void OnMouseExit()
    {
        Debug.Log("there");
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);  
    }

}
