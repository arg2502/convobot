using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyebrow : FacePart {

    public enum EyebrowState { NEUTRAL, LOWERED, RAISED, _COUNT }
    public EyebrowState eyebrowState;

    public enum WhichEyebrow { LEFT, RIGHT }
    public WhichEyebrow whichEyebrow; // set in instructor

    // used in determining which way the eyebrow rotates
    public int BrowSide { get { return (whichEyebrow == WhichEyebrow.LEFT) ? 1 : -1; } }

    float originalZRot; // base new rotation off of the original neutral position
    float rotateMult = 30f; // determines how far the brow can rotate. Arbitrariily chosen.

    Animator anim;

    void Start()
    {
        originalZRot = transform.rotation.z;
        numOfStates = (int)EyebrowState._COUNT;
        anim = GetComponent<Animator>();
        anim.speed = 0f;
    }

	void Update()
    {
        RotateBrow();
        CheckState();
    }

    /// <summary>
    /// Constantly updating the rotation of the eyebrow based off of the value
    /// </summary>
    void RotateBrow()
    {
        //var rot = transform.rotation;
        //var quat = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, originalZRot + (value * rotateMult * BrowSide));
        //transform.rotation = quat;
        var pos = (value + 1f) / 2f;
        if (pos >= 1) pos -= 0.01f; // weird thing where animator show 0 position when == 1
        anim.Play(BrowSide == 1 ? "L_brow" : "R_brow", 0, pos);
    }

    /// <summary>
    /// Changes the state of the eyebrow based off of the Control value.
    /// Currently, if at max, RAISED, if at min, LOWERED, everything else is NEUTRAL.
    /// </summary>
    void CheckState()
    {
        if (value <= MIN_VALUE)
            eyebrowState = EyebrowState.LOWERED;
        else if (value >= MAX_VALUE)
            eyebrowState = EyebrowState.RAISED;
        else
            eyebrowState = EyebrowState.NEUTRAL;
    }

}
