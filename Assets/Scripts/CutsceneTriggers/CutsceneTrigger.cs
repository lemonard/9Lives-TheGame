using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {

	public PlayableDirector timeline;

	private bool alreadyPlayed;
	private Cat player;

	// Use this for initialization
	void Start () {
		timeline = GetComponent<PlayableDirector>();
	}
	
	// Update is called once per frame
	void Update () {
		if(alreadyPlayed){
			if(timeline.state != PlayState.Playing){
				player.EnableControls();
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player" && !alreadyPlayed){
			player = other.gameObject.GetComponent<Cat>();
			player.DisableControls();
			timeline.Play();
			alreadyPlayed = true;

		}
	}


}
