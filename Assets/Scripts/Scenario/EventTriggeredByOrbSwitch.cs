using UnityEngine;
using System.Collections;

public class EventTriggeredByOrbSwitch : MonoBehaviour {

	public SwitchOrb mySwitch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mySwitch.turnedOn){
			Activate();
		}
	}

	void Activate(){
		Destroy( gameObject );
    }
}
