using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayShaderController : MonoBehaviour {
    Image imageRend;
    public string labelName;
    void Awake() {
        if (imageRend == null)imageRend = GetComponent<Image>();
    }

    public void SetColorValues(Color color, int label, string name) {
        imageRend.material = new Material(imageRend.material);
        imageRend.material.SetFloat("LabelValue", label);
        imageRend.material.SetColor("DisplayColor", color);
        labelName = name;
    }

    public void SetOpacity(float alpha) {
        imageRend.material.SetFloat("Aplha", alpha);
    }
}