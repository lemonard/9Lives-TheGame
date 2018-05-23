using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour {


	public float shieldDuration;
	public bool canReflect;

	public WizardDog myMagicDog;

    void Start()
    {
		myMagicDog = GetComponentInParent<WizardDog> ();
        StartCoroutine(selfDestruct());
    }

	void Update(){
		if(myMagicDog.life <= 0){
			Disappear ();
		}
	}

    //shield duration, change the number to alter duration
    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(shieldDuration);
        GetComponent<Animator>().SetBool("magicShieldEnd",true);
    }

    void Disappear(){
		Destroy(this.gameObject);
    }

	void ActivateReflect(){
		canReflect = true;
	}
}
