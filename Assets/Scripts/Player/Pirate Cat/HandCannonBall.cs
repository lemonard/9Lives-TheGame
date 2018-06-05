using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCannonBall : MonoBehaviour {

	public GameObject explosionEffectPrefab;

	public float speed; 
	public float launchForce; 

	public bool goRight;
	public Transform explosionPoint;
	public Transform positionToExplode;
	public Transform shadow;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, launchForce),ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		if(goRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
			shadow.position += Vector3.right * speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = false;
        } else {
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
			shadow.position += Vector3.left * speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = true;
        }

		if(explosionPoint.transform.position.y <= positionToExplode.position.y){
			Instantiate(explosionEffectPrefab,transform.position,Quaternion.identity);
			Destroy(transform.parent.gameObject);
        }
	}

//	void OnTriggerEnter2D(Collider2D other){
//		if(other.tag == "Ground"){
//
//			Instantiate(explosionEffectPrefab,transform.position,Quaternion.identity);
//
//			Destroy(gameObject);
//		}
//	}
}
