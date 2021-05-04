using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelIdentity : MonoBehaviour {
#if UNITY_EDITOR
    [StringInList(typeof(PropertyDrawersHelper), "AllLabelNames")]
#endif
    [SerializeField]
    public string labelName;
}