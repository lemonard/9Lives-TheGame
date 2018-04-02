using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroomCooldownIndicator : MonoBehaviour {

	public static BroomCooldownIndicator instance;
	
	public Image skillImage;
	public float cooldownTime;
	public bool onCooldown;

	private float fillAmount;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		skillImage.fillAmount = 1;
		fillAmount = 1;
	}

	void Update(){
		if(onCooldown){

			fillAmount += Time.deltaTime;
			skillImage.fillAmount = fillAmount/cooldownTime;

			if(skillImage.fillAmount >= 1f){
				onCooldown = false;
			}
		}

	}


	public void StartCooldown(){
		fillAmount = 0;
		skillImage.fillAmount = 0;
		onCooldown = true;

	}


}
