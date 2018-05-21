using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

	public DialogScript dialog;
	public bool startMovingDialog;



	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			dialog.StartDialog(startMovingDialog);
			Destroy(gameObject);
		}
	}
	

}
