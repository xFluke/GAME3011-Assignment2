using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControls : MonoBehaviour
{
    float xRotation = 0f;
    float cameraRotationSpeed = 200f;

    float speed = 10f;

    bool inRangeOfDoor = false;
    bool lockpicking = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        CameraMovement();
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.E) && inRangeOfDoor) {
            Debug.Log("Unlocking Door");
            lockpicking = true;
        }

    }

    private void PlayerMovement() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        GetComponent<CharacterController>().Move(move * speed * Time.deltaTime);
    }

    private void CameraMovement() {
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Door")) {
            inRangeOfDoor = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Door")) {
            inRangeOfDoor = false;
        }
    }
}
