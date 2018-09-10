using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DualButtonControl : Control {

    public Button UpButton, DownButton;

    void Start()
    {
        Button up   = UpButton.GetComponent<Button>();
        Button down = DownButton.GetComponent<Button>();
        up.onClick.AddListener(delegate {updateValue("Up"); });
        down.onClick.AddListener(delegate {updateValue("Down"); });
    }
	
	void updateValue(string direction)
	{
        if(direction == "Up") {
            if (value >= MAX_VALUE)
                return;

            value += 0.1f;
            if (value > MAX_VALUE) value = MAX_VALUE;
        } else {
            if (value <= MIN_VALUE)
                return;

            value -= 0.1f;
            if (value < MIN_VALUE) value = MIN_VALUE;
        }
        print(value);
		base.UpdateFace();
	}
}