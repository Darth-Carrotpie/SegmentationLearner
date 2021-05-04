#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine;

public static class EditorExtension {
    public static int DrawBitMaskField(Rect aPosition, int aMask, System.Type aType, GUIContent aLabel) {
        var itemNames = System.Enum.GetNames(aType);
        var itemValues = System.Enum.GetValues(aType)as int[];

        int val = aMask;
        int maskVal = 0;
        for (int i = 0; i < itemValues.Length; i++) {
            if (itemValues[i] != 0) {
                if ((val & itemValues[i]) == itemValues[i])
                    maskVal |= 1 << i;
            } else if (val == 0)
                maskVal |= 1 << i;
        }
        int newMaskVal = EditorGUI.MaskField(aPosition, aLabel, maskVal, itemNames);
        int changes = maskVal ^ newMaskVal;

        for (int i = 0; i < itemValues.Length; i++) {
            if ((changes & (1 << i)) != 0) // has this list item changed?
            {
                if ((newMaskVal & (1 << i)) != 0) // has it been set?
                {
                    if (itemValues[i] == 0) // special case: if "0" is set, just set the val to 0
                    {
                        val = 0;
                        break;
                    } else
                        val |= itemValues[i];
                } else // it has been reset
                {
                    val &= ~itemValues[i];
                }
            }
        }
        return val;
    }
    public static void AddArrayElement(this SerializedProperty prop, int elementValue) {
        prop.arraySize++;
        prop.GetArrayElementAtIndex(prop.arraySize - 1).intValue = elementValue;
    }
    public static void AddArrayElement(this SerializedProperty prop, string elementValue) {
        prop.arraySize++;
        prop.GetArrayElementAtIndex(prop.arraySize - 1).stringValue = elementValue;
    }
    public static void RemoveArrayElement(this SerializedProperty prop, int elementValue) {
        int toDel = 0;
        for (int i = 0; i < prop.arraySize; i++) {
            if (prop.GetArrayElementAtIndex(prop.arraySize - 1).intValue == elementValue) {
                toDel = i;
            }
        }
        prop.DeleteArrayElementAtIndex(toDel);
    }
}

[CustomPropertyDrawer(typeof(BitMaskAttribute))]
public class EnumBitMaskPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
        var typeAttr = attribute as BitMaskAttribute;
        // Add the actual int value behind the field name
        label.text = label.text + "(" + prop.intValue + ")";
        prop.intValue = EditorExtension.DrawBitMaskField(position, prop.intValue, typeAttr.propType, label);
    }
}
#endif