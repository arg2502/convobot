using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

	public Transform goalGizmoTransform;
	public Transform connectorEndTransform;

	private Animator animator;

	private Transform thisTransform;
	private Vector3 gizmoPosition;
	private Vector3 endPosition;
	//private Vector3 defaultPosition;

	private bool isTransitioning = false;
	private Quaternion connectorEndRotation;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator> ();
		thisTransform = GetComponent<Transform> ();
		//defaultPosition = this.gameObject.transform.position;

		connectorEndRotation = connectorEndTransform.rotation;

		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(isTransitioning)
		{
			connectorEndTransform.rotation = connectorEndRotation;
			//Debug.Log ("rotation after change is: " + connectorEndTransform.rotation.ToString ());
		}
	}

	public void Open(float speed, float transitionStep)
	{
		this.gameObject.SetActive (true);

		animator.speed = speed;
		animator.Play("ConnectorOpening1");

		StartCoroutine (AdjustPosition(connectorEndTransform, goalGizmoTransform, thisTransform, 1f / speed, transitionStep));
	}

	public void Close(float speed)
	{
		animator.speed = speed;
		animator.Play("ConnectorClosing1");

		Invoke ("HideMe", 1 / speed);
	}

	private IEnumerator AdjustPosition(Transform targetTransform, Transform goalTransform, Transform transformToAdjust, float duration, float step)
	{
		isTransitioning = true;

		yield return new WaitForSeconds(duration * 0.75f);

		float elapsedTime = 0f;

		float deltaX = 0f;
		float deltaY = 0f;
		float deltaZ = 0f;

		Vector3 targetPosition = targetTransform.position;
		Vector3 goalPosition = goalTransform.position;

		while (elapsedTime < duration * 0.5f && targetPosition != goalPosition)
		{
			targetPosition = targetTransform.position;
			goalPosition = goalTransform.position;

			deltaX = GetPositionDelta (targetPosition.x, goalPosition.x, step);
			deltaY = GetPositionDelta (targetPosition.y, goalPosition.y, step);
			deltaZ = GetPositionDelta (targetPosition.z, goalPosition.z, step);

			transformToAdjust.position += new Vector3 (deltaX, deltaY, deltaZ);

			//Debug.Log("current connector world position is: " + transformToAdjust.position.ToString());
			//Debug.Log ("elapsedTimeIs is: " + elapsedTime.ToString ());

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		isTransitioning = false;
	}

	private float GetPositionDelta(float current, float goal, float step)
	{
		float positionDelta = 0f;
		float diff = current - goal;

		if (Mathf.Abs(diff) > step)
		{
			if (diff < 0) 
			{ positionDelta = step; }
			else
			{ positionDelta = step * -1; }
		}

		else
		{
			positionDelta = diff;
		}

		return positionDelta;
	}

	private void HideMe()
	{
		this.gameObject.SetActive(false);
	}
}
