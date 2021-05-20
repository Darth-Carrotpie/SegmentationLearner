using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelMaskCoordinator : Singleton<LabelMaskCoordinator> {
#if UNITY_EDITOR
    [NameList(typeof(PropertyDrawersHelper), "AllLabelNames")]
#endif
    public SettableNameList maskState;

    public static void SetState(string labelName, bool state) {
        if (state) {
            if (!Instance.maskState.list.Contains(labelName))
                Instance.maskState.list.Add(labelName);
        } else {
            if (Instance.maskState.list.Contains(labelName))
                Instance.maskState.list.Remove(labelName);
        }
    }

    public static void SetAllStates(bool state) {
        Instance.maskState.list.Clear();
        if (state) {
            Instance.maskState.list.AddRange(LabelName.Get());
        }
    }
    public static bool HasState(string labelName) {
        if (Instance.maskState.list.Contains(labelName))
            return true;
        return false;
    }

    public static int[] GetCurrentStateIDs() {
        return Instance.maskState.indexes.ToArray();
    }
}