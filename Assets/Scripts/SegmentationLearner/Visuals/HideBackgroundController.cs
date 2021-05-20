using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideBackgroundController : Singleton<HideBackgroundController> {
    RawImage imageRend;
    float currentOpacity = .5f;
    void Awake() {
        if (imageRend == null)imageRend = GetComponent<RawImage>();
    }

    public static void ChangeShowState() {
        Show(!Instance.imageRend.enabled);
    }

    public static void Show(bool state) {
        Instance.imageRend.enabled = state;
    }
}