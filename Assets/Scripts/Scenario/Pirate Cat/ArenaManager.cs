using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

	public static ArenaManager instance;
	public Arena currentActiveArena;


	void Awake(){
		instance = this;
	}

	public void ResetArena(){


		if(currentActiveArena != null){
			CameraController.instance.follow = true;
			currentActiveArena.ResetArena();
			currentActiveArena = null;
		}



	}
}
