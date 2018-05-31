using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchingCannon : MonoBehaviour {

	public Transform projectile;
	public Transform targetPoint;

	public float firingAngle = 45.0f;
	public float gravityScale = 2f;

	public float elapsedTime;
	public float flightDuration;

	public bool canActivate;
	public PirateCat targetCat;

	public Vector3 targetCatVelocity;

	public bool launchRight;

	public bool catInCannon;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(canActivate){
			if(targetCat != null){

				if(!targetCat.isDying && !targetCat.freakoutMode && !targetCat.beingLaunched && !targetCat.controlersDisabled && !targetCat.isJumping && !targetCat.isAttacking && !targetCat.isShooting){
					
					if((Input.GetKeyDown (targetCat.attackKey) || Input.GetButtonDown(targetCat.attackGamepadButton))){
						//LaunchCat();
						if(!catInCannon){
							ActivateCannon();
						}else{
							ShootCannon();
						}

					}
				}
			}
		}
	}

	void ActivateCannon(){
		
		targetCat.PrepareToBeLaunched(launchRight);
		targetCat.inCannon = true;
		GetComponent<Animator>().SetBool("activated", true);

		catInCannon = true;
	}


	void ShootCannon(){
		GetComponent<Animator>().SetBool("activated", false);
		GetComponent<Animator>().SetBool("shoot", true);

		CameraController.instance.ChangeToTargetAndCenterNoDamp(targetCat.transform.gameObject);
		CameraController.instance.SetSpeedX(1);
		StartCoroutine(Launch());
	}

	void FinishedShotAnimation(){
		targetCat.inCannon = false;
		catInCannon = false;
		GetComponent<Animator>().SetBool("shoot", false);
	}

	IEnumerator Launch(){

		Vector3 cameraTopBorderPosition = CameraController.instance.topBorder.transform.localPosition;

		CameraController.instance.topBorder.transform.localPosition = new Vector3(CameraController.instance.topBorder.transform.localPosition.x,CameraController.instance.topBorder.transform.localPosition.y + 5000,CameraController.instance.topBorder.transform.localPosition.z);

		projectile = targetCat.transform.parent;

		projectile.position = transform.position + Vector3.zero;

		float gravity = gravityScale * 9.8f;

		float targetDistance = Vector3.Distance(projectile.position, targetPoint.position);

		float projectileVelocity = targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

		float velocityX = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		float velocityY = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

		// Calculate flight time.
		flightDuration = targetDistance / velocityX;
   
        // Rotate projectile to face the target.
       
		elapsedTime = 0;

		targetCat.LaunchAnimation();
		targetCat.GetComponentInParent<Rigidbody2D>().gravityScale = 0;
 
		while (elapsedTime < flightDuration)
        {

			projectile.Translate(velocityX * Time.deltaTime, (velocityY - (gravity * elapsedTime)) * Time.deltaTime, 0);
           
			elapsedTime += Time.deltaTime;
			targetCat.beingLaunched = true;
            yield return null;
        }

		float endYVelocity = (velocityY - (gravity * elapsedTime)); 

		targetCat.GetComponentInParent<Rigidbody2D>().gravityScale = gravityScale;
		targetCat.GetComponentInParent<Rigidbody2D>().velocity = new Vector3(velocityX,endYVelocity,0);
		targetCatVelocity = targetCat.GetComponentInParent<Rigidbody2D>().velocity;

		CameraController.instance.topBorder.transform.localPosition = cameraTopBorderPosition;


        targetCat = null;

	}

}
