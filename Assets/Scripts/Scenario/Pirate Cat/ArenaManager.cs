using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

	public static ArenaManager instance;
	public Arena currentActiveArena;
	public Collider2D cameraEdgeColliderRight;
	public Collider2D cameraEdgeColliderLeft;

	void Awake(){
		instance = this;
	}

	public void ResetArena(){


		if(currentActiveArena != null){
			CameraController.instance.follow = true;
			currentActiveArena.ResetArena();
			currentActiveArena = null;
		}
	}

	public void ActivateCameraColliders(){
		cameraEdgeColliderLeft.enabled = true;
		cameraEdgeColliderRight.enabled = true;
	}

	public void DeactivateCameraColliders(){
		cameraEdgeColliderLeft.enabled = false;
		cameraEdgeColliderRight.enabled = false;
	}
}
