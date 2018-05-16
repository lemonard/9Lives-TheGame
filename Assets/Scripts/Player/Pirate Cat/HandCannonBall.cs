﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCannonBall : MonoBehaviour {

	public GameObject explosionEffectPrefabAir;
	public GameObject explosionEffectPrefabGround;

	public float speed; 
	public float launchForce; 

	public bool goRight;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, launchForce),ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		if(goRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = false;
        } else {
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = true;
        }
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Enemy"){
			print("Bati no inimigo");
			Destroy(gameObject);
		}else if(other.tag == "Ground"){
			print("Bati no chão");
			Destroy(gameObject);
		}
	}
}
