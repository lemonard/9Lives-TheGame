using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundManager : MonoBehaviour {

	public AudioClip[] stepsSounds;
	public AudioClip[] jumpingSounds;
	public AudioClip landingSound;

	private AudioSource audioSource;

	private Coroutine stepsCoroutine;

	private bool stepsActivated;
	// Use this for initialization
	void Start () {
		audioSource = GetComponentInParent<AudioSource>();
	}
	
	public void StartStepsSound(){

		if(!stepsActivated){
			stepsActivated = true;
			stepsCoroutine = StartCoroutine(StartSteps());
		}

	}

	public void StopStepsSound(){

		stepsActivated = false;


	}

	public void PlayJumpingSound(){

		stepsActivated = false;

		int randomIndex = Random.Range(0,jumpingSounds.Length);
		audioSource.PlayOneShot(stepsSounds[randomIndex]);

	}

	public void PlayLandingSound(){

		audioSource.PlayOneShot(landingSound);

	}

	IEnumerator StartSteps(){

		int randomIndex;

		while(stepsActivated){

			randomIndex = Random.Range(0,stepsSounds.Length);
			audioSource.PlayOneShot(stepsSounds[randomIndex]);
			audioSource.clip = stepsSounds[randomIndex];
			yield return new WaitForSeconds(audioSource.clip.length * 3);

		}

		yield return null;
	}

}
