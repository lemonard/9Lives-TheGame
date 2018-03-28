using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishLevelTrigger : MonoBehaviour {

	private Cat player;
	private CameraController mainCamera;
	private ScreenFade screenFade;

	public GameObject finishLevelText;
    public int destinationSceneIndex;

    // Use this for initialization
    void Start () {
		player = FindObjectOfType<Cat>();
		mainCamera = FindObjectOfType<CameraController>();
		screenFade = FindObjectOfType<ScreenFade>();
	}
	
	void OnTriggerEnter2D(Collider2D other){

		if(other.gameObject.tag == "Player"){
			player.finishedLevel = true;
			mainCamera.follow = false;
			StartCoroutine(FadeScreenOut());
		}

	}

	IEnumerator FadeScreenOut(){

		yield return new WaitForSeconds(1);
		screenFade.FadeOut();
		StartCoroutine(ShowFinishLevelText());
	}

	IEnumerator ShowFinishLevelText(){

		yield return new WaitForSeconds(1.2f);
		finishLevelText.SetActive(true);
        StartCoroutine(ReturnToHUB());

    }

    IEnumerator ReturnToHUB()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(destinationSceneIndex);
    }

}
