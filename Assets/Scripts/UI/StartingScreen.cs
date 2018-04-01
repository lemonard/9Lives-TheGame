using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour {

	public Animator pressStartAnimator; 
	public KeyCode startKey;
	public string startGamePadButton;

	private bool pressedStart;
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown(startGamePadButton) || Input.GetKeyDown(startKey) && !pressedStart)
		{
			pressedStart = true;
			StartCoroutine(StartGame());
		}
	}

	IEnumerator StartGame(){
		pressStartAnimator.SetBool("startPressed",true);
		GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
		yield return new WaitForSeconds(0.4f);
		SceneManager.LoadScene (1);
	}
}
