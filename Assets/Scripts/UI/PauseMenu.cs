using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public Color selectedColor;
	public Color notSelectedColor;

	public Text acceptText;
	public Text declineText;

	public int titleSceneIndex;

	private bool playerAccepted;

	public KeyCode moveUpKey;
	public KeyCode moveDownKey;
	public KeyCode confirmKey;

	public string moveVerticalGamepadAxis;
	public string confirmGamepadButton;

	public bool active;

	public Cat player;

	public bool alreadyMoved;

	// Use this for initialization
	void Start () {
		active = false;
		playerAccepted = true;
		acceptText.color = new Color(selectedColor.r,selectedColor.g,selectedColor.b);
		declineText.color = new Color(notSelectedColor.r,notSelectedColor.g,notSelectedColor.b);
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if(active){
			if (Input.GetKeyDown (moveUpKey) || (Input.GetAxis (moveVerticalGamepadAxis) >= 0.5f ) && !alreadyMoved) {
				MoveUp ();
			} else if (Input.GetKeyDown (moveDownKey) || (Input.GetAxis (moveVerticalGamepadAxis) <= -0.5f) && !alreadyMoved) {
				MoveDown ();
			}

			if(Input.GetKeyDown (confirmKey) || Input.GetButtonDown(confirmGamepadButton)){
				SelectCurrentOption();
			}

			if((Input.GetAxis (moveVerticalGamepadAxis) >= -0.05 && Input.GetAxis (moveVerticalGamepadAxis) <= 0.05 )){
				alreadyMoved = false;
			}
		}
		
	}

	void MoveUp(){
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

	void MoveDown(){
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

		player.EnableControls();

		if(!playerAccepted){
			SceneManager.LoadScene(titleSceneIndex);
		} 

		Deactivate();
		gameObject.SetActive(false);
	}

	public void Activate(){
		Time.timeScale = 0f;
		active = true;
		player.DisableControls();

	}

	public void Deactivate(){
		Time.timeScale = 1f;
		player.EnableControls();
		active = false;
	}
}

