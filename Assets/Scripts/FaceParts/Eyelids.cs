using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyelids : FacePart {

    //public GameObject lowerlid;
    //public GameObject upperlid;

    //Animator animator;

    //float openPos;
    //float closedPos = 0.5f;
    //float middleYPos;

    SkinnedMeshRenderer smr;

    public enum EyelidState { OPEN, SQUINTING, CLOSED, _COUNT }
    public EyelidState eyelidState;

	void Start () {       
        //animator = GetComponent<Animator>();
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        numOfStates = (int)EyelidState._COUNT;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //MoveLids();
        CheckBlendState();
        CheckState();
	}

    void CheckBlendState()
    {
        // 0 = frown
        // 50 = neutral
        // 100 = smile

        // get the value, add 1, and multiply by 50 -- to translate to Blend State
        var newBlendState = (value + 1f) * 50f;
        smr.SetBlendShapeWeight(0, newBlendState); // 1 for open state
    }

    /*void MoveLids()
    {
        //var newPos = upperlid.transform.localPosition;
        //newPos.y = middleYPos + (value * (middleYPos - closedPos)); // some math was done to limit the lids to their open/closed positions
        //upperlid.transform.localPosition = newPos;
        //newPos.y *= -1;
        //lowerlid.transform.localPosition = newPos;

        var time = (value + 1) / 2f;
        if (time >= 1f) time = 0.99f;
        else if (time <= 0f) time = 0f;
        var stateName = "Eyelid";
        if (gameObject.name.Contains("Right"))
            stateName += "R";
        animator.Play(stateName, -1, time);
    }*/

    void CheckState()
    {
        if (value <= -0.9f)
            eyelidState = EyelidState.OPEN;
        else if (value > -0.9f && value <= 0.5f)
            eyelidState = EyelidState.SQUINTING;
        else
            eyelidState = EyelidState.CLOSED;
        /*if (value > 0.6f)
            eyelidState = EyelidState.OPEN;
        else if (value <= -1f)
            eyelidState = EyelidState.CLOSED;
        else
            eyelidState = EyelidState.SQUINTING;*/
    }
}
