using System.Collections;
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


	// Use this for initialization
	void Start () {
		FindObjectOfType<Cat>().UIBeingShown = true;
		playerAccepted = true;

		acceptText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
		declineText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
			MoveRight ();
		} else if (Input.GetKeyDown (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
			MoveLeft ();
		}

		if(Input.GetKeyDown (confirmKey) || Input.GetButtonDown(confirmGamepadButton)){
			SelectCurrentOption();
		}
		
	}

	void MoveRight(){
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

		FindObjectOfType<Cat>().UIBeingShown = false;

		if(!playerAccepted){
			SceneManager.LoadScene(SceneToGoIfRefused);
		} 

		Destroy(gameObject);
	}
}
