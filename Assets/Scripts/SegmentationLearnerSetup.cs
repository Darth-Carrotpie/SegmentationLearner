using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentationLearnerSetup : MonoBehaviour {
    public GameObject cam;
    public GameObject environment;
    public Material labelMaterial;

    private GameObject labelCamera;
    private GameObject labelEnvironment;

    public int totalLabeledItems;
    public int totalUniqueLabels;
    List<string> labeledNames = new List<string>();

    void Start() {
        SetupLabelCamera();
        SetupEnv();
        totalUniqueLabels = labeledNames.Count;
    }

    void SetupLabelCamera() {
        labelCamera = Instantiate(cam, cam.transform.position, Quaternion.identity, transform);

        int defLayer = LayerMask.NameToLayer("Default");
        int labLayer = LayerMask.NameToLayer("LabelLayer");

        labelCamera.GetComponent<Camera>().cullingMask = (0 << defLayer) | (1 << labLayer);
    }

    void SetupEnv() {
        labelEnvironment = Instantiate(environment, environment.transform.position, Quaternion.identity, transform);
        SetChildrenLayer(labelEnvironment.transform, "LabelLayer");
    }

    void SetChildrenLayer(Transform tr, string layer) {
        Transform trs = tr.GetComponentInChildren<Transform>(true);
        foreach (Transform t in trs) {
            t.gameObject.layer = LayerMask.NameToLayer(layer);
            TrySetLabelMat(t);
            SetChildrenLayer(t, layer);
        }
    }

    void TrySetLabelMat(Transform tr) {
        MeshRenderer rend = tr.GetComponent<MeshRenderer>();
        if (rend != null) {
            rend.material = labelMaterial;
            LabelIdentity identity = rend.GetComponent<LabelIdentity>();
            if (identity != null) {
                rend.material.color = LabelsBucket.GetLabel(identity).encoderColor;
                totalLabeledItems++;
                if (!labeledNames.Contains(identity.labelName))
                    labeledNames.Add(identity.labelName);
            } else {
                Debug.Log("identity component not found on: " + tr.name + " each renderable must have an LabelIdentity component with a type set.");
            }
        } else {
            //Debug.Log("MechRenderer not found on: " + tr.name + " , won't set.");
        }
    }
}