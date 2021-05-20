using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseLabel {
    public Color32 encoderColor;
#if UNITY_EDITOR
    [StringInList(typeof(PropertyDrawersHelper), "AllLabelNames")]
#endif
    public string labelName;
}