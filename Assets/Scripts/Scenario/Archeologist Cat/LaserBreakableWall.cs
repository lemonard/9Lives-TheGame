using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBreakableWall : MonoBehaviour {

	public StatueColor currentColor;
	public bool explodeRight;

	public void Break(){
		Animator animator = GetComponent<Animator>();

		if(explodeRight){
			animator.SetBool("right", true);
		}else{
			animator.SetBool("right", false);
		}

		animator.SetBool("break", true);

	}

	void Disappear(){
		Destroy(gameObject);
	}

}
