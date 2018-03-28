using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyText : MonoBehaviour {

	private Text myText;
	private Cat player;

	void Awake(){
		myText = GetComponent<Text>();
		player = (Cat)FindObjectOfType<Cat>();
	}
	// Use this for initialization
	void Start () {
		
	}

	void Update(){
		myText.text = player.currencyAmount.ToString();
	}
}
