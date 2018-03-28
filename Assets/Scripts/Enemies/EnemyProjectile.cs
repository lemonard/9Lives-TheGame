using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    [SerializeField]
    float speed;
    [SerializeField]
    float timeToDestroy;

    private WizardDog enemy;
    private bool goRight;
    private float timeStampToDestroy;

    // Use this for initialization
    void Start()
    {

        enemy = FindObjectOfType<WizardDog>();

        //Defines the direction that the fireball will go
        if (enemy.lookingRight)
        {
            goRight = true;
        }
        else
        {
            goRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }

        timeStampToDestroy = Time.time + timeToDestroy;
    }

    // Update is called once per frame
    void Update()
    {

        if (goRight)
        {
            GetComponent<Rigidbody2D>().transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            GetComponent<Rigidbody2D>().transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Time.time > timeStampToDestroy)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Scenario" || other.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            Cat playerVariables = other.GetComponent<Cat>();

            if (!playerVariables.invulnerable)
            {
                playerVariables.life -= 1;
                playerVariables.receivedDamage = true;
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
