using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthSmile : FacePart {

    SkinnedMeshRenderer smr;

    public enum MouthSmileState { NEUTRAL, FROWN, SLIGHTFROWN, SMIRK, SMILE }
    public MouthSmileState mouthSmileState;

    void Start()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Update()
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
        smr.SetBlendShapeWeight(0, newBlendState);
    }

    void CheckState()
    {
        if (value < -0.8f)
            mouthSmileState = MouthSmileState.FROWN;
        else if (value >= -0.8f && value < -0.1f)
            mouthSmileState = MouthSmileState.SLIGHTFROWN;
        else if (value >= -0.1f && value < 0.1f)
            mouthSmileState = MouthSmileState.NEUTRAL;
        else if (value >= 0.1f && value < 0.8f)
            mouthSmileState = MouthSmileState.SMIRK;
        else if (value >= 0.8f)
            mouthSmileState = MouthSmileState.SMILE;
    }
}
