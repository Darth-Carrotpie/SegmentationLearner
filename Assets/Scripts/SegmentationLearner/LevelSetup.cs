using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LevelSetup : MonoBehaviour {
    public GameObject environment;
    public Material labelMaterial;

    private GameObject labelEnvironment;
    public int totalLabeledItems;
    public int totalUniqueLabels;
    public int totalVoids;
    List<string> labeledNames = new List<string>();

    void Start() {
        SetupEnv();
        totalUniqueLabels = labeledNames.Count;
        CsvWriter.SaveLabels(LabelsBucket.GetLabels().Select(pair => pair.Key).ToList());
        Debug.Log("Total Labeled items: "+totalLabeledItems+" of which unique: "+totalUniqueLabels+" total void items: "+totalVoids);
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
        Renderer rend = tr.GetComponent<Renderer>();
        if (rend != null) {
            Material[] curMats = rend.materials;
            for (int i = 0; i < rend.materials.Length; i++) {
                curMats[i] = new Material(labelMaterial);
                LabelIdentity identity = rend.GetComponent<LabelIdentity>();
                int colVal = 0;
                if (identity != null) {
                    colVal = LabelsBucket.GetLabelIndex(identity) + 1;
                    totalLabeledItems++;
                    if (!labeledNames.Contains(identity.labelName))
                        labeledNames.Add(identity.labelName);
                } else {
                    Debug.Log("identity component not found on: " + tr.name + " each renderable must have an LabelIdentity component with a type set. Setting to void Val=[0]");
                    totalVoids++;
                }
                curMats[i].color = new Color32((byte)colVal, (byte)colVal, (byte)colVal, 255);
            }
            rend.materials = curMats;
        } else {
            //Debug.Log("MechRenderer not found on: " + tr.name + " , won't set.");
        }
    }

}