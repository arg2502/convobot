using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyebrow : FacePart {

    public enum EyebrowState { NEUTRAL, LOWERED, RAISED }
    public EyebrowState eyebrowState;

    public enum WhichEyebrow { LEFT, RIGHT }
    public WhichEyebrow whichEyebrow; // set in instructor

    // used in determining which way the 
    public int BrowSide { get { return (whichEyebrow == WhichEyebrow.LEFT) ? 1 : -1; } }

    float originalZRot; // base new rotation off of the original neutral position
    float rotateMult = 30f; // determines how far the brow can rotate. Arbitrariily chosen.

    void Start()
    {
        originalZRot = transform.rotation.z;
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
        var rot = transform.rotation;
        var quat = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, originalZRot + (value * rotateMult * BrowSide));
        transform.rotation = quat;
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
