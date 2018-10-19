using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePart : MonoBehaviour {

    // Blantantly stolen values from Control.cs
    // so not the way to do it, but we'll clean it up later
    protected const float MAX_VALUE = 1.0f; // the highest state the control (and face part) can be at
    protected const float MIN_VALUE = -1.0f; // the lowest state the control (and face part) can be at
    protected const float NEUTRAL = 0f;

    protected float value;
    private bool assigned = false;
    public bool Assigned { get { return assigned; } set { assigned = value; } }

    // keep track of how many states each part has
    protected int numOfStates;
    public int NumOfStates { get { return numOfStates; } }

    public void UpdateState(float _value) 
    {
        value = _value; 
    }
}
