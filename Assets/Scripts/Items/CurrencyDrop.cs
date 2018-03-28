using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDrop : MonoBehaviour {

	public int yarnValue;

	private void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			Cat cat = other.gameObject.GetComponent<Cat> ();

			cat.currencyAmount += yarnValue;

			Destroy (gameObject);
		}
	}
}
