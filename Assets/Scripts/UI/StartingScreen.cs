using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour {

	public KeyCode startKey;
	public string startGamePadButton;

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown(startGamePadButton) || Input.GetKeyDown(startKey))
		{
			SceneManager.LoadScene (1);
		}
	}
}
