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
    float prevValue;
    private bool assigned = false;
    public bool Assigned { get { return assigned; } set { assigned = value; } }

    // keep track of how many states each part has
    protected int numOfStates;
    public int NumOfStates { get { return numOfStates; } }

    public AudioClip sfx { get; set; }

    public void UpdateState(float _value, bool playSound) 
    {
        prevValue = (Mathf.Round(value * 10f) / 10f);
        value = Mathf.Clamp(_value, MIN_VALUE, MAX_VALUE);
        //print("value: " + value);
        
        float roundedValue = (Mathf.Round(_value * 10f) / 10f);
        if (playSound && prevValue != roundedValue)
            PlaySFX();
    }

    void PlaySFX()
    {
        AudioManager.instance.PlaySFX(sfx);
    }
}
