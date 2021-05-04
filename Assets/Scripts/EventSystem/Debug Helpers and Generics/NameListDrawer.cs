#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(NameListAttribute))]
public class NameListDrawer : PropertyDrawer {
    private List<int> SelectedIndexed = new List<int>();
    void OnPointSelected(object index) {
        var intIndex = (int)index;

        if (SelectedIndexed.Contains(intIndex)) {
            SelectedIndexed.Remove(intIndex);
        } else {
            SelectedIndexed.Add(intIndex);
        }
        SelectedIndexed.Sort();
    }
    private void DrawPointSelectorInspector(Rect rect, SerializedProperty property, string[] data) {
        System.Text.StringBuilder selectedPointButtonSb = new System.Text.StringBuilder();

        if (SelectedIndexed.Count == 0) {
            selectedPointButtonSb.Append("Select events to ignore.");
        } else {
            selectedPointButtonSb.Append("Ignored:");
            foreach (int i in SelectedIndexed) {
                selectedPointButtonSb.Append($"{i},");
            }
        }

        if (GUI.Button(rect, selectedPointButtonSb.ToString())) {
            GenericMenu selectedMenu = new GenericMenu();

            for (var i = 0; i < data.Length; ++i) {
                string menuString = $"{i}-{data[i]}";
                bool selected = SelectedIndexed.Contains(i);
                selectedMenu.AddItem(new GUIContent(menuString), selected, OnPointSelected, i);
            }

            selectedMenu.ShowAsContext();
        }
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        SerializedProperty pList = property.FindPropertyRelative("list");
        SerializedProperty pIndexes = property.FindPropertyRelative("indexes");
        NameListAttribute attr = attribute as NameListAttribute;
        string[] fullList = attr.FullList;
        //initial load data:
        int starter = 0;
        if (SelectedIndexed.Count == 0)
            for (int i = 0; i < pList.arraySize; i++) {
                for (int j = starter; j < fullList.Length; j++) {
                    if (pList.GetArrayElementAtIndex(i).stringValue == fullList[j]) {
                        starter = j + 1;
                        SelectedIndexed.Add(j);
                        break;
                    }
                }
            }

        //Show button, show pop up when btn clicked and Set editor values on clicks
        DrawPointSelectorInspector(position, property, fullList);

        //Save the values to the serialized object within the class of this drawer:
        //SerializedProperty pList = property.serializedObject.FindProperty("list");
        pList.ClearArray();
        pIndexes.ClearArray();
        for (int i = 0; i < SelectedIndexed.Count; i++) {
            pList.AddArrayElement(fullList[SelectedIndexed[i]]);
            pIndexes.AddArrayElement(SelectedIndexed[i]);
        }
        /*string outp = "";
                outp += "elements: ";
                for (int i = 0; i < pList.arraySize; i++) {
                    outp += pList.GetArrayElementAtIndex(i).stringValue;
                    outp += ";   ";
                }
                Debug.Log(outp); */
    }
}
#endif