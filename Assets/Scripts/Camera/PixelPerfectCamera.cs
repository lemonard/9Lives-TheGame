using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour {

	public float pixelsPerUnit = 100;

	// Use this for initialization
	void Awake () {
		 GetComponent<Camera>().orthographicSize = ((Screen.height / 2f) / 100f);
	}

	void LateUpdate(){

		transform.position = new Vector3(
			Mathf.Round(transform.parent.position.x * pixelsPerUnit) / pixelsPerUnit,
			Mathf.Round(transform.parent.position.y * pixelsPerUnit) / pixelsPerUnit,
			transform.parent.position.z
		);
	} 

}
