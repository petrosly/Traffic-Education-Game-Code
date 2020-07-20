using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class mainMenu : MonoBehaviour{

	private string connectionStr;
    private string filepath;
    private string dbConnection;
    public void goToRegister(){
    	SceneManager.LoadScene("registerMenu");
    }
    
    public void goToLogin(){
    	SceneManager.LoadScene("loginMenu");
    }

    public void closeApplication() {
        Application.Quit();
    }

    void Start(){

        if (!File.Exists(filepath)) {
            File.Copy(connectionStr,filepath);
        }
    	IDbCommand dbcmd;
		IDbConnection dbcon = new SqliteConnection(dbConnection);
	    dbcon.Open(); 
		dbcmd = dbcon.CreateCommand();
		string q_createPlayers = "CREATE TABLE IF NOT EXISTS players (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT UNIQUE, hash TEXT,"
                           + " salt TEXT, score INTEGER DEFAULT 0)" ;
		dbcmd.CommandText = q_createPlayers;
		dbcmd.ExecuteNonQuery();
		string q_createPlayerScores = "CREATE TABLE IF NOT EXISTS playerScores (id	INTEGER PRIMARY KEY AUTOINCREMENT, scoreCatOne REAL DEFAULT 0,"
            + " scoreCatTwo REAL DEFAULT 0,scoreCatThree REAL DEFAULT 0,scoreCatFour REAL DEFAULT 0, numOfSignsQuestions INTEGER DEFAULT 0,"
            + " numOfPedestrianQuestions INTEGER DEFAULT 0, numOfBicycleQuestions INTEGER DEFAULT 0, numOfGeneralQuestions INTEGER DEFAULT 0)";
		dbcmd.CommandText = q_createPlayerScores;
		dbcmd.ExecuteNonQuery();
		dbcon.Close();
    }

    void Awake(){
        filepath = Application.persistentDataPath + "/trafficDb.db";
        dbConnection = "URI=file:" + filepath;
        connectionStr = Application.dataPath + "/StreamingAssets" + "/trafficDb.db";
    }
}
