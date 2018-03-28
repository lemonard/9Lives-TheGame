using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagicPulse : MonoBehaviour {

	public void Disappear(){
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other){



		if(other.gameObject.tag == "InvisiblePlatform"){

			InvisiblePlatform invisiblePlatform = other.gameObject.GetComponent<InvisiblePlatform>();
			other.gameObject.GetComponent<SpriteRenderer>().enabled = true;

			invisiblePlatform.isRevealed = true;
		}
		else if(other.gameObject.tag == "InvisibleTextTrigger"){

			InvisibleTextTrigger invisibleText = other.gameObject.GetComponent<InvisibleTextTrigger>();
			invisibleText.myText.enabled = true;

			invisibleText.isRevealed = true;
		}
	}
}
