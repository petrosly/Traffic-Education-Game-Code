using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class Registration : MonoBehaviour{

    public InputField usernameField;
    public InputField passwordField;
    public Button submitButton;
    private string connectionStr;
    public Text userExists;
    


    public void verifyInputLengths(){
    	submitButton.interactable = (usernameField.text.Length >= 4 && passwordField.text.Length >=5);
    }

    //check if username exists
    public bool usernameNotExists(){
    	userExists.gameObject.SetActive(false); //disable UI before re-checking if user exists.
		IDbCommand dbcmd;
		IDataReader reader;
		IDbConnection dbcon = new SqliteConnection(connectionStr);
		dbcon.Open();
		dbcmd = dbcon.CreateCommand();
		string q_selectFromTable = "SELECT username FROM players WHERE username='" + usernameField.text + "';";

		dbcmd.CommandText = q_selectFromTable;
		reader = dbcmd.ExecuteReader();

		if(reader.Read() == false){ //username passed check
			return true;
		}
		
		return false;
    }

	public void insertUser(){      
    	if(usernameNotExists()){
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]); //generate salt
			var pbkdf2 = new Rfc2898DeriveBytes(passwordField.text, salt, 10000);
			byte[] hashBytes= pbkdf2.GetBytes(20);

			string savedHash = Convert.ToBase64String(hashBytes);
			string savedSalt = Convert.ToBase64String(salt);
					
			IDbCommand dbcmd;
			IDbConnection dbcon = new SqliteConnection(connectionStr);
		    dbcon.Open(); 
			dbcmd = dbcon.CreateCommand();
			string q_insertToPlayers = "INSERT INTO players (username,hash,salt) VALUES ('" 
				+ usernameField.text + "','"+savedHash+"','"+ savedSalt+"');" ;
			dbcmd.CommandText = q_insertToPlayers;
			dbcmd.ExecuteNonQuery();
			string q_insertToPlayerScores ="INSERT INTO playerScores (numOfSignsQuestions) VALUES (0);";
			dbcmd.CommandText = q_insertToPlayerScores;
			int a = dbcmd.ExecuteNonQuery();
			dbcon.Close(); //closed connection to Database
			SceneManager.LoadScene("loginMenu"); // if the user was created go to login before playing.
    	}else{
    		userExists.gameObject.SetActive(true);//enable UI that alerts username is taken
    	}
    }

    public void goToMenu(){
    	SceneManager.LoadScene("MainMenu");
    }

    void Start(){
		connectionStr = "URI=file:" + Application.persistentDataPath + "/trafficDb.db";
	}


}
