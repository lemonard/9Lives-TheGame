using UnityEngine;
using System.Collections;

public class EnemyDamageCollider : MonoBehaviour {



    void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Player"){

			Cat cat = other.gameObject.GetComponent<Cat>();

			if(!cat.invulnerable){
				cat.life -= GetComponentInParent<Enemy>().damage;
				cat.bounceBackAfterReceivingDamage = true;
				cat.sourceOfDamagePosition = gameObject.transform.position;
				cat.receivedDamage = true;
			} 
		 

		}
	}
 

}
