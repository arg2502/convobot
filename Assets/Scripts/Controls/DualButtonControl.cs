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
            value += 0.1f;
        } else {
            value -= 0.1f;
        }

		base.UpdateFace();
	}
}