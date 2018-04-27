using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StatueColorChangingSwitch : MonoBehaviour {

	[System.Serializable]
	public struct StatueToInteract{
		public EyeStatue statue;
		public SpriteRenderer statueColorIndicator;
	};

	public Color activatedColor;
	public Color deactivatedColor;

	public StatueToInteract[] statues;

	private SpriteRenderer mySpriteRenderer;

	public bool changeStatuesColorOnce;

	public bool activated;

	void Awake(){
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	// Use this for initialization
	void Start () {
		mySpriteRenderer.color = deactivatedColor;


	}
	
	// Update is called once per frame
	void Update () {
		DefineIndicatorsColor();
	}

	public void ToggleActivation(){
		if(changeStatuesColorOnce){
			if(activated){
				Deactivate();
			}else{
				Activate();
			}
		}else{
			Activate();
		}
	}

	public void Activate(){
		mySpriteRenderer.color = activatedColor;
		activated = true;

		for(int i = 0; i < statues.Length; i++){
			statues[i].statue.SwitchColorsForward();
		}

		DefineIndicatorsColor();

		if(!changeStatuesColorOnce){
			StartCoroutine(DeactivateAfterSeconds());
		}
	}

	public void Deactivate(){
		mySpriteRenderer.color = deactivatedColor;
		activated = false;

		for(int i = 0; i < statues.Length; i++){
			statues[i].statue.SwitchColorsForward();
		}

		DefineIndicatorsColor();
	}

	public void DefineIndicatorsColor(){

		for(int i = 0; i < statues.Length; i++){
			
			statues[i].statueColorIndicator.color = new Color(statues[i].statue.currentStatueColor.r,statues[i].statue.currentStatueColor.g,statues[i].statue.currentStatueColor.b,statues[i].statue.currentStatueColor.a);
		}

	}

	IEnumerator DeactivateAfterSeconds(){

		yield return new WaitForSeconds(0.5f);
		mySpriteRenderer.color = deactivatedColor;
	
	}
}
