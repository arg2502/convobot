using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorController : MonoBehaviour {

	public Connector[] connectorArray;

	public float transitionSpeed = 3f;
	public float randomThreshold = 0.1f;

	public float positionAdjustStrength = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.S))
		{
			OpenControls ();
		}

		if(Input.GetKey(KeyCode.A))
		{
			CloseControls ();
		}
	}

	public void OpenControls()
	{
		foreach (Connector connector in connectorArray)
		{
			connector.Open(transitionSpeed * Random.Range (1 - randomThreshold, 1 + randomThreshold), positionAdjustStrength);
		}
	}

	public void CloseControls()
	{
		foreach (Connector connector in connectorArray)
		{
			connector.Close(transitionSpeed * Random.Range (1 - randomThreshold, 1 + randomThreshold));
		}
	}
}
