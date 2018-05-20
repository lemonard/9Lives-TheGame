using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour {

	public bool isBeingPushed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartPushingObject(Cat cat){
		gameObject.transform.SetParent(cat.transform);
	}

	public void StopPushingObject(){
		gameObject.transform.SetParent(null);
	}
}
