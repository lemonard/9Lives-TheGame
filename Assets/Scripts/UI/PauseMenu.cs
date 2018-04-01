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

	public KeyCode moveRightKey;
	public KeyCode moveLeftKey;
	public KeyCode confirmKey;

	public string moveHorizontalGamepadAxis;
	public string confirmGamepadButton;

	public bool active;

	public Cat player;

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
			if (Input.GetKeyDown (moveRightKey) || (Input.GetAxis (moveHorizontalGamepadAxis) >= 0.5f)) {
				MoveRight ();
			} else if (Input.GetKeyDown (moveLeftKey) || (Input.GetAxis (moveHorizontalGamepadAxis) <= -0.5f)) {
				MoveLeft ();
			}

			if(Input.GetKeyDown (confirmKey) || Input.GetButtonDown(confirmGamepadButton)){
				SelectCurrentOption();
			}
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

