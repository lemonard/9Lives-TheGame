using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierFall : MonoBehaviour {

    public int anchorsCut;

    public GameObject chandelier;
    public GameObject anchor1;
    public GameObject anchor2;
    public GameObject finalDoor;
    public GameObject fallenChandelier;

    private Rigidbody2D rb;
    private bool lastAnchorWasRight;

	// Use this for initialization
	void Start () {
        anchorsCut = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
	
	// Update is called once per frame
	void Update () {
		if(anchor1 == null && anchor2 != null){
			lastAnchorWasRight = true;
		}else if(anchor1 != null && anchor2 == null){
			lastAnchorWasRight = false;
		}

        if (anchor1 == null && anchor2 == null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            anchorsCut++;
            if(lastAnchorWasRight){
            	ChandelierCutsceneManager.instance.StartTimeline(true, gameObject);
            }else{
				ChandelierCutsceneManager.instance.StartTimeline(false, gameObject);
            }

            finalDoor.SetActive(true);
            fallenChandelier.SetActive(true);


        }
	}

}
