using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    public GameObject wrongCrossingUI;
    public GameObject instructionsUI;
    public GameObject crosswalkUI;
    public GameObject playerInstructionsUI;
    public GameObject stopSignUI;
    public GameObject trafficLightUI;
    public GameObject finalDestUI;
    public GameObject halfDestUI;
    public GameObject goLeft;
    public GameObject goRight;

    // Start is called before the first frame update
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        if (wrongCrossingUI.activeSelf || instructionsUI.activeSelf || crosswalkUI.activeSelf || playerInstructionsUI.activeSelf || stopSignUI.activeSelf || trafficLightUI.activeSelf || finalDestUI.activeSelf || halfDestUI.activeSelf || goLeft.activeSelf || goRight.activeSelf) {
            Time.timeScale = 0f; //Game is paused when UI is open.
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f; //Game is resumed.
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -30f, 30f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
