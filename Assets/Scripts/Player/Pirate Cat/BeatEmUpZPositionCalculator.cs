using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEmUpZPositionCalculator : MonoBehaviour {


	void Awake(){
		//spriteReference = transform.GetChild(0).transform;
	}

	// Use this for initialization
	void Start () {
		
	}

	
	// Update is called once per frame
	void Update () {

//		zPosition = -1;
//		for(int i = 0; i < laneReferences.Length; i++){
//			if(i == laneReferences.Length - 1){
//				if(spriteReference.position.y > laneReferences[i].position.y){
//					break;
//				}
//
//			}else{
//				if(spriteReference.position.y > laneReferences[i].position.y && spriteReference.position.y <= laneReferences[i + 1].position.y){
//
//					break;
//				}
//			}
//
//			zPosition++;
//
//			if(zPosition == 0){
//				zPosition++;
//			}
//		}
//
//		spriteReference.transform.position = new Vector3(spriteReference.transform.position.x,spriteReference.transform.position.y,zPosition);
		
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.5f);
	}

}
