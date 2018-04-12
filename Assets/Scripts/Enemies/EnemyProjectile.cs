using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    [SerializeField]
    float speed;
    [SerializeField]
    float timeToDestroy;

    private WizardDog enemy;
    public bool goRight;
    private float timeStampToDestroy;

	public GameObject enemyShotParticlePrefab;
	public GameObject enemyShotVanishParticlePrefab;

    // Use this for initialization
    void Start()
    {

        //Defines the direction that the fireball will go

        timeStampToDestroy = Time.time + timeToDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        if (goRight)
        {
            GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
			GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
			GetComponent<SpriteRenderer>().flipX = true;
        }

        if (Time.time > timeStampToDestroy)
        {
			GetComponent<Animator> ().SetBool ("shotFade", true);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
		SpawnParticle();

        if (other.tag == "Scenario" || other.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            Cat playerVariables = other.GetComponent<Cat>();
			Health healthScript = other.gameObject.GetComponent<Health>();

            if (!playerVariables.invulnerable)
            {
				playerVariables.sourceOfDamagePosition = gameObject.transform.position;
                playerVariables.life -= 1;
				healthScript.damage = true;
                playerVariables.receivedDamage = true;
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

	void Disappear(){
		Instantiate (enemyShotVanishParticlePrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	void SpawnParticle(){
		if (goRight) {
			Instantiate (enemyShotParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,-90,0)));
		} else {
			Instantiate (enemyShotParticlePrefab, transform.position, Quaternion.Euler(new Vector3(0,90,0)));
		}
	}
}
