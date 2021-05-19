using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {

        }

        if (Input.GetKeyDown(KeyCode.P)) {
            ModelApiCoordinator.Predict(CameraCapture.CaptureScreenshot());
        }
    }
}