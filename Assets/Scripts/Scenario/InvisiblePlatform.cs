using UnityEngine;
using System.Collections;

public class InvisiblePlatform : MonoBehaviour {

	public float timeToStayRevealed;

	public bool isRevealed;

	private SpriteRenderer mySpriteRenderer;
	private Animator myAnimator;
	private float revealedTimestamp;

	void Awake(){
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		mySpriteRenderer.enabled = false;
		isRevealed = false;
		revealedTimestamp = 0;
		myAnimator = GetComponent<Animator>();
	}


	// Update is called once per frame
	void Update () {

		if(isRevealed && revealedTimestamp == 0){
			revealedTimestamp = Time.time + timeToStayRevealed;
			if(myAnimator){
				myAnimator.SetBool("fadeIn",true);
			}
		}

		if(Time.time > revealedTimestamp ){
			isRevealed = false;
			myAnimator.SetBool("fadeIn",false);
			revealedTimestamp = 0;
		}

	}

}
