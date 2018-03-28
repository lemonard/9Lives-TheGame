using UnityEngine;
using System.Collections;

public class ParryCollider : MonoBehaviour {

	public PussInBoots player;

	public GameObject parryParticlesPrefab;

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "EnemyDamageCollider"){
			Enemy targetEnemy = other.gameObject.GetComponentInParent<Enemy> ();
			targetEnemy.prepareForParry = true;
			player.ParrySuccess(targetEnemy);

			Instantiate (parryParticlesPrefab, transform.position, Quaternion.identity);
		}
	}
}
