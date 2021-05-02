using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {
    public GameObject environment;
    public Material labelMaterial;

    private GameObject labelEnvironment;
    public int totalLabeledItems;
    public int totalUniqueLabels;
    List<string> labeledNames = new List<string>();

    void Start() {
        SetupEnv();
        totalUniqueLabels = labeledNames.Count;
        CsvWriter.SaveLabels(LabelsBucket.GetLabels());
    }

    void SetupEnv() {
        labelEnvironment = Instantiate(environment, environment.transform.position, Quaternion.identity, environment.transform.parent);
        labelEnvironment.name = "Label Environment";
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