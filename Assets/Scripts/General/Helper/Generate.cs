using System;
using UnityEngine;
using System.Reflection;
using System.Linq;
public static class Generate {

    public static string uniqueID()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        int z1 = UnityEngine.Random.Range(0, 1000000);
        int z2 = UnityEngine.Random.Range(0, 1000000);
        string uid = currentEpochTime + ":" + z1 + ":" + z2;
        return uid;
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    public static Action FindMethodAction(Component source, string method_name)
    {
        Action myAction = () => { };
        Type type = source.GetType();
        MethodInfo mi = type.GetMethod(method_name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (mi != null)
        {
            myAction = (Action)Delegate.CreateDelegate(typeof(Action), source, mi);
        }
        return myAction;
    }
    public static string FindComponentVariable(Component source, string var_name)
    {
        Type type = source.GetType();
        string property = "";
        FieldInfo mi = type.GetField(var_name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        /*if (mi == null)
        {
            Debug.Log(var_name + " variable not found. Method: " +source.name);
            return "";

        }
        if (mi.GetIndexParameters().Length != 0)
        {
            for (int x = 0; x < mi.GetIndexParameters().Length; x++)
            {
                property += mi.GetValue(source, new object[] { x });
            }
        }
        else
        {
            property += mi.GetValue(source, null);
        }*/
        if (mi != null)
        {
            property = (string)mi.GetValue(source);
            //Debug.Log(var_name + " variable found. Method: " +source.name);
            //Debug.Log(property);
        }
        return property;
    }
    private static System.Random random = new System.Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
