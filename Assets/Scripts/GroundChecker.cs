using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour {

	public Cat player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.gameObject.tag == "Ground" || other.gameObject.tag == "InvisiblePlatform" || other.gameObject.tag == "Enemy")
        {
			player.IsGrounded();
            player.isOnGround = true;
		}
	}
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "InvisiblePlatform" || other.gameObject.tag == "Enemy")
        {
            player.isOnGround = false;
        }
    }
}
