using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	private bool isDropped;
	private bool flying;
	private Vector3 destinationPoint;

	// Update is called once per frame
	void Update () {
//		if (isDropped) {
//			float step = 2 * Time.deltaTime;
//
//			GetComponent<Rigidbody2D> ().transform.position = Vector3.MoveTowards (transform.position, destinationPoint, step);
//		}

		if (flying) {
			transform.Rotate ( 0, 0, 1 * Time.deltaTime * 1800);
		}
	}

	public void MoveItem(){
		float randomDistanceX = Random.Range (-1f, 1f);
		float yForce = 4f;

		//destinationPoint = new Vector3 (transform.position.x + randomDistanceX, transform.position.y, transform.position.z);

		GetComponent<Rigidbody2D> ().gravityScale = 1;

		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (randomDistanceX, yForce), ForceMode2D.Impulse);
		flying = true;
		isDropped = true;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground") {
			flying = false;
			transform.rotation = Quaternion.identity;
		}
	}
}
