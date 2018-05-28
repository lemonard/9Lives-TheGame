using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillCooldownIndicator : MonoBehaviour {

	public static SkillCooldownIndicator instance; 	 //Singleton for the skill indicator
													//I used this to be able to call this code anywhere as there will be only one SkillCooldownIndicator in each scene. 
												   //I can call this using SkillCooldownIndicator.instance 
	
	public Image skillImage; //Image representing the skill on the UI. This image is the one that will fill up in front of the "on cooldown image" that is in the UI.
							//This image needs to be marked as "Filled" in the editor and in front of the one that will be used to represent that the skill in on cooldown.
						   //When the skill is used the filled amount will go to 0, so it will disappear and start filling back up. Giving the impression of a "charging" effect

	public float cooldownTime; //Time until the image fills back up.
	public bool onCooldown; 

	private float fillAmount; //The percentage of the image that is filled up

	public bool isRepeatableSkill;

	public int skillRepeatableUses = 3;
	private int skillRemainingUses; 

	public TextMeshProUGUI skillAmountText;

	//I have set the cooldownTime as the same that is used on the character code to let it use the skill again,
   //but I think it can control whether the character can use the skill or not by checking the onCooldown bool

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		skillImage.fillAmount = 1;
		fillAmount = 1;
		skillRemainingUses = skillRepeatableUses;

		if(isRepeatableSkill){
			skillAmountText.text = skillRemainingUses.ToString();
		}
	}

	void Update(){
		if(onCooldown){ // Logic to refill the image after the skill has been used

			fillAmount += Time.deltaTime;
			skillImage.fillAmount = fillAmount/cooldownTime;

			if(skillImage.fillAmount >= 1f){
				onCooldown = false;
				if(isRepeatableSkill){
					RefreshUses();
				}
			}
		}

	}

	//Just call startCooldown when the skill is used
	public void StartCooldown(){
		fillAmount = 0;
		skillImage.fillAmount = 0;
		onCooldown = true;

	}

	public void SpendSkillUse(){
		skillRemainingUses--;

		if(skillRemainingUses <= 0){
			StartCooldown();
			skillRemainingUses = 0;
		}

		skillAmountText.text = skillRemainingUses.ToString();

	}

	public void RefreshUses(){
		skillRemainingUses = skillRepeatableUses;
		skillAmountText.text = skillRepeatableUses.ToString();
	}


}
