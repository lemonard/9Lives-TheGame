using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FreakoutManager : MonoBehaviour {

	public List<GameObject> enemiesOnScreen;
	public SpriteRenderer mySpriteRenderer;
	public Animator myAnimator;
	public GameObject clawEffectPrefab;

	public Cat cat;

    public GameObject m_FBFiller;                                       // Placeholder for the filler in the level.
    public GameObject m_FBIcon;                                         // Placeholder for the icon in the level.
    public float m_percentage = 0.1f;                                   // The percentage of how much the bar will increase by.
    public float m_fillAmount = 0;                                          // 0 = empty, 1 = full.

    void Start(){
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myAnimator = GetComponent<Animator>();
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Cat>();
        m_FBFiller = GameObject.Find("FOB Filler");                     // Assigns the Filler to m_FBFiller.
        m_FBIcon = GameObject.Find("FOB Icon");                         // Assigns the Icon to m_FBicon.
        m_FBFiller.GetComponent<Image>().fillAmount = m_fillAmount;     // Grabs the current fill amount.
    }

    void Update()
    {
        UpdateFillAmount();
        if (m_fillAmount >= 1)
        {
            cat.ready = true;
        }
        else if(m_fillAmount != 1)
        {
            //cat.ready = false; //was being called too many times in the scene as errors after using FreakOut
        }
    }

    public void StartFreakout(Cat cat){
		myAnimator.SetBool("on",true);
		this.cat = cat;
		this.cat.GetComponent<SpriteRenderer>().enabled = false;
		this.cat.invulnerable = true;

        // Resets the fill amount.
        m_fillAmount = 0;
        UpdateFillAmount();
        cat.ready = false;
    }

    public void IncreaseFBBar()
    {
        m_fillAmount += m_percentage; // Increases the fill by a percentage.
        UpdateFillAmount();
    }

    public void UpdateFillAmount()
    {
        m_FBFiller.GetComponent<Image>().fillAmount = m_fillAmount;
    }

    public void StopFreakout(){
		myAnimator.SetBool("on",false);
		this.cat.freakoutMode = false;
		this.cat.invulnerable = false;
		this.cat.StopFreakout();
		this.cat.GetComponent<SpriteRenderer>().enabled = true;
	}

	public void KillAll(){

		foreach(GameObject enemie in enemiesOnScreen){
			if(enemie.GetComponentInChildren<Enemy>() != null){
				enemie.GetComponentInChildren<Enemy> ().life = 0;
			} else{
				enemie.GetComponent<Enemy> ().life = 0;
			}

		}
		enemiesOnScreen.Clear();
	}

	public void SpawnClawEffect(){
		foreach(GameObject enemie in enemiesOnScreen){
			GameObject effect = null;
			if(enemie.GetComponentInChildren<Rigidbody2D>().transform != null){
			 	effect = (GameObject)Instantiate(clawEffectPrefab, enemie.GetComponentInChildren<Rigidbody2D>().transform);
			} else{
				effect = (GameObject)Instantiate(clawEffectPrefab, enemie.transform); 
			}

			effect.transform.localPosition = Vector3.zero;
		}
	}

	public void AddEnemie(GameObject enemie){
		enemiesOnScreen.Add(enemie);
	}

	public void RemoveEnemie(GameObject enemie){
		for(int i = 0; i < enemiesOnScreen.Count; i++){
			if(enemiesOnScreen[i].Equals(enemie)){
				enemiesOnScreen.RemoveAt(i);
				break;
			}
		}
	}
}
