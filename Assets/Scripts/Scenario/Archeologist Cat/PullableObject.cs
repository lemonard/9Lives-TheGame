using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullableObject : MonoBehaviour {

	public bool isBeingPulled;

	public Sprite regularSprite;
	public Sprite wrappedSprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void WrapObject(){
		GetComponentInParent<SpriteRenderer> ().sprite = wrappedSprite;
	}

	public void UnwrapObject(){
		GetComponentInParent<SpriteRenderer> ().sprite = regularSprite;
	}
}
