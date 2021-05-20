using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayColorController : Singleton<OverlayColorController> {
    RawImage imageRend;
    float currentOpacity = .5f;
    void Awake() {
        if (imageRend == null)imageRend = GetComponent<RawImage>();
    }

    public static void IncreaseOpacity() {
        Instance.currentOpacity += 0.1f;
        Instance.SetOpacity(Instance.currentOpacity);
    }
    public static void DecreaseOpacity() {
        Instance.currentOpacity -= 0.1f;
        Instance.SetOpacity(Instance.currentOpacity);
    }
    public void SetOpacity(float alpha) {
        imageRend.color = new Color(1, 1, 1, alpha);
    }

    public static void ChangeShowState() {
        Show(!Instance.imageRend.enabled);
    }

    public static void Show(bool state) {
        Instance.imageRend.enabled = state;
    }
}