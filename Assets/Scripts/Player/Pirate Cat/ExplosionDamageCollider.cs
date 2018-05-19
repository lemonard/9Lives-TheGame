using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageCollider : MonoBehaviour {

	public int damage;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Enemy"){
			Enemy enemy = other.GetComponent<Enemy>();

			if (enemy.canReceiveDamage)
            {
				if (!enemy.invulnerable)
                {
					enemy.life -= damage;
					enemy.receivedDamage = true;
                }

            }

		}
	}

	void Disappear(){
		Destroy(gameObject);
	}
}
