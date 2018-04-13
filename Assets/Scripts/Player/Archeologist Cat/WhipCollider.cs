using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipCollider : MonoBehaviour {

	public bool charged;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Checking enemies and objects collisions
	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<Enemy>() || other.GetComponentInParent<Enemy>()){  //Some enemies components are built in different ways  
																				//The Masked Dog for example, has its colliders as Children of the main GameObject
			              													   //So it is needed to check that to get the enemy script correctly
			if(charged){
				if (other.GetComponent<Enemy> ()) {
					other.GetComponent<Enemy> ().wasTurned = true;
				} else if (other.GetComponentInParent<Enemy> ()) {
					other.GetComponentInParent<Enemy> ().wasTurned = true;
				}
			}
		}else if(other.GetComponent<PullableObject>()){
			if(charged){
				ArcheologistCat cat = GetComponentInParent<ArcheologistCat>();
				cat.StartPulling(other);
			}
		}

	}

}
