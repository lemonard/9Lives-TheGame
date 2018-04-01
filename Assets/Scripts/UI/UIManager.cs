using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public PauseMenu pauseMenu;

	public KeyCode openPauseMenuKey;
	public string openPauseMenuGamepadButton;

	private Cat player;

	void Awake(){
		pauseMenu.gameObject.SetActive(true);
		player = FindObjectOfType<Cat>();
		pauseMenu.player = player;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown (openPauseMenuKey) || Input.GetButtonDown(openPauseMenuGamepadButton)){

			if(pauseMenu.active){
				pauseMenu.Deactivate();
				pauseMenu.gameObject.SetActive(false);
			} else {
				pauseMenu.gameObject.SetActive(true);
				pauseMenu.active = true;
				pauseMenu.Activate();
			}
		}
	}



}
