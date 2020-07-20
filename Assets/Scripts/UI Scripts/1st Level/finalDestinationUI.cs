using UnityEngine;
using System.Data;
using System;
using System.Dynamic;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class finalDestinationUI : MonoBehaviour{

    public GameObject finalDestUI;
    public GameObject halfDestUI;
    private string connectionStr;
    public static bool wentLeft;
    public static bool wentRight;
    public GameObject leftCollider;
    public GameObject rightCollider;

    public void OnTriggerEnter(Collider other) {
    	IDbCommand dbcmd;
    	IDataReader reader;
		IDbConnection dbcon = new SqliteConnection(connectionStr);
		int score;
	    dbcon.Open(); 
		dbcmd = dbcon.CreateCommand();
		string q_selectScoreFromPlayers = "SELECT score FROM players WHERE id="+Login.loginID+";";
        dbcmd.CommandText = q_selectScoreFromPlayers;
        reader = dbcmd.ExecuteReader();
        score = Convert.ToInt32(reader[0]);
        dbcon.Close();
        if(score == 10 && finalDestUI!=null){
        	finalDestUI.SetActive(true);
    	}else{
            if(halfDestUI!=null){
             halfDestUI.SetActive(true);   
            }
    	}
    }

    public void restartLvlOne(){
    	SceneManager.LoadScene("1stLevelScene");
    }

    public void goToLvlTwo(){
    	SceneManager.LoadScene("2ndLevelScene");
    }

    void Start(){
        connectionStr = "URI=file:" + Application.persistentDataPath + "/trafficDb.db";
    }

    void Awake(){
        DontDestroyOnLoad(this.gameObject);
        if(wentLeft){
            leftCollider.SetActive(true);
        }else if(wentRight){
            rightCollider.SetActive(true);
        }
    }
}
