using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : Control {

    // slider specific
    Slider uiSlider;
    public GameObject modelSlide;
    Animator slideAnimator;

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
    }
	
	new void Update()
	{
		if(canMove)
            value = uiSlider.value;

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
            slideAnimator.Play("Slider", -1, time);
        }
    }
}
