using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSearchingEnemy : Enemy {

	public float runningSpeed;

	public bool alert;
	public GameObject target;

	public virtual void TargetSpotted(GameObject target){

	}
}
