using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotGeneratorHandler : MonoBehaviour {
    int snapshotAmount = 10;

    public void OnAmountValueChanged(string newVal) {
        int.TryParse(newVal, out snapshotAmount);
    }

    public void OnGenerateClick() {
        PointCloudCoordinator.RegeneratePointCloud(snapshotAmount);
        StartCoroutine(CaptureAll());
    }
    IEnumerator CaptureAll(){
        int i = 0;
        Camera cam = Camera.main;
        while(i < snapshotAmount)
        {
            Transform tr = PointCloudCoordinator.NextPoint();
            cam.transform.position = tr.position;
            cam.transform.rotation = tr.rotation;
            CameraCapture.Capture();
            i++;
            yield return null;
        }
    }
    public void OnDeleteClick() {
        CameraCapture.RemoveCaptured();
    }
}