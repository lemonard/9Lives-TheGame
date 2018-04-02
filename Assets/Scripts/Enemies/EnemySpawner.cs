using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemySpawner : MonoBehaviour {

	public GameObject enemiePrefab;

	public GameObject[] enemiesOnStage;
	public Enemie[] enemiesInfo;

	// Use this for initialization
	void Start () {

		enemiesInfo = new Enemie[enemiesOnStage.Length];
		for(int i = 0; i < enemiesOnStage.Length; i++){
			enemiesInfo[i].isDead = false;
			enemiesInfo[i].enemieID = enemiesOnStage[i].GetInstanceID();
			enemiesInfo[i].enemieObject = enemiesOnStage[i];
			enemiesInfo[i].startingPosition = enemiesOnStage[i].transform.position;
			if(enemiesInfo[i].enemieObject.GetComponentInChildren<Enemy>() != null){
				enemiesOnStage[i].gameObject.GetComponentInChildren<Enemy>().mySpawner = this;
			} else{
				enemiesOnStage[i].gameObject.GetComponent<Enemy>().mySpawner = this;
			}

		}
	}


	public void SetDeadEnemy(int id){
		for(int i = 0; i < enemiesInfo.Length; i++){
			if(enemiesInfo[i].enemieID == id){
				enemiesInfo[i].isDead = true;
				break;
			}
		}
	}

	public void RespawnEnemies(){

		for(int i = 0; i < enemiesInfo.Length; i++){
			if(enemiesInfo[i].isDead){
				GameObject enemie = (GameObject)Instantiate(enemiePrefab,enemiesInfo[i].startingPosition,Quaternion.identity);
				enemiesInfo[i].enemieObject = enemie;

				if(enemiesInfo[i].enemieObject.GetComponentInChildren<Enemy>() != null){
					enemiesInfo[i].enemieObject.GetComponentInChildren<Enemy>().mySpawner = this;
				} else{
					enemiesInfo[i].enemieObject.GetComponent<Enemy>().mySpawner = this;
				}

				enemiesInfo[i].enemieID = enemie.GetInstanceID();
				enemiesInfo[i].isDead = false;
			}else{
				enemiesInfo[i].enemieObject.transform.position = enemiesInfo[i].startingPosition;
			}
		}
	}
}
