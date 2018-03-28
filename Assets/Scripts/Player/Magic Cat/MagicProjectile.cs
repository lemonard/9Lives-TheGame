using UnityEngine;
using System.Collections;

public class MagicProjectile : MonoBehaviour {

	[SerializeField]
	float speed;
	[SerializeField]
	float timeToDestroy;

	private Cat player;
	public bool goRight;
	private float timeStampToDestroy;

	public GameObject magicShotParticlePrefab;

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
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (goRight) {
			Instantiate (magicShotParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,-90,0)));
		} else {
			Instantiate (magicShotParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,90,0)));
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
            goRight = !goRight;
        }
        else {
            Destroy(gameObject);
        }

	}
}
