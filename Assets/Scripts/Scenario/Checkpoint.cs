using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public Sprite activatedSprite;
	public GameObject particle;

	public bool activated;

	void OnTriggerEnter2D(Collider2D other){

		if(other.tag == "Player" && !activated){
			GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
			Instantiate(particle,transform.position,Quaternion.identity);
			GetComponent<SpriteRenderer>().sprite = activatedSprite;
			activated = true;
			other.gameObject.GetComponent<Cat>().currentCheckpoint = transform.position;
		}
	}
}
