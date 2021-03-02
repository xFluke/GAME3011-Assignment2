using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickingMinigame : MonoBehaviour
{
    bool gameIsActive = false;

    float pinAngle;

    public RectTransform pinRectTransform;
    public RectTransform smallLockRectTransform;

    float pinRotateSpeed = 2f;

    public LockController lockController;

    bool attemptingLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
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

                Debug.Log("Angle is " + correctAngle);

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

        while (Mathf.Abs(-currentAngle - maxTurnAngle) > 0.1) { 
            float newAngle = Mathf.Lerp(-currentAngle, maxTurnAngle, 0.1f);

            smallLockRectTransform.rotation = Quaternion.Euler(0, 0, -newAngle);

            currentAngle = -newAngle; 

            yield return new WaitForSeconds(0.05f);
        }

        attemptingLock = false;

        if (correctAngle) {
            gameObject.SetActive(false);
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
    }
}
