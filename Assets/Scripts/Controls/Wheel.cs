using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : Control {

    float sensitivity;
    Vector3 mouseReference;
    Vector3 mouseOffset;
    Vector3 rotation;
    GameObject wheelModel;
    float valueMult = 0.01f;

    private void Start()
    {
        sensitivity = 0.4f;
        rotation = Vector3.zero;
        wheelModel = transform.GetChild(0).gameObject;
        base.Start();
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        mouseReference = Input.mousePosition;

    }

    private new void Update()
    {
        UpdateRotation();

        base.Update();
    }

    void UpdateRotation()
    {
        if(canMove)
        {
            mouseOffset = (Input.mousePosition - mouseReference);
            rotation.x = -(mouseOffset.x + mouseOffset.y) * sensitivity;

            // if negative, then we're trying to increase
            if (rotation.x < 0 && value >= MAX_VALUE)
                return;
            if (rotation.x > 0 && value <= MIN_VALUE)
                return;

            wheelModel.transform.Rotate(rotation);
            mouseReference = Input.mousePosition;

            // now change the value
            var newValue = -(rotation.x) * valueMult;
            value += newValue;
        }
        //// IF LEVEL THREE
        //else
        //{
        //    value += Skew;
        //    if (value < MAX_VALUE && value > MIN_VALUE)
        //    {
        //        rotation.x = -Skew / valueMult;
        //        wheelModel.transform.Rotate(rotation);
        //    }
        //}
    }
}
