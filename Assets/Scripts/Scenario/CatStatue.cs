using UnityEngine;
using System.Collections;

public class CatStatue : MonoBehaviour {

	public int catIndexToTransform;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			Cat player = other.gameObject.GetComponent<Cat>();

			player.isNearStatue = true;
			player.nearestStatue = this;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			Cat player = other.gameObject.GetComponent<Cat>();

			player.isNearStatue = false;
			player.nearestStatue = null;
		}
	}
}
