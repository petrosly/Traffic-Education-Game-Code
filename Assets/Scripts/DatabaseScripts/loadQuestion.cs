using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using UnityEngine.SceneManagement;

public class loadQuestion : MonoBehaviour{

	public GameObject player; // for character movement
	private bool timeToMove;
	private int moveCounter =0;
	private int worstCategory;
	private int worstIndexStart, worstIndexEnd;

	public GameObject questionPanel; //for activation and deactivation of UI elements
	public GameObject instructionsPanel;
	public GameObject finishUI;
	public GameObject midawayUI;
	private Text questionText;	//text variables for loading the question on UI
	private Text answerOneTxt;
	private Text answerTwoTxt;
	private Text answerThreeTxt;
	private Text answerFourTxt;
	private Image image;

	private Button btnAnswerOne;	//default buttons for loading question
	private Button btnAnswerTwo;
	private Button btnAnswerThree;
	private Button btnAnswerFour;
	private Button rightAnswerBtn;
	private Button wrongAnswerBtn;

	private string connectionStr;	//variables for storing retrieved data from Db
	private string question;
	private string answerOne;
	private string answerTwo;
	private string answerThree;
	private string answerFour;
	private int rightAnswer;
	private string imagePath;
	private int questionId;
	private string questionCategory;

	private int[] loadedQuestionsArray;
	private int loadedCounter = 0;


	public int EvaluateUser() {
		float scoreCat1, scoreCat2, scoreCat3, scoreCat4;
		int numOfSignsQuestions, numOfPedestrianQuestions, numOfBicycleQuestions, numOfGeneralQuestions;

		IDbCommand dbcmd;
		IDbConnection dbcon = new SqliteConnection(connectionStr);
		dbcon.Open();
		dbcmd = dbcon.CreateCommand();
		string q_selectFromTable = "SELECT * FROM playerScores WHERE id=" + Login.loginID + ";";
		dbcmd.CommandText = q_selectFromTable;
		using (IDataReader reader = dbcmd.ExecuteReader()) {
			do {
				scoreCat1 = Convert.ToSingle(reader[1]);
				scoreCat2 = Convert.ToSingle(reader[2]);
				scoreCat3 = Convert.ToSingle(reader[3]);
				scoreCat4 = Convert.ToSingle(reader[4]);
				numOfSignsQuestions = Convert.ToInt32(reader[5]);
				numOfPedestrianQuestions = Convert.ToInt32(reader[6]);
				numOfBicycleQuestions = Convert.ToInt32(reader[7]);
				numOfGeneralQuestions = Convert.ToInt32(reader[8]);
			} while (reader.Read());
			reader.Close();
		}
		dbcon.Close();
		if(numOfSignsQuestions+ numOfPedestrianQuestions+ numOfBicycleQuestions+numOfGeneralQuestions >= 20) {
			float min = Math.Min(Math.Min(scoreCat1, scoreCat2), Math.Min(scoreCat3, scoreCat4));
			if(min == scoreCat1) {
				return 1;
            }else if(min == scoreCat2) {
				return 2;
            }else if(min == scoreCat3) {
				return 3;
            } else {
				return 4;
            }
        } else {
			return 0;
        }
	}
	public void UpdatePlayerScore(bool isRight){
		float scoreCat1, scoreCat2, scoreCat3, scoreCat4;
		int numOfSignsQuestions, numOfPedestrianQuestions, numOfBicycleQuestions, numOfGeneralQuestions;

		IDbCommand dbcmd;
		IDbConnection dbcon = new SqliteConnection(connectionStr);
		dbcon.Open();
		dbcmd = dbcon.CreateCommand();
		string q_selectFromTable = "SELECT * FROM playerScores WHERE id=" + Login.loginID + ";";
		dbcmd.CommandText = q_selectFromTable;
		using (IDataReader reader = dbcmd.ExecuteReader()) {
			do {
				scoreCat1 = Convert.ToSingle(reader[1]);
				scoreCat2 = Convert.ToSingle(reader[2]);
				scoreCat3 = Convert.ToSingle(reader[3]);
				scoreCat4 = Convert.ToSingle(reader[4]);
				numOfSignsQuestions = Convert.ToInt32(reader[5]);
				numOfPedestrianQuestions = Convert.ToInt32(reader[6]);
				numOfBicycleQuestions = Convert.ToInt32(reader[7]);
				numOfGeneralQuestions = Convert.ToInt32(reader[8]);
			} while (reader.Read());
			reader.Close();
		}
       
		if(isRight){
            switch (questionCategory) {
				case "signs":
					scoreCat1 *= numOfSignsQuestions;
					scoreCat1++;
					numOfSignsQuestions++;
					scoreCat1 /= numOfSignsQuestions;
					break;
				case "pedestrian":
					scoreCat2 *= numOfPedestrianQuestions;
					scoreCat2++;
					numOfPedestrianQuestions++;
					scoreCat2 /= numOfPedestrianQuestions;
					break;
				case "bicycle":
					scoreCat3 *= numOfBicycleQuestions;
					scoreCat3++;
					numOfBicycleQuestions++;
					scoreCat3 /= numOfBicycleQuestions;
					break;
				case "general":
					scoreCat4 *= numOfGeneralQuestions;
					scoreCat4++;
					numOfGeneralQuestions++;
					scoreCat4 /= numOfGeneralQuestions;
					break;
				default:
					break;
            }
		}else{
			switch (questionCategory) {
				case "signs":
					scoreCat1 *= numOfSignsQuestions;
					numOfSignsQuestions++;
					scoreCat1 /= numOfSignsQuestions;
					break;
				case "pedestrian":
					scoreCat2 *= numOfPedestrianQuestions;
					numOfPedestrianQuestions++;
					scoreCat2 /= numOfPedestrianQuestions;
					break;
				case "bicycle":
					scoreCat3 *= numOfBicycleQuestions;
					numOfBicycleQuestions++;
					scoreCat3 /= numOfBicycleQuestions;
					break;
				case "general":
					scoreCat4 *= numOfGeneralQuestions;
					numOfGeneralQuestions++;
					scoreCat4 /= numOfGeneralQuestions;
					break;
				default:
					break;
			}
		}
		string updatePlayerScores = "UPDATE playerScores SET scoreCatOne=" 
			+ scoreCat1+",scoreCatTwo="+scoreCat2 + ",scoreCatThree=" 
			+ scoreCat3+",scoreCatFour="+ scoreCat4 +",numOfSignsQuestions="
			+numOfSignsQuestions+",numOfPedestrianQuestions="+numOfPedestrianQuestions
			+",numOfBicycleQuestions="+numOfBicycleQuestions+",numOfGeneralQuestions="
			+numOfGeneralQuestions+ " WHERE id= " + Login.loginID + ";";
		dbcmd.CommandText = updatePlayerScores;
		using (IDataReader reader = dbcmd.ExecuteReader()) {
			reader.Close();
		}
		dbcon.Close();
	}

	public void generateQuestion(){
		//if loadedQuestions is full and position.z > 200, player reached end destination
		if (loadedCounter == 20 && player.transform.position.z >200){
			questionPanel.SetActive(false);
			finishUI.SetActive(true);
			return;
		}else if(loadedCounter == 20 && player.transform.position.z < 200) { //midway finish
			questionPanel.SetActive(false);
			midawayUI.SetActive(true);
			return;
		}
		questionPanel.SetActive(true);
		image.gameObject.SetActive(false);
		enableButtons();
		btnColorReset();
		int randomQuestionId = 0;
		if (worstCategory == 0) { //first time playing Level2
			System.Random rnd = new System.Random();
			randomQuestionId = rnd.Next(1, 101); //create random id to pick a q
			int pos = Array.IndexOf(this.loadedQuestionsArray, randomQuestionId);

			while (pos != -1) { //avoid asking 2 times the same question
				randomQuestionId = rnd.Next(1, 101);
				pos = Array.IndexOf(this.loadedQuestionsArray, randomQuestionId);
			}
        } else {    //analysis has been done now we have to propose questions in the right frequency
			System.Random rnd = new System.Random();
			int randomCategory = rnd.Next(1, 4);
			if(randomCategory == 1) {   // pick a question from worst category
				randomQuestionId = rnd.Next(worstIndexStart, worstIndexEnd + 1);
				int pos = Array.IndexOf(this.loadedQuestionsArray, randomQuestionId);

				while (pos != -1) { //avoid asking 2 times the same question
					randomQuestionId = rnd.Next(worstIndexStart, worstIndexEnd + 1);
					pos = Array.IndexOf(this.loadedQuestionsArray, randomQuestionId);
				}
            } else {
				randomQuestionId = rnd.Next(1, 101);	// pick a random question id
				int pos = Array.IndexOf(this.loadedQuestionsArray, randomQuestionId);
				
				//avoid asking twice the same question, while checking if id between index value of worst category 
				while (pos != -1 && !(randomQuestionId>=worstIndexStart && randomQuestionId<=worstIndexEnd) ) { 
					randomQuestionId = rnd.Next(worstIndexStart, worstIndexEnd + 1);                          
					pos = Array.IndexOf(this.loadedQuestionsArray, randomQuestionId);
				}
			}
        }
		
		IDbCommand dbcmd;
		IDataReader reader;
		IDbConnection dbcon = new SqliteConnection(connectionStr);
		dbcon.Open();
		dbcmd = dbcon.CreateCommand();
		string q_selectFromTable = "SELECT * FROM questions WHERE id=" + randomQuestionId + ";";

		dbcmd.CommandText = q_selectFromTable;
		reader = dbcmd.ExecuteReader();

		while(reader.Read()){
			questionId= Convert.ToInt32(reader[0]);
			question = reader[1].ToString();
			answerOne = reader[2].ToString();
			answerTwo = reader[3].ToString();
			answerThree = reader[4].ToString();;
			answerFour = reader[5].ToString();;
			rightAnswer = Convert.ToInt32(reader[6]);
			imagePath = reader[7].ToString();
			questionCategory = reader[8].ToString();
		}
		dbcon.Close();
		this.loadedQuestionsArray[loadedCounter] = questionId; 
		loadedCounter++;
		loadOnUI();
	}

	public void loadOnUI(){
		questionText.text = question;
		answerOneTxt.text = answerOne;
		answerTwoTxt.text = answerTwo;
		answerThreeTxt.text = answerThree;
		answerFourTxt.text = answerFour;
		if(imagePath != "") {
			image.gameObject.SetActive(true);
			Sprite sprite = Resources.Load<Sprite>(imagePath);
			image.sprite = sprite;
		}
	}	

	public void evaluateAnswers(){
		if(rightAnswer == 1){
			btnAnswerOne.GetComponent<Image>().color = Color.green;
			btnAnswerTwo.GetComponent<Image>().color = Color.red;
			btnAnswerThree.GetComponent<Image>().color = Color.red;
			btnAnswerFour.GetComponent<Image>().color = Color.red;
		}else if(rightAnswer == 2) {
			btnAnswerOne.GetComponent<Image>().color = Color.red;
			btnAnswerTwo.GetComponent<Image>().color = Color.green;
			btnAnswerThree.GetComponent<Image>().color = Color.red;
			btnAnswerFour.GetComponent<Image>().color = Color.red;
		}else if(rightAnswer == 3) {
			btnAnswerOne.GetComponent<Image>().color = Color.red;
			btnAnswerTwo.GetComponent<Image>().color = Color.red;
			btnAnswerThree.GetComponent<Image>().color = Color.green;
			btnAnswerFour.GetComponent<Image>().color = Color.red;
		}else{
			btnAnswerOne.GetComponent<Image>().color = Color.red;
			btnAnswerTwo.GetComponent<Image>().color = Color.red;
			btnAnswerThree.GetComponent<Image>().color = Color.red;
			btnAnswerFour.GetComponent<Image>().color = Color.green;
		}
	}

	private void btnColorReset(){
		btnAnswerOne.GetComponent<Image>().color = Color.white;
		btnAnswerTwo.GetComponent<Image>().color = Color.white;
		btnAnswerThree.GetComponent<Image>().color = Color.white;
		btnAnswerFour.GetComponent<Image>().color = Color.white;
	}

	public void movePlayer(){
		questionPanel.SetActive(false);
		rightAnswerBtn.gameObject.SetActive(false);
		timeToMove = true;
		moveCounter = 0;
	}

	public void newQuestion(){
		wrongAnswerBtn.gameObject.SetActive(false);
		generateQuestion();
	}

	public void startQuestions() {
		instructionsPanel.SetActive(false);
		generateQuestion();
    }

	public void evaluate_btnOne(){
		if(rightAnswer == 1){
			disableButtons();
			rightAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(true);
		}else{
			disableButtons();
			wrongAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(false);
		}
	}

	public void evaluate_btnTwo(){
		if(rightAnswer == 2){
			disableButtons();
			rightAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(true);
		}
		else{
			disableButtons();
			wrongAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(false);
		}
	}

	public void evaluate_btnThree(){
		if(rightAnswer == 3){
			disableButtons();
			rightAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(true);
		}
		else{
			disableButtons();
			wrongAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(false);
		}
	}

	public void evaluate_btnFour(){
		if(rightAnswer == 4){
			disableButtons();
			rightAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(true);
		}
		else{
			disableButtons();
			wrongAnswerBtn.gameObject.SetActive(true);
			UpdatePlayerScore(false);
		}
	}

	private void disableButtons(){
		btnAnswerOne.enabled = false;
		btnAnswerTwo.enabled = false;
		btnAnswerThree.enabled = false;
		btnAnswerFour.enabled = false;
	}

	private void enableButtons(){
		btnAnswerOne.enabled = true;
		btnAnswerTwo.enabled = true;
		btnAnswerThree.enabled = true;
		btnAnswerFour.enabled = true;
	}

	void Start(){
		connectionStr = "URI=file:" + Application.persistentDataPath + "/trafficDb.db";
		worstCategory = EvaluateUser();
        switch (worstCategory) {
			case 1:
				worstIndexStart = 1;
				worstIndexEnd = 25;
				break;
			case 2:
				worstIndexStart = 26;
				worstIndexEnd = 50;
				break;
			case 3:
				worstIndexStart = 51;
				worstIndexEnd = 75;
				break;
			case 4:
				worstIndexStart = 76;
				worstIndexEnd = 100;
				break;
			default:
				break;
        }
	}

	public void reloadLevel() {
		SceneManager.LoadScene("2ndLevelScene");
    }

	public void stopApplication() {
		Application.Quit();
    }

	void Awake(){
		//get panel components
		btnAnswerOne = GameObject.Find("1stAnswer").GetComponent<Button>();
		btnAnswerTwo = GameObject.Find("2ndAnswer").GetComponent<Button>();
		btnAnswerThree = GameObject.Find("3rdAnswer").GetComponent<Button>();
		btnAnswerFour = GameObject.Find("4thAnswer").GetComponent<Button>();

		answerOneTxt = GameObject.Find("1stAnswerText").GetComponent<Text>();
		answerTwoTxt = GameObject.Find("2ndAnswerText").GetComponent<Text>();
		answerThreeTxt = GameObject.Find("3rdAnswerText").GetComponent<Text>();
		answerFourTxt = GameObject.Find("4thAnswerText").GetComponent<Text>();
		image = GameObject.Find("Image").GetComponent<Image>();

		rightAnswerBtn = GameObject.Find("rightAnswerBtn").GetComponent<Button>();
		wrongAnswerBtn = GameObject.Find("wrongAnswerBtn").GetComponent<Button>();


		questionText = GameObject.Find("questionText").GetComponent<Text>();
		this.loadedQuestionsArray = new int[100]; // 100 = # of questions in db
		questionPanel.SetActive(false);
		rightAnswerBtn.gameObject.SetActive(false);
		wrongAnswerBtn.gameObject.SetActive(false);
		image.gameObject.SetActive(false);
	}


	void Update(){
		if (timeToMove && moveCounter<50){	//move handling
			player.transform.Translate(0.0f,0.0f,0.3f, Space.World);
			moveCounter++;
			if(moveCounter == 50){
				generateQuestion();
			}
		}else{
				timeToMove = false;
				moveCounter = 0;
		}
	}	
}