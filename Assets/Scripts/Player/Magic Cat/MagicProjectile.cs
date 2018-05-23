using UnityEngine;
using System.Collections;

public class MagicProjectile : MonoBehaviour {

	[SerializeField]
	float speed;
	[SerializeField]
	public float timeToDestroy;

	private Cat player;
	public bool goRight;
	private float timeStampToDestroy;

	public GameObject magicShotParticlePrefab;
	public GameObject magicShotVanishParticlePrefab;

	public GameObject magicShotReflectedParticlePrefab;
	public GameObject magicShotReflectedVanishParticlePrefab;

	private bool reflected;

	// Use this for initialization
	void Start () {

		player = FindObjectOfType<Cat>();

		//Defines the direction that the fireball will go
		if(player.isLookingRight){
			goRight = true;
		} else {
			goRight = false;
			GetComponent<SpriteRenderer>().flipX = true;
		}

		timeStampToDestroy = Time.time + timeToDestroy;
	}
	
	// Update is called once per frame
	void Update () {

		if(goRight){
			GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = false;
        } else {
			GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
            GetComponent<SpriteRenderer>().flipX = true;
        }

		if(Time.time > timeStampToDestroy){
			if(reflected){
				Instantiate (magicShotReflectedVanishParticlePrefab, transform.position, Quaternion.identity);
			}else{
				Instantiate (magicShotVanishParticlePrefab, transform.position, Quaternion.identity);
			}
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.tag != "DeflectShield"){
			if(reflected){
				SpawnReflectedParticle();
			}else{
				SpawnParticle();
			}

		}

		if(reflected && other.tag == "Player"){
			Cat playerVariables = other.GetComponent<Cat>();

			if (!playerVariables.invulnerable) {
				playerVariables.life -= 1;
				playerVariables.receivedDamage = true;
            }
		}

        if (other.tag == "Scenario" || other.tag == "Ground") {
            Destroy(gameObject);
        } else if (other.tag == "Enemy") {
            Enemy enemyVariables = other.GetComponent<Enemy>();

            if (!enemyVariables.invulnerable) {
                enemyVariables.life -= 1;
                enemyVariables.receivedDamage = true;
            }

            Destroy(gameObject);
        } else if(other.tag == "DeflectShield") 
        {
			if (other.GetComponent<MagicShield> ().canReflect) {
				goRight = !goRight;
				reflected = true;
				GetComponent<Animator> ().SetBool ("reflected", true);
				timeStampToDestroy = Time.time + timeToDestroy;
			}
        }
        else {
            Destroy(gameObject);
        }

	}

	void SpawnParticle(){
		if (goRight) {
			Instantiate (magicShotParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,-90,0)));
		} else {
			Instantiate (magicShotParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,90,0)));
		}
	}

	void SpawnReflectedParticle(){
		if (goRight) {
			Instantiate (magicShotReflectedParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,-90,0)));
		} else {
			Instantiate (magicShotReflectedParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,90,0)));
		}
	}
}
