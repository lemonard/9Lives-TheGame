﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeeTutorialPanel : MonoBehaviour {

	public Color selectedColor;
	public Color notSelectedColor;

	public Text acceptText;
	public Text declineText;

	public int SceneToGoIfRefused;

	private bool playerAccepted;

	public KeyCode moveRightKey;
	public KeyCode moveLeftKey;
	public KeyCode confirmKey;

	public string moveHorizontalGamepadAxis;
	public string confirmGamepadButton;

	public bool alreadyMoved;

	// Use this for initialization
	void Start () {
		FindObjectOfType<Cat>().DisableControls();
		playerAccepted = true;

		acceptText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
		declineText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f) && !alreadyMoved) {
			MoveRight ();
		} else if (Input.GetKeyDown (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f) && !alreadyMoved) {
			MoveLeft ();
		}

		if(Input.GetKeyDown (confirmKey) || Input.GetButtonDown(confirmGamepadButton)){
			SelectCurrentOption();
		}

		if((Input.GetAxis (moveHorizontalGamepadAxis) >= -0.05 && Input.GetAxis (moveHorizontalGamepadAxis) <= 0.05 )){
			alreadyMoved = false;
		}
		
	}

	void MoveRight(){
		alreadyMoved = true;
		if(playerAccepted){

			playerAccepted = false;
			acceptText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);
			declineText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
		}else{
			playerAccepted = true;
			acceptText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
			declineText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);
		}

	}

	void MoveLeft(){
		alreadyMoved = true;
		if(playerAccepted){
			playerAccepted = false;
			acceptText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);
			declineText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
		}else{
			playerAccepted = true;
			acceptText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
			declineText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);
		}

	}

	void SelectCurrentOption(){

		FindObjectOfType<Cat>().EnableControls();

		if(!playerAccepted){
			SceneManager.LoadScene(SceneToGoIfRefused);
		} 

		Destroy(gameObject);
	}
}
