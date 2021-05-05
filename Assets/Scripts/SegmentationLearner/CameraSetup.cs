using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour {
    public GameObject mainCam;

    private GameObject normalCamera;
    private GameObject labelCamera;

    public RenderTexture normalRendTexture;
    public RenderTexture labelRendTexture;
    void Start() {
        SetupCameras();
        CameraCapture.Setup(normalCamera.GetComponent<Camera>(), labelCamera.GetComponent<Camera>());
    }

    void SetupCameras() {
        labelCamera = Instantiate(mainCam, mainCam.transform.position, Quaternion.identity, mainCam.transform.parent);
        labelCamera.name = "Label Camera";

        int defLayer = LayerMask.NameToLayer("Default");
        int labLayer = LayerMask.NameToLayer("LabelLayer");
        int markLayer = LayerMask.NameToLayer("MakerLayer");

        labelCamera.GetComponent<Camera>().cullingMask = (0 << defLayer) | (1 << labLayer);
        labelCamera.GetComponent<Camera>().targetTexture = labelRendTexture;

        normalCamera = Instantiate(mainCam, mainCam.transform.position, Quaternion.identity, mainCam.transform.parent);
        normalCamera.name = "Regular Camera";
        normalCamera.GetComponent<Camera>().cullingMask = (0 << markLayer) | (1 << defLayer);
        normalCamera.GetComponent<Camera>().targetTexture = normalRendTexture;
        
        normalCamera.transform.parent = mainCam.transform;
        labelCamera.transform.parent = mainCam.transform;
    }
}