using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockpickingMinigame : MonoBehaviour
{
    bool gameIsActive = false;

    float pinAngle;

    [SerializeField] RectTransform pinRectTransform;
    [SerializeField] RectTransform smallLockRectTransform;
    [SerializeField] Text difficultyText;
    [SerializeField] Text timerText;
    [SerializeField] GameObject resultsPanel;

    [SerializeField] float allowedTime;

    float pinRotateSpeed = 2f;

    public LockController lockController;

    bool attemptingLock = false;

    private void Start() {
        difficultyText.text = "Lock Difficulty: " + lockController.GetLockDifficulty().ToString();
        timerText.text = "Time Left: " + allowedTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsActive) {
            if (!attemptingLock) {
                float mouseX = Input.GetAxis("Mouse X") * pinRotateSpeed;
                RotatePin(mouseX);
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                bool correctAngle;

                // Check if pinAngle is in between the correct range
                if (-pinAngle >= lockController.GetLockpickRangeMin() && -pinAngle <= lockController.GetLockpickRangeMax()) {
                    correctAngle = true;
                }
                else {
                    correctAngle = false;
                }

                float distFromPinAngleToLockpickRangeMinimum = Mathf.Abs(-pinAngle - lockController.GetLockpickRangeMin());
                float distFromPinAngleToLockpickRangeMaximum = Mathf.Abs(-pinAngle - lockController.GetLockpickRangeMax());

                float maxTurnAngle;

                if (distFromPinAngleToLockpickRangeMinimum < distFromPinAngleToLockpickRangeMaximum) {
                    Debug.Log("Closer to Min");
                    // pass in Min
                    maxTurnAngle = distFromPinAngleToLockpickRangeMinimum;
                }
                else {
                    Debug.Log("Closer to Max");
                    // pass in Min
                    maxTurnAngle = distFromPinAngleToLockpickRangeMaximum;
                }

                if (!correctAngle) {
                    maxTurnAngle = Mathf.Clamp(maxTurnAngle, 0, 90);
                    maxTurnAngle = 90 - maxTurnAngle;
                }
                else {
                    maxTurnAngle = 90;
                }

                Debug.Log("Max Turn Angle: " + maxTurnAngle);
                StartCoroutine(AnimatePinRotation(maxTurnAngle, correctAngle));
            }

            allowedTime -= Time.deltaTime;
            
            if (allowedTime <= 0) {
                allowedTime = 0;
                gameIsActive = false;
            }

            timerText.text = "Time Left: " + Mathf.Round(allowedTime).ToString();
        }
        else {
            resultsPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void RotatePin(float degrees) {
        pinAngle -= degrees;
        pinAngle = Mathf.Clamp(pinAngle, -180, 0);

        pinRectTransform.rotation = Quaternion.Euler(0, 0, pinAngle);
    }


    IEnumerator AnimatePinRotation(float maxTurnAngle, bool correctAngle) {
        attemptingLock = true;
        float currentAngle = smallLockRectTransform.rotation.eulerAngles.z;

        while (Mathf.Abs(-currentAngle - maxTurnAngle) > 0.1 && Input.GetKey(KeyCode.E)) { 
            float newAngle = Mathf.Lerp(-currentAngle, maxTurnAngle, 0.1f);

            smallLockRectTransform.rotation = Quaternion.Euler(0, 0, -newAngle);

            currentAngle = -newAngle; 

            yield return new WaitForSeconds(0.05f);
        }

        attemptingLock = false;

        if (correctAngle && Input.GetKey(KeyCode.E)) {
            //gameObject.SetActive(false);
            resultsPanel.SetActive(true);
            resultsPanel.GetComponentInChildren<Text>().text = "You successfully picked the lock!";
            gameIsActive = false;
        }
        else {

            smallLockRectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnEnable() {
        gameIsActive = true;
    }

    private void OnDisable() {
        gameIsActive = false;

        FindObjectOfType<FPSControls>().lockpicking = false;
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(gameObject);
    }
}
