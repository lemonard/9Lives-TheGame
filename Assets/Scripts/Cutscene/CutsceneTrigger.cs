using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour {

	public PlayableDirector timeline;

	protected bool alreadyPlayed;
	protected Cat player;

	// Use this for initialization
	protected virtual void Start () {
		timeline = GetComponent<PlayableDirector>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(alreadyPlayed){
			
			if(timeline.time >= timeline.duration || timeline.state != PlayState.Playing){
				player.EnableControls();
				Destroy(gameObject);
			}
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player" && !alreadyPlayed){
			player = other.gameObject.GetComponent<Cat>();
			player.DisableControls();
			timeline.Play();
			alreadyPlayed = true;

		}
	}


}
