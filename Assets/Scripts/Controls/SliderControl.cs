using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : Control {

    // slider specific
    Slider uiSlider;

    void Start()
    {
        uiSlider = GetComponent<Slider>();
        uiSlider.minValue = MIN_VALUE;
        uiSlider.maxValue = MAX_VALUE;
        uiSlider.value = NEUTRAL;
    }
	
	new void Update()
	{
		if(canMove)
            value = uiSlider.value;		

		base.Update();
	}	
}
