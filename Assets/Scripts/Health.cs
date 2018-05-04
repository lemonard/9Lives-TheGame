using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public Image healthBar;    // The main Health Object.
    public Text healthText; // The empty game object holding all the lives.
    public int numOfLives;            // The number of lives can be changed in the inspector.
    public static int healthMultiplier = 0;           // Used to increase the number of health based on the increase from the merchant.

    private int currentHealth;
    private int maxHealth;
    
	private Cat cat;

	// Use this for initialization
	void Start () 
    {
		cat = GetComponentInParent<Cat> ();

		maxHealth = cat.life;
		currentHealth = cat.life;

		healthBar.fillAmount = (float)currentHealth / maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (cat.life > maxHealth) {
			cat.life = maxHealth;
		}
		if (cat.life < 0) {
			cat.life = 0;
		}

		currentHealth = cat.life;
		healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

		healthBar.fillAmount = (float)currentHealth / maxHealth;

	}


	public void FillHealth()
	{
		cat.life = maxHealth;

	}
}
