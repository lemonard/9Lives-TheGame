using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueColorChangingSwitch : MonoBehaviour {

	public Color activatedColor;
	public Color deactivatedColor;

	public EyeStatue[] statuesToChangeColor;

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

		for(int i = 0; i < statuesToChangeColor.Length; i++){
			statuesToChangeColor[i].SwitchColorsForward();
		}

		if(!changeStatuesColorOnce){
			StartCoroutine(DeactivateAfterSeconds());
		}
	}

	public void Deactivate(){
		mySpriteRenderer.color = deactivatedColor;
		activated = false;
		for(int i = 0; i < statuesToChangeColor.Length; i++){
			
			statuesToChangeColor[i].SwitchColorsBackwards();
		}
	}

	IEnumerator DeactivateAfterSeconds(){

		yield return new WaitForSeconds(0.5f);
		mySpriteRenderer.color = deactivatedColor;
	
	}
}
