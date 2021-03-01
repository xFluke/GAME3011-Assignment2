using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickingMinigame : MonoBehaviour
{
    bool gameIsActive = false;

    float pinAngle;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsActive) {

        }
    }

    void RotatePin(float degrees) {
        pinAngle += degrees;
        pinAngle = Mathf.Clamp(pinAngle, 0, 180);
    }

    private void OnEnable() {
        gameIsActive = true;
    }

    private void OnDisable() {
        gameIsActive = false;
    }
}
