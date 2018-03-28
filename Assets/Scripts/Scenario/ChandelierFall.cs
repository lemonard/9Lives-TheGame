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


	// Use this for initialization
	void Start () {
        anchorsCut = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
	
	// Update is called once per frame
	void Update () {

        


        if (anchor1 == null && anchor2 == null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            anchorsCut++;

            finalDoor.SetActive(true);
            fallenChandelier.SetActive(true);
            Destroy(this.gameObject);

        }
	}
}
