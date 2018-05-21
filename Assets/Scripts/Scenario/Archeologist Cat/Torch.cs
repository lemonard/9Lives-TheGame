using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

	public GameObject flame;
	public RoomDarkness roomDarkness;

	public bool isActive;
	public bool isSceneryTorch;
	// Use this for initialization

	void Start(){
		if (!isSceneryTorch) {
			flame.SetActive (false);
		}
	}

	public void Activate(){
		if (!isSceneryTorch) {
			if (!isActive) {
				isActive = true;
				roomDarkness.IncreaseAmountOfTorches ();
				flame.SetActive (true);
			}
		}
	}

	public void Deactivate(){
		if (!isSceneryTorch) {
			isActive = false;
			roomDarkness.DecreaseAmountOfTorches ();
			flame.SetActive (false);
		}
	}
}
