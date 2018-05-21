using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

[System.Serializable]
public struct DialogJsonInfo{
	public string dialogFolderPath;
	public string dialogFileName;
}

public class DialogScript : MonoBehaviour
{
    public Image charImgRight;
    public Image charImgLeft;

	public Text charNameLeft;
    public Text charNameRight;

    public Text dialogTextBoxRight;
	public Text dialogTextBoxLeft;

    public GameObject dialogBox;

	public DialogJsonInfo[] dialogJsonFiles;

    public float dialogDelay = 0.02f;
	public float automaticDialogDelay = 2f;

    public Sprite[] characterImages;

    static public bool pause = true;

  	private Cat currentCat;

	private bool isTyping;
	private bool cancelTyping;

	private JsonData lines;
	public List<DialogData> dialogLines = new List<DialogData>();
	private int currentDialogFileIndex = 0;
	private int currentLineNumber;

	private string currentJsonFile;
	private bool movingDialog;

    // Used for Initialization
    public void Start()
    {
    	currentCat = FindObjectOfType<Cat>();
		currentDialogFileIndex = 0;
    	currentLineNumber = 0;


        //scripts = GameObject.FindObjectsOfType<MonoBehaviour>();
        //StartCoroutine(testDelay());      // Use this to test the dialog box
    }

    IEnumerator testDelay()
    {
        yield return new WaitForSeconds(3);
        StartDialog(true);
    }

    public void StartDialog(bool movingDialog)
    {
		currentJsonFile = File.ReadAllText(Application.dataPath + dialogJsonFiles[currentDialogFileIndex].dialogFolderPath + dialogJsonFiles[currentDialogFileIndex].dialogFileName + ".json");
		dialogLines = JsonMapper.ToObject<List<DialogData>>(currentJsonFile);

		this.movingDialog = movingDialog;

		//Disable character controll and reset variables
		currentLineNumber = 0;

		if(!this.movingDialog){
			currentCat.DisableControls();
			pause = true;
		}

        this.enabled = true;

		SetupDialogBoxOrientation();

        //Setup dialog box before starting
        if(dialogLines[0].appearRight){
			charImgRight.sprite =  characterImages[dialogLines[0].imageIndex];
			charNameRight.text = dialogLines[0].characterName;
        }else{
			charImgLeft.sprite =  characterImages[dialogLines[0].imageIndex];
			charNameLeft.text = dialogLines[0].characterName;
        }
		
        //Start Dialog
        dialogBox.SetActive(true);
		StartCoroutine(WriteText(this.dialogLines[0].dialogText, dialogLines[0].appearRight));
    }

    public void StopDialog()
    {

    	if(!movingDialog){
			pause = false;
			StartCoroutine(EnableCharacterControls());
    	}

    	currentDialogFileIndex++;

		dialogBox.SetActive(false);

    }

	public void NextDialogLine(){


		if(!isTyping){

			if(currentLineNumber < dialogLines.Count - 1){
				currentLineNumber++;

				SetupDialogBoxOrientation();

				if(dialogLines[currentLineNumber].appearRight){
					charImgRight.sprite =  characterImages[dialogLines[currentLineNumber].imageIndex];
					charNameRight.text = dialogLines[currentLineNumber].characterName;
		        }else{
					charImgLeft.sprite =  characterImages[dialogLines[currentLineNumber].imageIndex];
					charNameLeft.text = dialogLines[currentLineNumber].characterName;
		        }	

				StartCoroutine(WriteText(this.dialogLines[currentLineNumber].dialogText, dialogLines[currentLineNumber].appearRight));

			}else{
				StopDialog();
			}
		}else{
			cancelTyping = true;
		}
	}

	IEnumerator WriteText(string currentLine, bool showRight){

		int letter = 0;

		dialogTextBoxLeft.text = "";
		dialogTextBoxRight.text = "";

		isTyping = true;
		cancelTyping = false;

		while(isTyping && !cancelTyping && (letter < currentLine.Length - 1)){

			if(showRight){
				dialogTextBoxRight.text += currentLine[letter];
			}else{
				dialogTextBoxLeft.text += currentLine[letter];
			}

			letter += 1;
			yield return new WaitForSeconds(dialogDelay);
		}

		if(showRight){
			dialogTextBoxRight.text = currentLine;
		}else{
			dialogTextBoxLeft.text = currentLine;
		}

		isTyping = false;
		cancelTyping = false;

		if(movingDialog){
			yield return new WaitForSeconds(automaticDialogDelay);
			NextDialogLine();
		}

	}

    
    // Use this to test the dialog changing
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !movingDialog)
        {
            NextDialogLine();
        }

    }

    IEnumerator EnableCharacterControls(){
    	yield return new WaitForSeconds(0.5f);
    	currentCat.EnableControls();
    }

    void SetupDialogBoxOrientation(){

		if(dialogLines[currentLineNumber].appearRight){

			charImgLeft.gameObject.SetActive(false);
			charImgRight.gameObject.SetActive(true);

			charNameLeft.gameObject.SetActive(false);
			charNameRight.gameObject.SetActive(true);

			dialogTextBoxLeft.gameObject.SetActive(false);
			dialogTextBoxRight.gameObject.SetActive(true);

		}else{

			charImgLeft.gameObject.SetActive(true);
			charImgRight.gameObject.SetActive(false);

			charNameLeft.gameObject.SetActive(true);
			charNameRight.gameObject.SetActive(false);

			dialogTextBoxLeft.gameObject.SetActive(true);
			dialogTextBoxRight.gameObject.SetActive(false);
		}

    }

    
}

[System.Serializable]
public class DialogData{

	public string dialogText {get; set;}
	public string characterName {get; set;}
	public int imageIndex {get; set;}
	public bool appearRight {get; set;}


	public DialogData(int imageIndex, string dialogText,string characterName, bool appearRight){

		this.imageIndex = imageIndex;
		this.dialogText = dialogText;
		this.characterName = characterName;
		this.appearRight = appearRight;
	}

	public DialogData(){
		this.imageIndex = 0;
		this.dialogText = "Default";
		this.characterName = "Default";
		this.appearRight = false;
	}

}
