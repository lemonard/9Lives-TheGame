using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioClip itemCollectSound;

	private AudioSource myAudioSource;
	// Use this for initialization
	void Start () {
		instance = this;
		myAudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayItemCollectSound(){

	 float random = Random.Range(0.8f,2.5f);

	 myAudioSource.pitch = random;
	 myAudioSource.PlayOneShot(itemCollectSound);
	 
	}

	public void PlayDeathSound(AudioClip deathClip){
		myAudioSource.pitch = 1;
		myAudioSource.PlayOneShot(deathClip);
	}

	public void PlaySound(AudioClip clip){
		myAudioSource.pitch = 1;
		myAudioSource.PlayOneShot(clip);
	}
}
