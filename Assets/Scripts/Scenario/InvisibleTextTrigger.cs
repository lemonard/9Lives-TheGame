using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisibleTextTrigger : MonoBehaviour {

	public float timeToStayRevealed;

	public bool isRevealed;

	private Animator myAnimator;
	public Text myText;

	private float revealedTimestamp;

	void Awake(){
		myText.enabled = false;
		isRevealed = false;
		revealedTimestamp = 0;
		myAnimator = myText.gameObject.GetComponent<Animator>();
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
			revealedTimestamp = 0;
			myAnimator.SetBool("fadeIn",false);
		}

	}


}
