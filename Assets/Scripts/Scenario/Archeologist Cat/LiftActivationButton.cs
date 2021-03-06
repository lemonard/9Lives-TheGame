﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftActivationButton : MonoBehaviour {

	public Lift lift;

	public Sprite activatedSprite;
	public Sprite deactivatedSprite;

	private SpriteRenderer mySpriteRenderer;

	void Awake(){
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player" || other.tag == "ButtomPressingObjects"){
			Activate();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player" || other.tag == "ButtomPressingObjects"){
			Deactivate();
		}
	}

	void Activate(){
		mySpriteRenderer.sprite = activatedSprite;
		if(!lift.isMoving){
			lift.Activate();
		}
	}

	void Deactivate(){
		mySpriteRenderer.sprite = deactivatedSprite;
	}
}
