using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangingJewelSwitch : MonoBehaviour {

	public StatueColor currentColor;

	public Color[] spriteColors;

	public GameObject[] objectsToInteract;

	public StatueColor desiredColor;

	private SpriteRenderer mySpriteRenderer;
	// Use this for initialization
	void Awake(){
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () {
		DefineColor();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentColor == desiredColor){
			ActivateObjects();
		}
	}

	public void ChangeColorTo(StatueColor color){

		if(color == StatueColor.Red){

			if(currentColor == StatueColor.Blue){

				currentColor = StatueColor.Purple;

			}else if(currentColor == StatueColor.Green){

				currentColor = StatueColor.Yellow;

			}else if(currentColor != StatueColor.Purple && currentColor != StatueColor.Yellow){

				currentColor = color;
			}
		}else if(color == StatueColor.Blue){

			if(currentColor == StatueColor.Red){

				currentColor = StatueColor.Purple;

			}else if(currentColor == StatueColor.Green){

				currentColor = StatueColor.Cyan;

			}else if(currentColor != StatueColor.Purple  && currentColor != StatueColor.Cyan){

				currentColor = color;
			}
		}else if(color == StatueColor.Green){

			if(currentColor == StatueColor.Red){

				currentColor = StatueColor.Yellow;

			}else if(currentColor == StatueColor.Blue){

				currentColor = StatueColor.Cyan;

			}else if(currentColor != StatueColor.Yellow && currentColor != StatueColor.Cyan){

				currentColor = color;
			}
		}else{
			currentColor = color;
		}


		DefineColor();
	}

	void DefineColor(){

		switch(currentColor){

			case(StatueColor.Red):
			mySpriteRenderer.color = spriteColors[0];
				break;

			case(StatueColor.Cyan):
			mySpriteRenderer.color = spriteColors[1];
				break;

			case(StatueColor.Purple):
			mySpriteRenderer.color = spriteColors[2];
				break;

			case(StatueColor.Blue):
			mySpriteRenderer.color = spriteColors[3];
				break;

			case(StatueColor.White):
			mySpriteRenderer.color = spriteColors[4];
				break;

			case(StatueColor.Green):
				mySpriteRenderer.color = spriteColors[5];
			break;

			case(StatueColor.Yellow):
				mySpriteRenderer.color = spriteColors[6];
			break;

		}

	}

	void ActivateObjects(){
		for(int i = 0; i < objectsToInteract.Length; i++){
			objectsToInteract[i].SetActive(false);
		}

	}
}
