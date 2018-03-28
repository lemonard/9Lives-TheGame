using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public GameObject activeCat; // Current active cat
	public int catIndex; //Child index in unity
	public Vector3 catPosition;
	//public int life = 3;
	public bool isChangingCat;
	private CameraController cameraController;

	// Use this for initialization
	void Start () {
		cameraController = FindObjectOfType<CameraController>();
		isChangingCat = false;

		catIndex = 0;
		activeCat = transform.GetChild(catIndex).gameObject;	

	}
	
	// Update is called once per frame
	void Update () {

		catPosition = activeCat.transform.position;  //Keep track of the current position, so the next cat can appear on the right spot

		if(!isChangingCat && activeCat.GetComponent<Cat>().isNearStatue){

			if(Input.GetKeyDown(activeCat.GetComponent<Cat>().transformInHubKey) || Input.GetButtonDown(activeCat.GetComponent<Cat>().transformInHubGamepadButton)){
				if(!activeCat.GetComponent<Cat>().isDying){
					ChangeCat(activeCat.GetComponent<Cat>().nearestStatue.catIndexToTransform);
				}
			}
		}

	}

	public void ChangeCat(int catToChangeIndex){

		bool catWasLookingRight;

		isChangingCat = true;
		catWasLookingRight = activeCat.GetComponent<Cat>().isLookingRight;

		//Deactivate the current cat
		activeCat.SetActive(false);

		//Change the current active c
		activeCat = transform.GetChild(catToChangeIndex).gameObject;

		activeCat.GetComponent<Cat>().isLookingRight = catWasLookingRight;

		catIndex = catToChangeIndex;

		//Activate the next dragon
		activeCat.SetActive(true);

		activeCat.GetComponent<Cat>().ChangeLookingDirection();

		activeCat.transform.position = catPosition;

		cameraController.UpdateActiveCat();

		isChangingCat = false;
	}
}
