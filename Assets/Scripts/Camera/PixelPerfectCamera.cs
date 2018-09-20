using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour {

	public float pixelsPerUnit = 100;
	public float snapValue;
	// Use this for initialization
	void Awake () {
		 GetComponent<Camera>().orthographicSize = ((Screen.height / 2f) / 100f);
		 snapValue = 1f / pixelsPerUnit;
	}

	void FixedUpdate(){

		transform.position = new Vector3(
			(float)Mathf.Round(transform.parent.position.x / snapValue) * snapValue,
			(float)Mathf.Round(transform.parent.position.y / snapValue) * snapValue,
			transform.parent.position.z
		);
	} 

}
