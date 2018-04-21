using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerablePoint : MonoBehaviour {

	public bool isBackVulnerablePoint;

	private Cat player;
	private Enemy myEnemy;

	void Start(){
		if(isBackVulnerablePoint){
			player = FindObjectOfType<Cat>();
		}

		myEnemy = GetComponentInParent<Enemy>();
	}

	public void TryToDoDamageToEnemy(int damage){

		if(isBackVulnerablePoint){
			if((myEnemy.transform.position.x < player.transform.position.x) && !myEnemy.lookingRight){
				DoDamage(damage);
			}

			if((myEnemy.transform.position.x > player.transform.position.x) && myEnemy.lookingRight){
				DoDamage(damage);
			}

		}else{
			DoDamage(damage);
        }
	}

	void DoDamage(int damage){

		Enemy enemyVariables = GetComponentInParent<Enemy>();

		if (!myEnemy.invulnerable)
	    {

			myEnemy.life -= damage;
			myEnemy.receivedDamage = true;

	    }
	}
}
