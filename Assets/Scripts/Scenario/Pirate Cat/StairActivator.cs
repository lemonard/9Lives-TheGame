using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairActivator : MonoBehaviour {

	public Transform stairStartPoint;
	public Transform stairEndPoint;
	public Transform catSpriteTransform;

	public Collider2D stairCollider;

	public Collider2D[] noStairColliders;

	public bool stairActivated;
	public bool catInsideStairArea;

	public float initialCatMaxY;
	public float initialCatMinY;

	public float maxReferenceDistanceBelowStairs = 0.4f;

	public float maxReferenceDistanceAboveStairs = 0.6f;
	public float minReferenceDistanceAboveStairs = 0.4f;

	private Transform currentStairActivationReferencePoint;
	// Use this for initialization
	void Start () {
		initialCatMaxY = catSpriteTransform.parent.GetComponent<BeatEmUpCatReference>().maxY;
		initialCatMinY = catSpriteTransform.parent.GetComponent<BeatEmUpCatReference>().minY;
	}
	
	// Update is called once per frame
	void Update () {

		if(!catInsideStairArea){
			ToogleCollider();
		}


		if(catInsideStairArea){ //Cat Inside Stair Area
			
			if(stairActivated){
				catSpriteTransform.parent.GetComponent<BeatEmUpCatReference>().maxY = maxReferenceDistanceAboveStairs;
				catSpriteTransform.parent.GetComponent<BeatEmUpCatReference>().minY = minReferenceDistanceAboveStairs;
			}else{
				catSpriteTransform.parent.GetComponent<BeatEmUpCatReference>().maxY = maxReferenceDistanceBelowStairs;
				catSpriteTransform.parent.GetComponent<BeatEmUpCatReference>().minY = initialCatMinY;
			}
		}


	}

	void ToogleCollider(){

		if(catSpriteTransform.position.x > stairEndPoint.position.x){
			currentStairActivationReferencePoint = stairEndPoint;
		}else if(catSpriteTransform.position.x < stairStartPoint.position.x){
			currentStairActivationReferencePoint = stairStartPoint;
		}

		if(catSpriteTransform.position.y >= currentStairActivationReferencePoint.position.y){
			stairActivated = true;
			stairCollider.enabled = true;
			foreach(Collider2D collider in noStairColliders){
				collider.enabled = false;
			}
		}else{
			stairActivated = false;
			stairCollider.enabled = false;
			foreach(Collider2D collider in noStairColliders){
				collider.enabled = true;
			}

		}

	}


	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			catInsideStairArea = true;

		}
	}


	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			catInsideStairArea = false;
			other.transform.parent.GetComponent<BeatEmUpCatReference>().maxY = initialCatMaxY;
			other.transform.parent.GetComponent<BeatEmUpCatReference>().minY = initialCatMinY;
		}
	}
}
