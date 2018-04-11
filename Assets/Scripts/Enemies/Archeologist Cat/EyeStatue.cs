using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeStatue : MonoBehaviour {

	[System.Serializable]
	public struct StatueType{
		public RuntimeAnimatorController animator;
		public Material material;
		public GameObject chargingParticle;
		public GameObject collideParticle;
	};

	public StatueType[] statueColors;

	public RuntimeAnimatorController currentAnimator;

	public Material currentLaserMaterial;


	public float laserDistance;
	public float laserSpeed;
	public float laserCooldown = 3f;
	public float maximumLaserDuration = 3f;
	public float laserChargingTime = 1f;
	public int laserDamage;
	public GameObject laserChargingParticle;
	public GameObject laserCollideParticle;

	private Vector2 laserDirection;
	private Vector2 laserEndPoint;
	private LineRenderer laserLine;
	private Animator myAnimator;

	private GameObject currentChargingParticle;
	public float laserElapsedTime = 0;
	public float elapsedChargingTime = 0;

	public bool lookingRight;
	public bool shooting;
	public bool charging;
	public bool waiting;
	public bool turning;


	public bool playerInRange;
	public bool turned;

	public enum StatueColor{
		Red,
		Cyan,
		Purple,
		Blue,

	}

	public StatueColor currentColor;

	// Use this for initialization
	void Start () {

		laserLine = GetComponent<LineRenderer>();
		laserLine.enabled = false;
		laserDirection = new Vector2(-1,-3);

		myAnimator = GetComponent<Animator>();

		DefineColor();

	}
	
	// Update is called once per frame
	void Update () {

		if(!waiting){

			if(!charging && !shooting && playerInRange){
				StartLaserCharge();
			}

			if(charging && (elapsedChargingTime < laserChargingTime)){
				ChargeLaser();
			}

			if(elapsedChargingTime > laserChargingTime){
				StartLaserShooting();
			}

			if(shooting){
				ShootLaser();
				laserElapsedTime += Time.deltaTime;
				if(turned){
					laserDirection += Vector2.right * laserSpeed * Time.deltaTime;
				}else{
					laserDirection += Vector2.left * laserSpeed * Time.deltaTime; 
				}
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

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Player","Ground","BreakableScenario"));
		Debug.DrawRay(position, direction, Color.green);

		if(hit.collider != null){
			laserLine.enabled = true;
			laserLine.SetPosition(0,transform.position);	
			laserLine.SetPosition(1,hit.point);
			Instantiate(laserCollideParticle,hit.point,Quaternion.identity);

			if(hit.collider.GetComponent<Cat>()){
				Cat cat = hit.collider.gameObject.GetComponent<Cat>();
				Health healthScript = hit.collider.gameObject.GetComponent<Health>();

				if(!cat.invulnerable){
	                healthScript.damage = true;
					cat.life -= laserDamage;
					cat.receivedDamage = true;
				} 
			}else if(hit.collider.gameObject.GetComponent<LaserBreakableWall>()){
				Destroy(hit.collider.gameObject);
			}
				
		}
	}

	void CancelLaser(){
		laserElapsedTime = 0;
		shooting = false;
		laserLine.enabled = false;
		if(turned){
			laserDirection = new Vector2(1,-3);
		}else{
			laserDirection = new Vector2(-1,-3);
		}
		StartCoroutine(StartWaiting());
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.GetComponent<WhipCollider>() && !turning){
			WhipCollider whip = other.GetComponent<WhipCollider>();
			if(whip.charged){
				StartTurn();
			}

			//whip.GetComponent<BoxCollider2D>().enabled = false;		
		}

	}

	void StartTurn(){
		turning = true;
//		elapsedChargingTime = 0;
//		charging = false;
//		DestroyObject(currentChargingParticle);
//		currentChargingParticle = null;
//		CancelLaser();
		GetComponent<Animator>().SetBool("turning",true);
	}

	void StopTurning(){
		turning = false;
		turned = !turned;
		if(turned){
			transform.rotation = Quaternion.Euler(new Vector3(0,900,0));
			if(!shooting){
				laserDirection = new Vector2(1,-3);
			}
		}else{
			transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
			if(!shooting){
				laserDirection = new Vector2(-1,-3);
			}
		}
		GetComponent<Animator>().SetBool("turning",false);
	}

	void DefineColor(){

		switch(currentColor){

			case(StatueColor.Red):
					currentAnimator = statueColors[0].animator;
					currentLaserMaterial = statueColors[0].material;
					laserChargingParticle = statueColors[0].chargingParticle;
					laserCollideParticle = statueColors[0].collideParticle;
				break;

			case(StatueColor.Cyan):
					currentAnimator = statueColors[1].animator;
					currentLaserMaterial = statueColors[1].material;
					laserChargingParticle = statueColors[1].chargingParticle;
					laserCollideParticle = statueColors[1].collideParticle;
				break;

			case(StatueColor.Purple):
					currentAnimator = statueColors[2].animator;
					currentLaserMaterial = statueColors[2].material;
					laserChargingParticle = statueColors[2].chargingParticle;
					laserCollideParticle = statueColors[2].collideParticle;
				break;

			case(StatueColor.Blue):
					currentAnimator = statueColors[3].animator;
					currentLaserMaterial = statueColors[3].material;
					laserChargingParticle = statueColors[3].chargingParticle;
					laserCollideParticle = statueColors[3].collideParticle;
				break;

		}

		myAnimator.runtimeAnimatorController = currentAnimator;
		laserLine.material = currentLaserMaterial;
	}

	public void SwitchColors(){

		switch(currentColor){

			case(StatueColor.Red):
				currentColor = StatueColor.Cyan;
				DefineColor();
			break;

			case(StatueColor.Cyan):
				currentColor = StatueColor.Purple;
				DefineColor();
			break;

			case(StatueColor.Purple):
				currentColor = StatueColor.Blue;
				DefineColor();
			break;

			case(StatueColor.Blue):
				currentColor = StatueColor.Red;
				DefineColor();
			break;
			
		}
	}


	IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(laserCooldown);
    	waiting = false;

    }
}
