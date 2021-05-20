using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiJsonHandler : Singleton<ApiJsonHandler> {
    public static string LabelColors() {

        LabelClass labelClass = new LabelClass();
        int len = LabelsBucket.GetLabels().Count;
        int labelClassIndex = 0;
        labelClass.colors = new Color32[len];
        labelClass.labels = new int[len];
        foreach (KeyValuePair<BaseLabel, int> pair in LabelsBucket.GetLabels()) {
            labelClass.labels[labelClassIndex] = pair.Value;
            if (LabelMaskCoordinator.HasState(pair.Key.labelName))
                labelClass.colors[labelClassIndex] = pair.Key.encoderColor;
            else
                labelClass.colors[labelClassIndex] = new Color32(0, 0, 0, 0);

            //Debug.Log(pair.Key.encoderColor + " " + pair.Value + " " + pair.Key.labelName);
            labelClassIndex += 1;
        }

        labelClass.mime = "mimey";
        string json = JsonUtility.ToJson(labelClass);
        return json;
    }

    public static string GetPrediction(byte[] imageBytes) {
        string encodedText = Convert.ToBase64String(imageBytes);
        DataClass dataClass = new DataClass();
        dataClass.mime = "mimey";
        dataClass.image64 = Convert.ToBase64String(imageBytes);
        return JsonUtility.ToJson(dataClass);
    }
}