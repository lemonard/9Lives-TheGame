using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public bool magicCatStageCleared;
	public bool pussInBootsStageCleared;
	public bool archeologistStageCleared;

	public Cat player;
	public int playerCurrency = 0;
	public bool changingScene;

	private bool foundCat;

	public List<GameObject> enemiesNearCat = new List<GameObject>();
	public int maxAmountOfEnemiesNearCat = 2;

	void Awake(){
		if(GameManager.instance == null){
			instance = this;
			DontDestroyOnLoad(this.gameObject);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}else{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void AddToEnemyNearCatList(GameObject enemy){

		if(enemiesNearCat.Count < maxAmountOfEnemiesNearCat){
			enemiesNearCat.Add(enemy);
		}

	}

	public void RemoveFromEnemyNearCatList(GameObject enemy){

		enemiesNearCat.Remove(enemy);


	}

//	public void StartStage(){
//		changingScene = false;
//		SetPlayerCurrency();
//	}

	public void EndStage(){
		foundCat = false;
		UpdatePlayerCurrency();
	}

	public void UpdateCat(Cat cat){
		player = cat;
	}

	void SetPlayerCurrency(){
		player.currencyAmount = playerCurrency;
	}

	void UpdatePlayerCurrency(){
		playerCurrency = player.currencyAmount;
	}

	void TryFindingCat(){
		Cat cat = FindObjectOfType<Cat>();
		if(cat != null){
			foundCat = true;
			player = cat;
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){

		TryFindingCat();

		if(player != null){
			SetPlayerCurrency();
		}

	}
}


public enum CatType{
		MagicCat,
		PussInBoots,
		PirateCat,
		ArcheologistCat,
		NinjaCat,
		RobotCat,
		GodCat,
		DemonCat
	}