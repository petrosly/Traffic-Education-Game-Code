using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goRight : MonoBehaviour{

	public GameObject goRightUI;
	public GameObject closeLeftUI;

	void OnTriggerEnter(Collider other){
		goRightUI.SetActive(true);
	}

	public void closeUI(){
		goRightUI.SetActive(false);
		closeLeftUI.SetActive(false);
	}
}
