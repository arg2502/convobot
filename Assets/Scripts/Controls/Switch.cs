using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Control {

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
        UpdateSwitchPosition();
        base.Start();
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        if (!canMove) return;

        ChangeValue();
    }

    void ChangeValue()
    {
        if (value <= MIN_VALUE)
            value = NEUTRAL;
        else if (value == NEUTRAL)
            value = MAX_VALUE;
        else if (value >= MAX_VALUE)
            value = MIN_VALUE;

        UpdateSwitchPosition();
    }

    void UpdateSwitchPosition()
    {
        var time = (value + 1) / 2f;
        if (time >= 1f) time = 0.99f;
        var stateName = "Switch";
        if (gameObject.name.Contains("B"))
            stateName += "B";
        animator.Play(stateName, -1, time);
    }

    protected override void SwitchTimer()
    {
        ChangeValue();
        base.SwitchTimer();
    }
}
