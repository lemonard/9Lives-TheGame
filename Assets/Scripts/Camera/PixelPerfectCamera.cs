using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		 GetComponent<Camera>().orthographicSize = ((Screen.height / 2f) / 100f);
	}
	

}
