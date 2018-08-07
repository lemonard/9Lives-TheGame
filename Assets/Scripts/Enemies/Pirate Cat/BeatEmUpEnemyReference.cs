using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEmUpEnemyReference : MonoBehaviour {

	private CombableEnemy myEnemy;

	void Awake(){
		myEnemy = GetComponentInChildren<CombableEnemy>();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected virtual void OnCollisionEnter2D(Collision2D other){


		if(other.gameObject.tag == "Ground"){
			if(myEnemy.knockedDown){
				myEnemy.myAnimator.SetBool("knockedOnFloor", true);
			}

			if(myEnemy.GetComponent<Chihuahuarr>() && myEnemy.GetComponent<Chihuahuarr>().jumpAttacking){
				myEnemy.GetComponent<Chihuahuarr>().jumpAttacking = false;
				myEnemy.GetComponentInParent<Rigidbody2D>().velocity = Vector3.zero;
			}
		}

    }
}
