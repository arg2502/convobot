using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : Control {

    // slider specific
    Slider uiSlider;
    public GameObject modelSlide;
    Animator slideAnimator;
    bool soundIsPlaying = false;

    void Start()
    {
        uiSlider = GetComponent<Slider>();
        uiSlider.minValue = MIN_VALUE;
        uiSlider.maxValue = MAX_VALUE;
        uiSlider.value = NEUTRAL;        
        
        if (modelSlide != null)
        {
            slideAnimator = modelSlide.GetComponent<Animator>();
            slideAnimator.speed = 0;
            //UpdateModelSlider();
            //slideAnimator.Play("Slider", -1, 0);
        }

        base.Start();
    }
	
	new void Update()
	{        
        if (canMove)
        {
            value = uiSlider.value; // -1 to 1
            float pitch = (value + 1.01f) / 2f; // 0.01 to 1.01 (really 1 max though)         
            if (!soundIsPlaying)
            {
                AudioManager.instance.PlaySFX(sfx, false, true, pitch);
                soundIsPlaying = true;
            }
            if (AudioManager.instance.GetPlayingSource(sfx) != null)
                AudioManager.instance.GetPlayingSource(sfx).pitch = pitch;
        }
        else
        {
            if (soundIsPlaying)
            {
                soundIsPlaying = false;
                AudioManager.instance.StopSource(sfx);
            }

            // IF LEVEL IS 3
            value += Skew;
            uiSlider.value = value;
        }
        //var uiPos = uiSlider.handleRect.;
        //print(uiPos);
        UpdateModelSlider();

		base.Update();
	}	

    void UpdateModelSlider()
    {
        if (slideAnimator != null)
        {
            var time = (value + 1) / 2f;
            if (time >= 1f) time = 0.99f;
            else if (time <= 0f) time = 0f;
            var stateName = "Slider";
            if (gameObject.name.Contains("B"))
                stateName += "B";
            slideAnimator.Play(stateName, -1, time);
        }
    }
}
