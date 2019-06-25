﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    protected const float MAX_VALUE = 1.0f; // the highest state the control (and face part) can be at
    protected const float MIN_VALUE = -1.0f; // the lowest state the control (and face part) can be at
    protected const float NEUTRAL = 0f;
    protected float value; // current state of gear -- starts at neutral
    protected bool canMove = false; // true if the control is being manipulated 

    // corresponding part that is affected by the control -- assigned in inspector
    public FacePart facePart;
    public AudioClip sfx;

    // OnPointer functions for when interacting with UI elements
    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleCanMove(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ToggleCanMove(false);
    }

    // OnMouse functions for when interacting to 3D GameObjects
    protected virtual void OnMouseDown()
    {
        ToggleCanMove(true);
    }

    protected virtual void OnMouseUp()
    {
        ToggleCanMove(false);
    }
     
    protected void Update()
    {
        if (value > MAX_VALUE)
            value = MAX_VALUE;
        if (value < MIN_VALUE)
            value = MIN_VALUE;

        if(canMove)
            UpdateFace();
    }

    // Updates the value in the corresponding face part, so that it can change position/rotation/states accordingly
    protected void UpdateFace()
    {
        facePart.UpdateState(value);
    }

    public void AssignControl(FacePart fp)
    {
        facePart = fp;
        facePart.sfx = sfx;
        facePart.Assigned = true;
    }

    public void ToggleCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

}
