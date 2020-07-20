using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasController : MonoBehaviour
{
    public GameObject instructionsUI;
    public GameObject playerInstructionsUI;

    private void Start() {
        instructionsUI.SetActive(true);
    }

    public void furtherInfo() {
        instructionsUI.SetActive(false);
        playerInstructionsUI.SetActive(true);
    }

    public void startLvl() {
        playerInstructionsUI.SetActive(false);
    }

}
