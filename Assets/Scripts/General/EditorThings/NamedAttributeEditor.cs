/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NamedPriceAttribute))]
//[CanEditMultipleObjects]
public class NamedAttributeEditor : Editor
{
    string[] _choices = new[] { "foo", "foobar" };
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        var theClass = target as NamedPriceAttribute;
        _choiceIndex = System.Array.IndexOf(_choices,theClass.attributeName);
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
        // Update the selected choice in the underlying object
        theClass.attributeName = _choices[_choiceIndex];
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
        DrawDefaultInspector();
    }
}
 */