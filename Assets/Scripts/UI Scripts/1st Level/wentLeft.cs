using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;


public class wentLeft : MonoBehaviour{
  private GameObject player;
  public GameObject crossedLeftCollider; // it is used to stop player from going right after crossing that path
  private string connectionStr;
  private int existingScore;


  void OnTriggerExit(Collider other){
    player = GameObject.Find("First Person Player");
    if(player.transform.position.z >= 29 && player.transform.position.x < 0){
      IDbConnection dbcon = new SqliteConnection(connectionStr);
      dbcon.Open();
      string q_selectScoreFromPlayers = "SELECT score FROM players WHERE id= "+Login.loginID+";";
      IDbCommand dbcmd = dbcon.CreateCommand();
      dbcmd.CommandText = q_selectScoreFromPlayers;
      using(IDataReader reader = dbcmd.ExecuteReader()){
        existingScore= Convert.ToInt32(reader[0]);
        reader.Close();
      }
      existingScore += 5;
      string q_updateScoreToPlayers = "UPDATE players SET score=" +existingScore +" WHERE id= "+Login.loginID+";";
      dbcmd.CommandText = q_updateScoreToPlayers;
      using(IDataReader reader = dbcmd.ExecuteReader()){
        reader.Close();
      }
      setColliderActive();
      finalDestinationUI.wentLeft = true;
      dbcon.Close();
    }
  }

  void setColliderActive(){
    crossedLeftCollider.SetActive(true);
  }

  void Start(){
        connectionStr = "URI=file:" + Application.persistentDataPath + "/trafficDb.db";
    }
}  
