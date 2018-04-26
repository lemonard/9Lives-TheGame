using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

	public GameObject flame;
	public RoomDarkness roomDarkness;

	public bool isActive;
	// Use this for initialization

	void Start(){
		flame.SetActive(false);
	}

	public void Activate(){
		if(!isActive){
			isActive = true;
			roomDarkness.IncreaseAmountOfTorches();
			flame.SetActive(true);
		}
	}

	public void Deactivate(){
		isActive = false;
		roomDarkness.DecreaseAmountOfTorches();
		flame.SetActive(false);
	}
}
