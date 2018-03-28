using UnityEngine;
using System.Collections;

public class PortalDistanceChecker : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			PlayerManager playerManager = FindObjectOfType<PlayerManager>();

			if(playerManager.catIndex != 0){
				playerManager.ChangeCat(0);
			}
		}
	}
}
