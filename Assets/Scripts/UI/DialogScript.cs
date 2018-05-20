using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogScript : MonoBehaviour
{
    public string dialogText;
    public Image charImg;
    public Text charName;
    public Text dialogTextBox;
    public GameObject dialogBox;
    public float dialogDelay = 0.1f;
    private string currentText = "";
    public Sprite[] spriteTest = new Sprite[2];
    static public bool pause = true;
    private MonoBehaviour[] scripts;
    private Sprite character;
    private bool test = false;

    // Used for Initialization
    public void Start()
    {
        scripts = GameObject.FindObjectsOfType<MonoBehaviour>();
        StartCoroutine(testDelay());      // Use this to test the dialog box
    }

    IEnumerator testDelay()
    {
        yield return new WaitForSeconds(3);
        StartDialogPauseGame();
        DialogText("My name is Fiona!", "Fiona");
    }

    public void StartDialogPauseGame()
    {
        pause = true;
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        this.enabled = true;
        dialogBox.SetActive(true);
    }

    public void StopDialogResumeGame()
    {
        pause = false;
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        dialogBox.SetActive(false);
    }

    public void DialogText(string dialog, string characterName)
    {
        if(characterName == "Sebastian")
        {
            character = spriteTest[1];
        }
        else if (characterName == "Fiona")
        {
            character = spriteTest[0];
        }

        dialogText = dialog;
        charImg.sprite = character;
        charName.text = characterName;
        StartCoroutine(DisplayText());
    }

    // Displays the text
    IEnumerator DisplayText()
    {
        // Creates a Typewriting Effect
        for (int i = 0; i < dialogText.Length; i++)
        {
            currentText = dialogText.Substring(0, i);
            
            // Cancels the typewriting effect
            if(Input.anyKeyDown)
            {
                i = dialogText.Length;      // Makes sure the for statement doesnt continue to iterate.
                currentText = dialogText;   // Sets the current text to the full dialog
            }

            dialogTextBox.text = currentText;
            yield return new WaitForSeconds(dialogDelay);
        }

    }

    
    // Use this to test the dialog changing
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            test = true;
        }

        if (test)
        {
            test = false;
            StartCoroutine(TestDialog());       
        }
    }

    IEnumerator TestDialog()
    {
        yield return new WaitForSeconds(1);
        DialogText("My name is Sebastian!", "Sebastian");
    }
    
}
