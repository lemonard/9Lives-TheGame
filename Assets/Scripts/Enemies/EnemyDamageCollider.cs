using UnityEngine;
using System.Collections;

public class EnemyDamageCollider : MonoBehaviour {

	public int damage;
    public Health healthScript;

    void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.tag == "Player"){

			Cat cat = other.gameObject.GetComponent<Cat>();
            healthScript = other.gameObject.GetComponent<Health>();

			if(!cat.invulnerable){
                healthScript.damage = true;
				cat.life -= damage;
				cat.receivedDamage = true;
			} 
		 

		}
	}
 

}
