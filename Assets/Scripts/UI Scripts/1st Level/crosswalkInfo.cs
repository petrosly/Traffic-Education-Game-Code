using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crosswalkInfo : MonoBehaviour
{
    public GameObject crosswalkUI;
    private GameObject[] array;
    public void OnTriggerEnter(Collider other) {
        crosswalkUI.SetActive(true);
    }

    public void closeCrosswalkUI() {
        array = GameObject.FindGameObjectsWithTag("Crosswalk Trigger");
        foreach (GameObject trigger in array) {
            trigger.SetActive(false);
        }
        crosswalkUI.SetActive(false);
    }

}
