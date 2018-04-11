using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioButtom : MonoBehaviour {

	public Sprite activatedSprite;
	public Sprite deactivatedSprite;
	public GameObject[] objectsToInteract;

	private SpriteRenderer mySpriteRenderer;

	void Awake(){
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player" || other.tag == "ButtomPressingObjects"){
			Activate();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player" || other.tag == "ButtomPressingObjects"){
			Deactivate();
		}
	}

	void Activate(){
		mySpriteRenderer.sprite = activatedSprite;
		for(int i = 0; i < objectsToInteract.Length; i++){
			objectsToInteract[i].SetActive(false);
		}
	}

	void Deactivate(){
		mySpriteRenderer.sprite = deactivatedSprite;
		for(int i = 0; i < objectsToInteract.Length; i++){
			objectsToInteract[i].SetActive(true);
		}
	}
}
