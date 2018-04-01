using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

	public ScreenFade screenFade;
	public int destinationSceneIndex;

	void Awake(){
		screenFade = FindObjectOfType<ScreenFade>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			HouseCat player = other.gameObject.GetComponent<HouseCat>();

			player.isNearPortal = true;
			player.nearestPortal = this;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			HouseCat player = other.gameObject.GetComponent<HouseCat>();

			player.isNearPortal = false;
			player.nearestPortal = null;
		}
	}

	public IEnumerator Enter(){
		screenFade.FadeOut();
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(destinationSceneIndex);
	}
}
