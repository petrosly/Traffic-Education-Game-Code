using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wrongCrossingScript : MonoBehaviour
{
    public GameObject wrongCrossingUI;

    
    public void OnTriggerEnter(Collider other) {
        wrongCrossingUI.SetActive(true);
    }

    public void levelRestart() {
        wrongCrossingUI.SetActive(false);
        SceneManager.LoadScene("1stLevelScene");
    }
}
