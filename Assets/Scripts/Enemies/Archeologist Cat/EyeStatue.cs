using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatueColor{
		Red,
		Cyan,
		Purple,
		Blue,
		Green,
		Yellow,
		White
	}

public class EyeStatue : MonoBehaviour {

	[System.Serializable]
	public struct StatueType{
		public RuntimeAnimatorController animator;
		public Material material;
		public Sprite sprite;
		public Sprite disabledSprite;
		public Color statueColor;
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
	public float laserAccelerationFactor = 0.05f;
	private float initialSpeed;
	public GameObject laserChargingParticle;
	public GameObject laserCollideParticle;
	public GameObject laserFiringPoint;

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
	public bool disabled;

	public bool playerInRange;
	public bool turned;

	public StatueColor currentColor;
	public Color currentStatueColor;
	private Sprite currentSprite;
	private Sprite currentDisabledSprite;
	private SpriteRenderer mySpriteRenderer;
	// Use this for initialization
	void Start () {

		mySpriteRenderer = GetComponent<SpriteRenderer>();
		laserLine = GetComponent<LineRenderer>();
		laserLine.enabled = false;
		laserDirection = new Vector2(-1,-3);
		initialSpeed = laserSpeed;
		myAnimator = GetComponent<Animator>();

		DefineColor();

	}
	
	// Update is called once per frame
	void Update () {   //Verifying statue's shooting cycle

		if(!disabled){
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
					laserSpeed += laserAccelerationFactor;
				}

				if(shooting && laserElapsedTime > maximumLaserDuration){
					CancelLaser();
				}


			}
		}

	}

	void StartLaserCharge(){
		currentChargingParticle = (GameObject)Instantiate(laserChargingParticle,laserFiringPoint.transform.position,Quaternion.identity,gameObject.transform);
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
		
		Vector3 position = laserFiringPoint.transform.position;
		Vector2 direction = laserDirection; 
		float distance = laserDistance;

		RaycastHit2D hit = Physics2D.Raycast(position,direction,distance,LayerMask.GetMask("Player","Ground","LaserInteractableScenario"));
		Debug.DrawRay(position, direction, Color.green);

		if(hit.collider != null){
			
			laserLine.enabled = true;
			laserLine.SetPosition(0,laserFiringPoint.transform.position);
			laserLine.SetPosition(1,hit.point);
			Instantiate(laserCollideParticle,hit.point,Quaternion.identity);

			if (hit.collider.GetComponent<Cat> ()) {
				Cat cat = hit.collider.gameObject.GetComponent<Cat> ();			//When laser hits the cat, calculate damage

				if (!cat.invulnerable) {    //Check if the player can be hit
					
					cat.sourceOfDamagePosition = new Vector3 (hit.point.x, hit.point.y, 0);
					cat.life -= laserDamage;
					cat.receivedDamage = true;
				} 
			} else if (hit.collider.gameObject.GetComponent<LaserBreakableWall> ()) {   //When laser hits a breakable wall with the same color, destroy the target

				BreakWall(hit);

			} else if (hit.collider.gameObject.GetComponent<EyeStatue> ()) {    //When hitting another statue, change the target's color to this statue's color
				EyeStatue statue = hit.collider.gameObject.GetComponent<EyeStatue> ();
				statue.SwitchColorTo (currentColor);
			} else if (hit.collider.gameObject.GetComponent<ColorChangingJewelSwitch> ()) {    //When hitting a jewel, change the target's color to this statue's color
				ColorChangingJewelSwitch jewel = hit.collider.gameObject.GetComponent<ColorChangingJewelSwitch>();
				jewel.ChangeColorTo	(currentColor);
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
		laserSpeed = initialSpeed;
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
		GetComponent<Animator>().SetBool("turning",false);
		if(turned){
			mySpriteRenderer.flipX = true;
			if(!shooting){
				laserDirection = new Vector2(1,-3);
			}
		}else{
			mySpriteRenderer.flipX = false;
			if(!shooting){
				laserDirection = new Vector2(-1,-3);
			}
		}

	}

	void DefineColor(){

		switch(currentColor){

			case(StatueColor.Red):
					currentAnimator = statueColors[0].animator;
					currentLaserMaterial = statueColors[0].material;
					laserChargingParticle = statueColors[0].chargingParticle;
					laserCollideParticle = statueColors[0].collideParticle;
					currentSprite = statueColors[0].sprite;
					currentDisabledSprite = statueColors[0].disabledSprite;
					currentStatueColor = statueColors[0].statueColor;
				break;

			case(StatueColor.Green):
					currentAnimator = statueColors[1].animator;
					currentLaserMaterial = statueColors[1].material;
					laserChargingParticle = statueColors[1].chargingParticle;
					laserCollideParticle = statueColors[1].collideParticle;
					currentSprite = statueColors[1].sprite;
					currentDisabledSprite = statueColors[0].disabledSprite;
					currentStatueColor = statueColors[1].statueColor;
				break;

			case(StatueColor.Blue):
					currentAnimator = statueColors[2].animator;
					currentLaserMaterial = statueColors[2].material;
					laserChargingParticle = statueColors[2].chargingParticle;
					laserCollideParticle = statueColors[2].collideParticle;
					currentSprite = statueColors[2].sprite;
					currentDisabledSprite = statueColors[0].disabledSprite;
					currentStatueColor = statueColors[2].statueColor;
				break;

		}

		myAnimator.runtimeAnimatorController = currentAnimator;
		laserLine.material = currentLaserMaterial;

		if(disabled){
			mySpriteRenderer.sprite = currentDisabledSprite;
		}else{
			mySpriteRenderer.sprite = currentSprite;
		}
	}


	public void SwitchColorsForward(){

		switch(currentColor){

			case(StatueColor.Red):
				currentColor = StatueColor.Green;
				DefineColor();
			break;

			case(StatueColor.Green):
				currentColor = StatueColor.Blue;
				DefineColor();
			break;


			case(StatueColor.Blue):
				currentColor = StatueColor.Red;
				DefineColor();
			break;
			
		}
	}

	public void SwitchColorsBackwards(){

		switch(currentColor){

			case(StatueColor.Red):
				currentColor = StatueColor.Blue;
				DefineColor();
			break;

			case(StatueColor.Green):
				currentColor = StatueColor.Red;
				DefineColor();
			break;


			case(StatueColor.Blue):
				currentColor = StatueColor.Green;
				DefineColor();
			break;
			
		}
	}

	public void SwitchColorTo(StatueColor color){ 
		currentColor = color;
		DefineColor();
	}

	public void Enable(){
		disabled = false;
		mySpriteRenderer.sprite = currentSprite;
	}

	public void Disable(){
		disabled = true;
		mySpriteRenderer.sprite = currentDisabledSprite;
	}

	void BreakWall(RaycastHit2D hit){

		LaserBreakableWall breakableWall = hit.collider.gameObject.GetComponent<LaserBreakableWall> ();

		if (breakableWall.currentColor == currentColor) {

			if(breakableWall.transform.position.x >= hit.point.x){
				breakableWall.explodeRight = true;
			}else{
				breakableWall.explodeRight = false;
			}

			breakableWall.Break();
		}

	}

	IEnumerator StartWaiting(){

    	waiting = true;
    	yield return new WaitForSeconds(laserCooldown);
    	waiting = false;

    }
}
