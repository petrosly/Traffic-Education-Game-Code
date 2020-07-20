using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trafficLightInfo : MonoBehaviour
{
    public GameObject trafficLightUI;
    private GameObject[] trafficLightArray;
    public void OnTriggerEnter(Collider other) {
        trafficLightUI.SetActive(true);
    }

    public void closeLightUI() {
        trafficLightUI.SetActive(false);
        trafficLightArray = GameObject.FindGameObjectsWithTag("Traffic Light Trigger");
        foreach (GameObject trigger in trafficLightArray) {
            trigger.SetActive(false);
        }
    }
}
