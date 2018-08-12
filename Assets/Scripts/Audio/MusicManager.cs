using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicClip{
	public AudioClip BGMOriginal;
	public AudioClip BGMLoop;
}

public class MusicManager : MonoBehaviour {

	public static MusicManager instance;

	public List<MusicClip> myMusicClips;

	private AudioSource audioPlayer;

	private Coroutine playLoopCoroutine;

	void Awake (){
		instance = this;

		audioPlayer = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		audioPlayer.loop = true;
		playLoopCoroutine = StartCoroutine (PlayMusicLooping(0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddSongAndPlay(MusicClip newMusicClip){
		myMusicClips.Add (newMusicClip);
		if (playLoopCoroutine != null) {
			StopCoroutine (playLoopCoroutine);
		}
		playLoopCoroutine = StartCoroutine(PlayMusicLooping(myMusicClips.Count - 1));
	}

	IEnumerator PlayMusicLooping(int musicListPlacement){
		if (myMusicClips [musicListPlacement].BGMLoop == null) {
			audioPlayer.clip = myMusicClips [musicListPlacement].BGMOriginal;
			audioPlayer.Play ();
		} else {
			audioPlayer.clip = myMusicClips [musicListPlacement].BGMOriginal;
			audioPlayer.Play ();
			yield return new WaitForSeconds (audioPlayer.clip.length);
			audioPlayer.clip = myMusicClips [musicListPlacement].BGMLoop;
			audioPlayer.Play ();
		}
	}
}
