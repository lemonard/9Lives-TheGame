using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour {

	public MusicClip newMusicClips;

	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			MusicManager.instance.AddSongAndPlay (newMusicClips);
			Destroy (gameObject);
		}
	}
}
