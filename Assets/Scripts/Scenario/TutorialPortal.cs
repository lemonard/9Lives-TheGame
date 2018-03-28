using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPortal : MonoBehaviour {

	public int destinationSceneIndex;


	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			Enter();
		}

	}

	public void Enter(){
		SceneManager.LoadScene(destinationSceneIndex);
	}
}
