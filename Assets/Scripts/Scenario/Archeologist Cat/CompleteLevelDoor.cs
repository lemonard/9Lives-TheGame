using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CompleteLevelDoor : MonoBehaviour {

	public CameraController camera; 
	public GameObject doorLayer1;
	public GameObject doorLayer2;
	public GameObject doorLayer3;
	public Collider2D finishLevelTrigger;
	public GameObject doorFocalPoint; 

	private Cat player;

	public bool isOpen;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Cat>();
		finishLevelTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isOpen){
			if(!doorLayer1.activeSelf && !doorLayer2.activeSelf && !doorLayer3.activeSelf ){
				Open();
			}
		}
	
	}

	void Open(){
		isOpen = true;
		finishLevelTrigger.enabled = true;
		StartCoroutine(DoCameraAnimation());
	}


	IEnumerator DoCameraAnimation(){
		player.DisableControls();

		yield return new WaitForSeconds(0.5f);
		camera.ChangeToTargetAndCenter(doorFocalPoint);
		yield return new WaitForSeconds(5f);
		camera.ReturnCameraToPlayerFollowing();
		yield return new WaitForSeconds(3f);
		camera.DeactivateAnimationMode();
		player.EnableControls();
	}
}
