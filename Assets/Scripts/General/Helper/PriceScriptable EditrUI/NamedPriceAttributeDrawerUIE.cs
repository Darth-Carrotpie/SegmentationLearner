/* #if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomPropertyDrawer(typeof(PriceBase))]
public class NamedPriceAttributeDrawerUIE : PropertyDrawer {
    /*     public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var amountField = new PropertyField(property.FindPropertyRelative("basePrice"), "Base Price");
            var unitField = new PropertyField(property.FindPropertyRelative("increment"), "Increment");
            var nameField = new PropertyField(property.FindPropertyRelative("level"), "Level");

            // Add fields to the container.
            container.Add(amountField);
            container.Add(unitField);
            container.Add(nameField);

            return container;
        }

// Draw the property inside the given rect
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    // Using BeginProperty / EndProperty on the parent property means that
    // prefab override logic works on the entire property.
    EditorGUI.BeginProperty(position, label, property);
    // Draw label
    position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

    // Don't make child fields be indented
    var indent = EditorGUI.indentLevel;
    EditorGUI.indentLevel = 0;

    float width = 40f;
    float labelW = 35f;
    // Calculate rects
    //      var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleRight};
    //EditorGUILayout.LabelField("Blabla", style, GUILayout.ExpandWidth(true));
    EditorGUIUtility.labelWidth = labelW; // Replace this with any width
    var amountRect = new Rect(position.x, position.y, width + labelW, position.height);
    var unitRect = new Rect(position.x + width + labelW, position.y, width + labelW, position.height);
    var nameRect = new Rect(position.x + (width + labelW) * 2, position.y, position.width - (width + labelW) * 2 + labelW, position.height);

    // Draw fields - passs GUIContent.none to each so they are drawn without labels
    EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("basePrice"), new GUIContent("base:", "Base Price. Y = BP + inc * Lvl."));
    EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("increment"), new GUIContent("inc:", "Increment per level change."));
    EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("level"), new GUIContent("lvl:", "Current Level. Should be 0 to start with."));

    // Set indent back to what it was
    EditorGUI.indentLevel = indent;

    EditorGUI.EndProperty();
}
}
#endif */