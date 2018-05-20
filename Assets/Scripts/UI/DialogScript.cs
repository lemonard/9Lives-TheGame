using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DialogData{
	public string dialogText;
	public string characterName;
	public int imageIndex;
}

public class DialogScript : MonoBehaviour
{
    public Image charImg;
    public Text charName;
    public Text dialogTextBox;
    public GameObject dialogBox;

    public float dialogDelay = 0.02f;

    public Sprite[] characterImages;

    static public bool pause = true;

  	private Cat currentCat;

	private bool isTyping;
	private bool cancelTyping;

	public List<DialogData> dialogLines;
	private int currentLineNumber;

    // Used for Initialization
    public void Start()
    {
    	currentCat = FindObjectOfType<Cat>();
    	currentLineNumber = 0;
        //scripts = GameObject.FindObjectsOfType<MonoBehaviour>();
        //StartCoroutine(testDelay());      // Use this to test the dialog box
    }

    IEnumerator testDelay()
    {
        yield return new WaitForSeconds(3);
        StartDialogPauseGame();
    }

    public void StartDialogPauseGame()
    {
        pause = true;

		//Disable character controll and reset variables
		currentLineNumber = 0;
		currentCat.DisableControls();
        this.enabled = true;

        //Setup dialog box before starting
        charImg.sprite =  characterImages[dialogLines[0].imageIndex];
        charName.text = dialogLines[0].characterName;

        //Start Dialog
        dialogBox.SetActive(true);
		StartCoroutine(WriteText(this.dialogLines[0].dialogText));
    }

    public void StopDialogResumeGame()
    {
        pause = false;

        dialogBox.SetActive(false);

		StartCoroutine(EnableCharacterControls());
    }


	public void NextDialogLine(){

		if(!isTyping){
			if(currentLineNumber < dialogLines.Count - 1){
				currentLineNumber++;

				charImg.sprite =  characterImages[dialogLines[currentLineNumber].imageIndex];
				charName.text = dialogLines[currentLineNumber].characterName;		

				StartCoroutine(WriteText(this.dialogLines[currentLineNumber].dialogText));

			}else{
				StopDialogResumeGame();
			}
		}else{
			cancelTyping = true;
		}
	}

	IEnumerator WriteText(string currentLine){

		int letter = 0;
		dialogTextBox.text = "";
		isTyping = true;
		cancelTyping = false;
		while(isTyping && !cancelTyping && (letter < currentLine.Length - 1)){
			dialogTextBox.text += currentLine[letter];
			letter += 1;
			yield return new WaitForSeconds(dialogDelay);
		}
		dialogTextBox.text = currentLine;
		isTyping = false;
		cancelTyping = false;
	}

    
    // Use this to test the dialog changing
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogLine();
        }

    }

    IEnumerator EnableCharacterControls(){
    	yield return new WaitForSeconds(0.5f);
    	currentCat.EnableControls();
    }

    
}
