using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverlayShaderController : Singleton<OverlayShaderController>
{
    public RawImage imageRend;
    void Start()
    {
        if(imageRend == null) imageRend = GetComponent<RawImage>();
    }

    public static void SetLabelColor(Color color, int label, float alpha){
        Instance.SetMatValues(color, label, alpha);
    }

    void SetMatValues(Color color, int label, float alpha)
    {
        string labelStr = "Label"+label.ToString();
        imageRend.material.SetFloat(labelStr, label);
        string colorStr = "Label"+label.ToString();
        imageRend.material.SetColor(colorStr, color);
        imageRend.material.SetFloat("Aplha", alpha);
    }
}
