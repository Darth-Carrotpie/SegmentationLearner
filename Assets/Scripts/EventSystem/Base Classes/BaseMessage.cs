using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
public class BaseMessage {
    public override string ToString() {
        string output = "";
        Type type = typeof(GameMessage);
        BaseMessage msg = this as GameMessage;

        List<string> boolNames = new List<string>();
        FieldInfo[] boolFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in boolFields) {
            if (field.FieldType == typeof(bool)) {
                bool isSet = bool.Parse(field.GetValue(msg).ToString());
                if (isSet)
                    boolNames.Add(field.Name.Remove(field.Name.Length - 3));
            }
        }

        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties) {
            if (!boolNames.Any(property.Name.Contains))continue;
            object val = property.GetValue(msg, null);
            if (val == null)
                continue;
            output += "(" + property.PropertyType + ")";
            output += property.Name + " = ";
            output += val + ";\n";
        }

        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields) {
            if (!boolNames.Any(field.Name.Contains))continue;
            object val = field.GetValue(msg);
            if (val == null)
                continue;
            output += "(" + field.FieldType + ")";
            output += field.Name + " = ";
            output += field.GetValue(msg) + ";\n";
        }
        //output += "\n";
        return output;
    }
}