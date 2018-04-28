using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBManager : MonoBehaviour {

	public CatStatue[] catStatues;


	// Use this for initialization
	void Start () {
		DefineActivatedStatues();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DefineActivatedStatues(){
		for(int i = 0; i < catStatues.Length; i++){
			switch(catStatues[i].type)
			{

				case(CatType.MagicCat):
					if(GameManager.instance.magicCatStageCleared){
						catStatues[i].Activate();
					}
					break;

				case(CatType.PussInBoots):
					if(GameManager.instance.pussInBootsStageCleared){
						catStatues[i].Activate();
					}
					break;
				case(CatType.ArcheologistCat):
					if(GameManager.instance.archeologistStageCleared){
						catStatues[i].Activate();
					}
					break;


			}

		}
	}
}
