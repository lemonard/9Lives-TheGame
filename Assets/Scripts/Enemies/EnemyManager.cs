using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Enemie{
		public GameObject enemieObject;
		public Vector3 startingPosition;
		public int enemieID;
		public bool isDead;
};

public class EnemyManager : MonoBehaviour {

	public static EnemyManager instance;

	public EnemySpawner[] enemieSpawners;

	// Use this for initialization
	void Start () {
		instance = this;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RespawnEnemies(){
		for(int i = 0; i < enemieSpawners.Length; i++){
			enemieSpawners[i].RespawnEnemies();
		}
	}
}
