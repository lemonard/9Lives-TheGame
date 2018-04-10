using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChandelierCutsceneManager : MonoBehaviour {

	public static ChandelierCutsceneManager instance;

	public CameraController camera; 
	public GameObject chandelierFocalPoint;

	public PlayableDirector directorRight;
	public PlayableDirector directorLeft;

	private PlayableDirector activatedDirector;
	private Cat player;
	private bool alreadyPlayed;
	private GameObject chandelier;

	void Awake(){
		instance = this;

	}
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Cat>();
	}
	
	// Update is called once per frame
	void Update () {
		if(alreadyPlayed){
			
			if(activatedDirector.time >= activatedDirector.duration || activatedDirector.state != PlayState.Playing){
				player.EnableControls();
				alreadyPlayed = false;
				camera.ReturnCameraToPlayerFollowing();
				Destroy(chandelier);
			}
		}
	}

	public void StartTimeline(bool activateRight, GameObject chandelier){
		player.DisableControls();


		StartCoroutine(WaitToStart(activateRight));
	}

	IEnumerator WaitToStart(bool activateRight){

		yield return new WaitForSeconds(0.5f);
		camera.ChangeToTargetAndCenter(chandelierFocalPoint);

		if(activateRight){
			activatedDirector = directorRight;
			directorRight.Play();
		}else{
			activatedDirector = directorLeft;
			directorLeft.Play();
		}
		alreadyPlayed = true;	
	}

}
