using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReferenceManager : MonoBehaviour {

	public static LevelReferenceManager instance;

	public Transform[] shipBackRailReferences;
	public Transform[] shipBackRailFloorReferences;

	void Awake(){
		instance = this;
	}

	public Transform GetShipBackRailReference(int height){

		

		return shipBackRailReferences[height];

	}

	public Transform GetShipBackRailFloorReference(int height){

		

		return shipBackRailFloorReferences[height];

	}
}
