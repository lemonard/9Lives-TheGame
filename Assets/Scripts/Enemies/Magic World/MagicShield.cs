using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour {



    void Start()
    {
        StartCoroutine(selfDestruct());
    }

    //shield duration, change the number to alter duration
    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(this.gameObject);
    }
}
