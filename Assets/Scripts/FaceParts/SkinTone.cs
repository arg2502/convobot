using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTone : FacePart {

    MeshRenderer skinMat;
    Color originalColor;

    public enum SkinState { NEUTRAL, PALE, BLUSHING, FURIOUS }
    public SkinState skinState;

    void Start()
    {
        skinMat = GetComponentInChildren<MeshRenderer>();
        originalColor = Color.gray;
    }

    void Update()
    {
        CheckSkinTone();
        CheckState();
    }

    void CheckSkinTone()
    {
        // if positive, add red -- blushing, fury
        if (value >= 0)
            skinMat.material.color = originalColor + new Color(value/2f, -value/2f, -value/2f);
        // if negative, add white -- pale
        else
            skinMat.material.color = originalColor + new Color(-value/2f, -value/2f, -value/2f);
    }

    void CheckState()
    {
        if (value <= -0.5)
            skinState = SkinState.PALE;
        else if (value > -0.5 && value <= 0.1)
            skinState = SkinState.NEUTRAL;
        else if (value > 0.1 && value < 0.5)
            skinState = SkinState.BLUSHING;
        else if (value >= 0.5)
            skinState = SkinState.FURIOUS;
    }
}
