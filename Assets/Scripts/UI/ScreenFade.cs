using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	public void FadeIn(){
		animator.SetBool("fadeOut",false);
	}

	public void FadeOut(){
		animator.SetBool("fadeOut",true);
	}


}
