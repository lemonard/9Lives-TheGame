using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class FinishLevelTrigger : CutsceneTrigger {

	protected CameraController mainCamera;

	public GameObject finishLevelText;
    public int destinationSceneIndex;
    public int hubSceneIndex;

    public CatType type;
    public bool lastLevel;

    // Use this for initialization
    protected override void Start () {

    	base.Start();

		mainCamera = FindObjectOfType<CameraController>();
	}

	protected override void Update ()
	{
		if(alreadyPlayed){
			if(timeline.time >= timeline.duration || timeline.state != PlayState.Playing){

				player.EnableControls();
				GameManager.instance.EndStage();

				if(lastLevel){
					GoToHUB();
				} else{
					GoToNextScene();
				}
			}
		}
	}
	
	protected override void OnTriggerEnter2D(Collider2D other){

		base.OnTriggerEnter2D(other);

		if(other.gameObject.tag == "Player"){
			mainCamera.follow = false;

		}

	}


    void GoToNextScene()
    {
        SceneManager.LoadScene(destinationSceneIndex);
    }

    void GoToHUB(){

		switch(type){

			case(CatType.MagicCat):
				GameManager.instance.magicCatStageCleared = true;
			break;

			case(CatType.PussInBoots):
				GameManager.instance.pussInBootsStageCleared = true;
			break;

			case(CatType.ArcheologistCat):
				GameManager.instance.archeologistStageCleared = true;
			break;

			case(CatType.PirateCat):
				GameManager.instance.pirateCatCleared = true;
			break;

			case(CatType.NinjaCat):
				GameManager.instance.ninjaStageCleared = true;
			break;

		}

    	SceneManager.LoadScene(hubSceneIndex);
    }

}
