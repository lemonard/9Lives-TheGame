using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Sprite m_FullHeart;        // hearthealth.png
    [SerializeField] private Sprite m_EmptyHeart;       // tinyheartgone.png
    [SerializeField] private GameObject m_HealthObj;    // The main Health Object.
    [SerializeField] private GameObject m_HealthParent; // The empty game object holding all the lives.
    [SerializeField] private int numOfLives;            // The number of lives can be changed in the inspector.
    public static int m_healthMultiplier = 0;           // Used to increase the number of health based on the increase from the merchant.
    private int m_totalNumHealth;                       // Holds the maximum number of hearts for the character.
    private GameObject[] m_heart;                       // Placeholder for each heart.
    public bool damage = false;
    public bool heal = false;
    private int m_numHealth;
    private int m_maxHealth;
    private Cat cat;

	// Use this for initialization
	void Start () 
    {
        cat = gameObject.GetComponent<Cat>();
        if (this.gameObject.name == "Magic Cat")
        {
            m_maxHealth = numOfLives + m_healthMultiplier;
            cat.life = m_maxHealth;
            m_totalNumHealth = m_maxHealth;
            m_numHealth = m_totalNumHealth;
        }
        if(this.gameObject.name == "Puss In Boots")
        {
            m_maxHealth = numOfLives + m_healthMultiplier;
            cat.life = m_maxHealth;
            m_totalNumHealth = m_maxHealth;
            m_numHealth = m_totalNumHealth;
        }
        m_heart = new GameObject[m_totalNumHealth];
        for (int i = 0; i < m_totalNumHealth; i++)
        {
            m_heart[i] = Instantiate(                                               //              ^
                m_HealthObj,                                                        //              |
                m_HealthParent.transform.position + new Vector3(0.55f*i, 0, 0),     // Instantiates the Characters Health
                m_HealthParent.transform.rotation,                                  //              |
                m_HealthParent.transform);                                          //              v
            m_heart[i].name = "Health" + i;                                         // Renames each heart so that they arent all "Clones"
            m_heart[i].GetComponent<Image>().sprite = m_FullHeart;                  // Makes sure the player is at full hearts.
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_numHealth = cat.life;
        if(damage)
        {
            damage = false;
            if (m_numHealth <= 0)
            {
				m_numHealth = 0;
				for(int i = m_maxHealth - 1; i > m_numHealth - 1; i--)
				{
					m_heart[i].GetComponent<Image>().sprite = m_EmptyHeart;
				}
                // GAME OVER.
            }
            else
            {
				for(int i = m_maxHealth - 1; i > m_numHealth - 1; i--)
				{
					m_heart[i].GetComponent<Image>().sprite = m_EmptyHeart;
				}
                //m_heart[m_numHealth].GetComponent<Image>().sprite = m_EmptyHeart;
            }
        }

        if (heal)
        {
            cat.life++;
			if (cat.life > m_maxHealth) 
			{
				cat.life = m_maxHealth;
			}
            heal = false;
            if (m_numHealth > m_maxHealth)
            {
                m_numHealth = m_maxHealth;
            }
            else
            {
                m_heart[m_numHealth].GetComponent<Image>().sprite = m_FullHeart;
            }
        }
	}
}
