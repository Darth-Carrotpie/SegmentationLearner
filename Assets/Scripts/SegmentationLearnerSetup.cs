using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentationLearnerSetup : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CameraCapture.Capture();
        }
    }

}