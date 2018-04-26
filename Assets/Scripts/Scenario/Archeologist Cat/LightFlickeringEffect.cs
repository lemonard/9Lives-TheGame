using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickeringEffect : MonoBehaviour {

	private SpriteRenderer mySpriteRenderer;
	private int frames = 0;
	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		frames++;
		if(frames == 4){
			mySpriteRenderer.color = new Color(mySpriteRenderer.color.r,mySpriteRenderer.color.g,mySpriteRenderer.color.b, Random.Range(0.6f,0.8f));
			float random = Random.Range(0.75f,1f);
			transform.localScale = new Vector3(random,random,transform.localScale.z);

			frames = 0;
		}

	}


}
