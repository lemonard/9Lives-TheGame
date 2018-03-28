using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivatorHelper : MonoBehaviour {

	void Awake(){
		GetComponent<Image>().enabled = true;
	}
}
