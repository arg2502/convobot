using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyelids : FacePart {

    public GameObject lowerlid;
    public GameObject upperlid;

    float openPos;
    float closedPos = 0.5f;
    float originalYPos;

    public enum EyelidState { OPEN, SQUINTING, CLOSED }
    public EyelidState eyelidState;

	void Start () {
        openPos = upperlid.transform.localPosition.y;

        // set eyelids to halfway
        var middle = (openPos + closedPos) / 2f;

        var newUpper = upperlid.transform.localPosition;
        newUpper.y = middle;
        upperlid.transform.localPosition = newUpper;
        newUpper.y *= -1;
        lowerlid.transform.localPosition = newUpper;

        originalYPos = upperlid.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveLids();
        CheckState();
	}

    void MoveLids()
    {
        var newPos = upperlid.transform.localPosition;
        newPos.y = originalYPos + (value * (originalYPos - closedPos));
        upperlid.transform.localPosition = newPos;
        newPos.y *= -1;
        lowerlid.transform.localPosition = newPos;
    }

    void CheckState()
    {
        if (value > 0.6f)
            eyelidState = EyelidState.OPEN;
        else if (value <= -1f)
            eyelidState = EyelidState.CLOSED;
        else
            eyelidState = EyelidState.SQUINTING;
    }
}
