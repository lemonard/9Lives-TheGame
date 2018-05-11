using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutlassCollider : MonoBehaviour {

	public int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
		
        if (other.gameObject.tag == "Enemy")
        {
        	
            Enemy enemyVariables = other.GetComponent<Enemy>();

            if (enemyVariables.canReceiveDamage)
            {
                if (!enemyVariables.invulnerable)
                {

                    enemyVariables.life -= damage;
                    enemyVariables.receivedDamage = true;

                }
            }

    	}
	}
}
