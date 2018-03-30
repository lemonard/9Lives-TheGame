using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour {



    void Start()
    {
        StartCoroutine(selfDestruct());
    }


    // Use this for initialization
   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            //collision.gameObject.GetComponent<MagicProjectile>().goRight = !collision.gameObject.GetComponent<MagicProjectile>().goRight;
        }
    }*/

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
