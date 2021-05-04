using System.Reflection;
using System;
using UnityEngine;
public static class DebugHelper
{
    //for debuging:
    public static string PrintGameMessage(GameMessage msg){
        string output = "";
        Type type = typeof(GameMessage);
        PropertyInfo[] properties = type.GetProperties();
/*         if (properties.Length > 0)
            output+="Properties: "; */
        foreach (PropertyInfo property in properties)
        {
            object val = property.GetValue(msg, null);
            if (val == null)
                continue;
            output += property.Name + "=";
            output += val + "; ";
        }
        FieldInfo[] fields = type.GetFields();
/*         if (fields.Length > 0)
            output+="Fields: "; */
        foreach (FieldInfo field in fields)
        {
            object val = field.GetValue(msg);
            ///var castedVec3 = val as Vector3;|| (val == Vector3.zero)
            if (val == null)
                continue;
            output += field.Name + "=";
            output += field.GetValue(msg) + "; ";
        }
        return output;
    }
}
