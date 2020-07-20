using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goLeft : MonoBehaviour{

	public GameObject goLeftUI;
	public GameObject closeRightUI;

	void OnTriggerEnter(){
		goLeftUI.SetActive(true);
	}

	public void closeUI(){
		goLeftUI.SetActive(false);
		closeRightUI.SetActive(false);
	}

}
