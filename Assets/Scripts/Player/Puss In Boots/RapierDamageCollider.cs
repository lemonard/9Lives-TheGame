using UnityEngine;
using System.Collections;

public class RapierDamageCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
		
        if (other.gameObject.tag == "Enemy")
        {
        	
            Enemy enemyVariables = other.GetComponent<Enemy>();

            if (enemyVariables.canReceiveDamage)
            {
                if (!enemyVariables.invulnerable)
                {

                    enemyVariables.life -= 1;
                    enemyVariables.receivedDamage = true;

                }
            }
            else
            {
                 PussInBoots player = FindObjectOfType<PussInBoots>();

            }
        } else if (other.gameObject.tag == "Anchor"){

            Destroy(other.gameObject);

        } else if(other.gameObject.tag == "VulnerablePoint"){
        	other.GetComponent<VulnerablePoint>().TryToDoDamageToEnemy(1);

        }
    }
}
