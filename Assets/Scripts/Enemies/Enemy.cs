using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float life;
	public float speed;
	public bool receivedDamage;
	public bool invulnerable;
	public bool lookingRight;

	private int flashDelay = 2;
	protected SpriteRenderer mySpriteRenderer;
	protected Animator myAnimator;
	private int flashingCounter;
	private bool toggleFlashing = false;
	protected float lifeWhileInvulnerable;

	public float invulnerableSeconds = 1;


	protected bool attacking;
	protected bool dying;

	protected float invulnerableTimeStamp;

	public bool prepareForParry;

	public bool stunned;

	public bool canReceiveDamage;

	public GameObject[] droppedItems;
	public float[] itemDropRate;

	protected FreakoutManager freakoutManager;

	protected float lastPositionX;

	// Use this for initialization
	void Start () {
		
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

	virtual public void Disappear(){
		DropItem ();
		Destroy (gameObject);
	}

	public void DropItem()  //Dropping items from enemies
	{
		for (int i = 0; i < droppedItems.Length; i++) {
			if (Random.value < itemDropRate [i]) {
				GameObject droppedItem = Instantiate (droppedItems [i], transform.position, Quaternion.identity);

				droppedItem.GetComponent<Item> ().MoveItem();
			}
		}

	}


}
