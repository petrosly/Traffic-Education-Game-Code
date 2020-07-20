using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class Login : MonoBehaviour {

	public static int loginID;
	public InputField usernameField;
    public InputField passwordField;
    public Button submitButton;
    private string connectionStr;
    public GameObject noUser;
    public GameObject passwordFailed;

    //make submit button interactable
    public void buttonInteract(){
    	submitButton.interactable = (usernameField.text.Length >= 4 && passwordField.text.Length >=5);
    }

    //check if username exists
    public bool usernameExists(string username){
    	noUser.SetActive(false);
		IDbCommand dbcmd;
		IDataReader reader;
		IDbConnection dbcon = new SqliteConnection(connectionStr);
		dbcon.Open();
		dbcmd = dbcon.CreateCommand();
		string q_selectFromTable = "SELECT username FROM players WHERE username='" + username + "';";
		dbcmd.CommandText = q_selectFromTable;
		reader = dbcmd.ExecuteReader();

		if(reader.Read() == true){ //username exists in DB
			dbcon.Close();
			return true;
		}
		dbcon.Close();
		return false;
    }

    public void getLoginInfo(){		// get the info that the user provides in order to log in
    	string hash;
    	string saltString;
    	int score;
		string username = usernameField.text;
		string password = passwordField.text;

		if(usernameExists(username)){	//retrieve all of his info
			IDbCommand dbcmd;
			IDataReader reader;
			IDbConnection dbcon = new SqliteConnection(connectionStr);
		    dbcon.Open(); 
			dbcmd = dbcon.CreateCommand();
			string q_insertToPlayers = "SELECT * FROM players WHERE username='" + username + "';";
			dbcmd.CommandText = q_insertToPlayers;
			reader = dbcmd.ExecuteReader();
			loginID = Convert.ToInt32(reader[0]);
			hash=reader[2].ToString();	//retrieve hash and salt from Db
			saltString = reader[3].ToString();
			score = Convert.ToInt32(reader[4].ToString());
			dbcon.Close();
			byte[] saltBytesArray = Convert.FromBase64String(saltString);			
			var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytesArray, 10000);
			byte[] hashBytes= pbkdf2.GetBytes(20);
			string loginHash = Convert.ToBase64String(hashBytes);
			if(loginHash != hash){
				passwordFailed.SetActive(true);
			}else{
				if(score < 10){
					SceneManager.LoadScene("1stLevelScene");
				}else{
					SceneManager.LoadScene("2ndLevelScene");
				}
			}
		}else{
			noUser.SetActive(true); //UI for non-existing user input
		}
    }

    public void goToMenu(){
    	SceneManager.LoadScene("MainMenu");
    }

    void Start(){
		connectionStr = "URI=file:" + Application.persistentDataPath + "/trafficDb.db";
	}

    void Update(){
    	buttonInteract();  // check if input conditions is met every frame and then make button interactable
    }

}
