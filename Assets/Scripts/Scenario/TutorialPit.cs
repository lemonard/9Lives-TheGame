using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPit : MonoBehaviour {

	public GameObject returnPoint;

	void OnCollisionEnter2D(Collision2D other){
		
		if(other.gameObject.GetComponent<Cat>() != null){
			GameObject cat = other.gameObject;
			cat.transform.position = returnPoint.transform.position;
		}
	}
}
