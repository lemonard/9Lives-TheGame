using UnityEngine;
using System.Collections;

public class SwitchOrb : MonoBehaviour {

	public bool turnedOn;

	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other){

		if(other.gameObject.tag == "PlayerProjectile"){
			turnedOn = !turnedOn;
			animator.SetBool("turnedOn",turnedOn);
		}

	}
}
