using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDarkness : MonoBehaviour {

	public Torch[] torchesNeeded;

	public int amountOfTorchesNeeded;
	public int currentAmountOfTorchesLit = 0;
	public EyeStatue[] disabledEyeStatues;

	public bool vanished;

	private Animator myAnimator;
	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
		amountOfTorchesNeeded = torchesNeeded.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncreaseAmountOfTorches(){
		currentAmountOfTorchesLit++;
		if(currentAmountOfTorchesLit >= amountOfTorchesNeeded){
			vanished = true;
			myAnimator.SetBool("vanish",true);
			for(int i = 0; i < disabledEyeStatues.Length; i++){
				disabledEyeStatues[i].Enable();
			}
		}
	}

	public void DecreaseAmountOfTorches(){
		currentAmountOfTorchesLit--;
		if(currentAmountOfTorchesLit <= amountOfTorchesNeeded && vanished){
			vanished = false;
			myAnimator.SetBool("vanish",false);
			for(int i = 0; i < disabledEyeStatues.Length; i++){
				disabledEyeStatues[i].Disable();
			}
		}
	}
}
