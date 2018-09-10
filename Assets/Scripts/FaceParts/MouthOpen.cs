using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthOpen : FacePart {

    SkinnedMeshRenderer smr;

    public enum MouthOpenState { CLOSED, AJAR, OPEN }
    public MouthOpenState mouthOpenState;
    
    void Start ()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
    }
		
	void Update ()
    {
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
        smr.SetBlendShapeWeight(1, newBlendState); // 1 for open state
    }

    void CheckState()
    {
        if (value <= -0.9f)
            mouthOpenState = MouthOpenState.CLOSED;
        else if (value > -0.9f && value <= 0.5f)
            mouthOpenState = MouthOpenState.AJAR;
        else
            mouthOpenState = MouthOpenState.OPEN;
    }
}
