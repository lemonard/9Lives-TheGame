using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPortal : MonoBehaviour {

	public int destinationSceneIndex;
	public KeyCode enterPortalKey;
	public string enterPortalGamepadButton;

	private bool catIsNear;

	void Update(){
		if(catIsNear){
			if(Input.GetKeyDown (enterPortalKey) || Input.GetButtonDown(enterPortalGamepadButton)){
				Enter();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			catIsNear = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			catIsNear = false;
		}

	}

	public void Enter(){
		SceneManager.LoadScene(destinationSceneIndex);
	}
}
