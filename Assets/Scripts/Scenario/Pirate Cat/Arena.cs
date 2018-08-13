using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ArenaEnemyData{
	public GameObject enemyPrefab;
	public bool randomSpawnPoint;
	public bool spawnRight;
}

public class Arena : MonoBehaviour {

	public ArenaEnemyData[] enemyList;
	public int numberOfInitialEnemies;
	public float timeBetweenEnemies = 5f;

	public int maxEnemiesActive = 5;
	public int currentEnemiesActive = 0;

	private int enemyIndex = 0;
	private int currentEnemiesDead = 0;

	public Transform rightSpawnPoint;
	public Transform leftSpawnPoint;

	public Transform rightEntranceSpot;
	public Transform leftEntranceSpot;

	public bool active;

	private Coroutine spawnCoroutine;
	private bool canSpawn;

	public Transform[] laneReferences;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(currentEnemiesActive < maxEnemiesActive && canSpawn){
			if(enemyIndex < enemyList.Length && spawnCoroutine == null){
				spawnCoroutine = StartCoroutine(SpawnCooldown());
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player" && !active){
			GetComponent<Collider2D>().enabled = false;
			StartCoroutine(ActivateArena());
		}
	}

	IEnumerator ActivateArena(){
		active = true;
		CameraController.instance.follow = false;
		ArenaManager.instance.currentActiveArena = this;

		for(int i = 0; i < numberOfInitialEnemies; i++){
			SpawnEnemy(i);
			yield return new WaitForSeconds(1f);
			enemyIndex++;
		}

		canSpawn = true;

	}

	void SpawnEnemy(int index){

		GameObject enemy;
		currentEnemiesActive++;

		if(enemyList[index].randomSpawnPoint){

			float random = Random.value;

			if(random < 0.5f){
				enemy = (GameObject)Instantiate(enemyList[index].enemyPrefab,rightSpawnPoint.position,Quaternion.identity);
				enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);

				enemy.GetComponentInChildren<CombableEnemy>().arenaEnemy = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrance = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrancePoint = rightEntranceSpot;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntranceSpotToTheRight = false;
				//enemy.GetComponentInChildren<BeatEmUpZPositionCalculator>().laneReferences = laneReferences;
			}else{
				enemy = (GameObject)Instantiate(enemyList[index].enemyPrefab,leftSpawnPoint.position,Quaternion.identity);
				enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
				enemy.GetComponentInChildren<CombableEnemy>().arenaEnemy = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrance = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrancePoint = leftEntranceSpot;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntranceSpotToTheRight = true;
				//enemy.GetComponentInChildren<BeatEmUpZPositionCalculator>().laneReferences = laneReferences;
			}

		}else{

			if(enemyList[index].spawnRight){
				enemy = (GameObject)Instantiate(enemyList[index].enemyPrefab,rightSpawnPoint.position,Quaternion.identity);
				enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
				enemy.GetComponentInChildren<CombableEnemy>().arenaEnemy = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrance = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrancePoint = rightEntranceSpot;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntranceSpotToTheRight = false;
				//enemy.GetComponentInChildren<BeatEmUpZPositionCalculator>().laneReferences = laneReferences;
			}else{
				enemy = (GameObject)Instantiate(enemyList[index].enemyPrefab,leftSpawnPoint.position,Quaternion.identity);
				enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
				enemy.GetComponentInChildren<CombableEnemy>().arenaEnemy = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrance = true;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntrancePoint = leftEntranceSpot;
				enemy.GetComponentInChildren<CombableEnemy>().arenaEntranceSpotToTheRight = true;
				//enemy.GetComponentInChildren<BeatEmUpZPositionCalculator>().laneReferences = laneReferences;
			}
		}

	}

	public void IncreaseAmountOfEnemiesDead(){
		currentEnemiesDead++;
		currentEnemiesActive--;

		if(currentEnemiesDead >= enemyList.Length){
			FinishArena();
		}
	}

	void FinishArena(){

		CameraController.instance.follow = true;
		ArenaManager.instance.currentActiveArena = null;

		Destroy(gameObject);
	}

	public void ResetArena(){
		active = false;
		enemyIndex = 0;
		currentEnemiesDead = 0;
		currentEnemiesActive = 0;
		canSpawn = false;
		if(spawnCoroutine != null){
			StopCoroutine(spawnCoroutine);
		}

	}

	IEnumerator SpawnCooldown(){
		
		yield return new WaitForSeconds(timeBetweenEnemies);
		if(active){
			SpawnEnemy(enemyIndex);
			enemyIndex++;
		}

		spawnCoroutine = null;
	}
}
