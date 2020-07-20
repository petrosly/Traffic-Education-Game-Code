using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopSignInfo : MonoBehaviour
{
    public GameObject stopSignUI;
    private GameObject[] stopSignArray;
    public void OnTriggerEnter(Collider other) {
        stopSignUI.SetActive(true);
    }

    public void closeStopUI() {
        stopSignUI.SetActive(false);
        stopSignArray = GameObject.FindGameObjectsWithTag("Stop Sign");
        foreach (GameObject trigger in stopSignArray) {
            trigger.SetActive(false);
        }
    }
}
