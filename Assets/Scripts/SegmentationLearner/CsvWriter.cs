using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CsvWriter : Singleton<CsvWriter> {

    void Start() {}

    public static void SaveLabels(List<BaseLabel> labels) {

        List<string[]> rowData = new List<string[]>();
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "LabelName";
        rowDataTemp[1] = "LabelColor_Red";
        rowDataTemp[2] = "LabelColor_Green";
        rowDataTemp[3] = "LabelColor_Blue";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.

        for (int i = 0; i < labels.Count; i++) {
            rowDataTemp = new string[4];
            Color32 col = labels[i].encoderColor;
            rowDataTemp[0] = labels[i].labelName;
            rowDataTemp[1] = col.r.ToString();
            rowDataTemp[2] = col.g.ToString();
            rowDataTemp[3] = col.b.ToString();
            rowData.Add(rowDataTemp);
        }
        Debug.Log("Saving " + rowData.Count + " Labels to CSV in " + Instance.getPath());
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++) {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        string filePath = Instance.getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath() {
#if UNITY_EDITOR
        return Application.dataPath + "/Data~/" + "labels.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath + "labels.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath + "/" + "labels.csv";
#else
        return Application.dataPath + "/" + "labels.csv";
#endif
    }
}