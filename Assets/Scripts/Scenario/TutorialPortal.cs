using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPortal : MonoBehaviour {

	public int destinationSceneIndex;
	public KeyCode enterPortalKey;
	public string enterPortalGamepadButton;
	public ScreenFade screenFade;

	private bool catIsNear;

	void Update(){
		if(catIsNear){
			if(Input.GetKeyDown (enterPortalKey) || Input.GetButtonDown(enterPortalGamepadButton)){
				StartCoroutine(Enter());
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

	IEnumerator Enter(){
		screenFade.FadeOut ();
		yield return new WaitForSeconds (1.5f);
		SceneManager.LoadScene(destinationSceneIndex);
	}
}
