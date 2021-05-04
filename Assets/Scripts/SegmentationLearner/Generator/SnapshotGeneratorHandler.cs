using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotGeneratorHandler : MonoBehaviour {
    int snapshotAmount = 50;

    public void OnAmountValueChanged(string newVal) {
        int.TryParse(newVal, out snapshotAmount);
    }

    public void OnGenerateClick() {
        PointCloudCoordinator.RegeneratePointCloud(snapshotAmount);
        Camera cam = Camera.main;
        for (int i = 0; i < snapshotAmount; i++) {
            Transform tr = PointCloudCoordinator.NextPoint();
            cam.transform.position = tr.position;
            cam.transform.rotation = tr.rotation;
            CameraCapture.Capture();
        }
    }
    public void OnDeleteClick() {
        CameraCapture.RemoveCaptured();
    }
}