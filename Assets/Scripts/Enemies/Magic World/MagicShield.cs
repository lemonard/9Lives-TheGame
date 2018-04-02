using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour {


	public float shieldDuration;

    void Start()
    {
    	
        StartCoroutine(selfDestruct());
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
}
