using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

	public static ArenaManager instance;
	public Arena currentActiveArena;

	void Awake(){
		instance = this;
	}


}
