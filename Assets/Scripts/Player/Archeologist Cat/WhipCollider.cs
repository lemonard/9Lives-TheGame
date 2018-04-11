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

	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<Enemy>()){
			if(charged){
				other.GetComponent<Enemy>().wasTurned = true;
			}
		}else if(other.GetComponent<PullableObject>()){
			if(charged){
				ArcheologistCat cat = GetComponentInParent<ArcheologistCat>();
				cat.StartPulling(other);
			}
		}

	}

}
