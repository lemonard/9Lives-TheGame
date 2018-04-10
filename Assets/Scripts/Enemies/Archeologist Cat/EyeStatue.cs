using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeStatue : MonoBehaviour {

	public float laserDistance;
	public float laserSpeed;
	public float laserCooldown = 3f;
	public float maximumLaserDuration = 3f;
	public float laserChargingTime = 1f;
	public GameObject laserChargingParticle;
	public GameObject laserCollideParticle;

	private Vector2 laserDirection;
	private Vector2 laserEndPoint;
	private LineRenderer laserLine;

	private GameObject currentChargingParticle;
	public float laserElapsedTime = 0;
	public float elapsedChargingTime = 0;

	public bool shooting;
	public bool charging;
	public bool waiting;

	// Use this for initialization
	void Start () {
		laserLine = GetComponent<LineRenderer>();
		laserLine.enabled = false;
		laserDirection = new Vector2(-1,-3);
	}
	
	// Update is called once per frame
	void Update () {
		if(!waiting){

			if(!charging && !shooting){
				StartLaserCharge();
			}

			if(charging && (elapsedChargingTime < laserChargingTime)){
				ChargeLaser();
			}else{
				StartLaserShooting();
			}

			if(shooting){
				ShootLaser();
				laserElapsedTime += Time.deltaTime;
				laserDirection += Vector2.left * laserSpeed * Time.deltaTime; 
			}

			if(shooting && laserElapsedTime > maximumLaserDuration){
				CancelLaser();
			}

		}

	}

	void StartLaserCharge(){
		currentChargingParticle = (GameObject)Instantiate(laserChargingParticle,transform.position,Quaternion.identity);
		charging = true;
	}

	void ChargeLaser(){
		elapsedChargingTime += Time.deltaTime;
	}

	void StartLaserShooting(){
		elapsedChargingTime = 0;
		charging = false;
		shooting = true;
		DestroyObject(currentChargingParticle);
		currentChargingParticle = null;
	}

	void ShootLaser(){
		
		Vector3 position = transform.position;
		Vector2 direction = laserDirection; 
		float distance = laserDistance;

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Player","Ground"));
		Debug.DrawRay(position, direction, Color.green);

		if(hit.collider != null){
			laserLine.enabled = true;
			laserLine.SetPosition(0,transform.position);	
			laserLine.SetPosition(1,hit.point);
			Instantiate(laserCollideParticle,hit.point,Quaternion.identity);	
		}
	}

	void CancelLaser(){
		laserElapsedTime = 0;
		shooting = false;
		laserLine.enabled = false;
		laserDirection = new Vector2(-1,-3);
		StartCoroutine(StartWaiting());
	}

	IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(laserCooldown);
    	waiting = false;

    }
}
