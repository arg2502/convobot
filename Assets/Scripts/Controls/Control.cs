using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    protected const float MAX_VALUE = 1.0f; // the highest state the control (and face part) can be at
    protected const float MIN_VALUE = -1.0f; // the lowest state the control (and face part) can be at
    protected const float NEUTRAL = 0f;
    protected float value; // current state of gear -- starts at neutral
    protected bool canMove = false; // true if the control is being manipulated 
    protected bool isPaused = false;
    protected float skewAmount = 0.1f;
    protected int skewDirection = 1;
    protected float Skew { get { return skewAmount * skewDirection * Time.deltaTime; } }
    protected float timer, timerStart;
    private int timerChangeMin = 4;
    private int timerChangeMax = 7;


    // corresponding part that is affected by the control -- assigned in inspector
    public FacePart facePart;
    public AudioClip sfx;

    protected void Start()
    {
        timerStart = Mathf.RoundToInt(Random.Range(timerChangeMin, timerChangeMax));
        timer = timerStart;
        skewDirection = Mathf.RoundToInt(Random.value * 100 % 2) == 0 ? 1 : -1;
    }

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

    private void TimerTick()
    {
        //timer -= Time.deltaTime;

        //if (timer <= 0f)
        //{
        //    SwitchTimer();
        //}
    }

    protected virtual void SwitchTimer()
    {
        //skewDirection *= -1;
        //timerStart = Mathf.RoundToInt(Random.Range(timerChangeMin, timerChangeMax));
        //timer = timerStart;
    }
     
    protected void Update()
    {
        TimerTick();
        value = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
        UpdateFace(canMove);
    }

    // Updates the value in the corresponding face part, so that it can change position/rotation/states accordingly
    protected void UpdateFace(bool playSound = true)
    {
        facePart.UpdateState(value, playSound);
    }

    public void AssignControl(FacePart fp)
    {
        facePart = fp;
        if (!(this is SliderControl))
            facePart.sfx = sfx;
        facePart.Assigned = true;
    }

    public void ToggleCanMove(bool _canMove)
    {
        if (!isPaused)
            canMove = _canMove;
    }

    public void PauseControl(bool pause)
    {
        isPaused = pause;
    }
}
