using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DroppedItem{
	public GameObject itemPrefab;
	public float dropRate;
}

public class Enemy : MonoBehaviour {

	public float life;
	public float speed;
	public bool receivedDamage;
	public bool invulnerable;
	public bool lookingRight;
	public EnemySpawner mySpawner;
	public AudioClip deathSound;
	public int damage;

	private int flashDelay = 2;
	protected SpriteRenderer mySpriteRenderer;
	public Animator myAnimator;
	private int flashingCounter;
	private bool toggleFlashing = false;
	protected float lifeWhileInvulnerable;

	public float invulnerableSeconds = 1;

	protected bool attacking;
	protected bool dying;

	protected float invulnerableTimeStamp;

	public bool prepareForParry;

	public bool stunned;
	public bool wasTurned;

	public bool canReceiveDamage;

	public DroppedItem[] droppedItemms;

	protected FreakoutManager freakoutManager;

	protected float lastPositionX;

	public Cat player;
	public bool playerInWalkingRange;
	public bool playerInAttackingRange;

	// Use this for initialization
	protected void EnemyInitialize () {
		player = FindObjectOfType<Cat>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        lookingRight = true;
        canReceiveDamage = true;
        freakoutManager = FindObjectOfType<FreakoutManager>();
		lastPositionX = transform.position.x;
	}

	public void Flash(){


		if(flashingCounter >= flashDelay){ 

			flashingCounter = 0;

			toggleFlashing = !toggleFlashing;

			if(toggleFlashing) {
				mySpriteRenderer.enabled = true;
			}
			else {
				mySpriteRenderer.enabled = false;
			}

		}
		else {
			flashingCounter++;
		}

	}

	public void ToggleInvinsibility(){
		receivedDamage = false;
		invulnerable = true;
		invulnerableTimeStamp = Time.time + invulnerableSeconds;

	}

	virtual public void ReceiveParry(){
		prepareForParry = false;
		stunned = true;
	}

	virtual public void DoDamage(){

	}

	public void PlayDeathSound(){
		if(deathSound){
			AudioManager.instance.PlayDeathSound(deathSound);
		}
	}

	virtual public void Disappear(){
		DropItem ();
		if (mySpawner != null) {
			mySpawner.SetDeadEnemy (this.gameObject.GetInstanceID ());
		}

		Destroy (gameObject);
	}

	public void DropItem()  //Dropping items from enemies
	{
		for (int i = 0; i < droppedItemms.Length; i++) {
			if (Random.value < droppedItemms[i].dropRate) {
				GameObject droppedItem = Instantiate (droppedItemms[i].itemPrefab, transform.position, Quaternion.identity);
				droppedItem.GetComponent<Item> ().MoveItem();
			}
		}

	}

	protected virtual void DefineDirectionToLook()
    {
        if (player.transform.position.x > transform.position.x)
        {
            lookingRight = true;
            mySpriteRenderer.flipX = false;
        }
        else
        {
            lookingRight = false;
            mySpriteRenderer.flipX = true;
        }
    }


}
