using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTone : FacePart {

    public List<Renderer> skinRenderers;
    Color originalColor;

    public enum SkinState { NEUTRAL, PALE, BLUSHING, FURIOUS, _COUNT }
    public SkinState skinState;

    void Start()
    {
		originalColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
        numOfStates = (int)SkinState._COUNT;
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
            skinRenderers.ForEach((r) => SetRed(r));            
        
        // if negative, add white -- pale
        else
            skinRenderers.ForEach((r) => SetWhite(r));
    }

    void SetRed(Renderer r)
    {                
        foreach(var mat in r.materials)
        {
            if (mat.name.Contains("skin"))
                mat.SetColor("_Color", originalColor + new Color(value / 2f, -value / 2f, -value / 2f));
        }        
    }

    void SetWhite(Renderer r)
    {
        foreach (var mat in r.materials)
        {
            if (mat.name.Contains("skin"))
                mat.SetColor("_Color", originalColor + new Color(-value / 2f, -value / 2f, -value / 2f));
        }
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
