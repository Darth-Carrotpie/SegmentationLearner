#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
public class EnumFactory {
    public static string enumsPath = "Assets/Scripts/Enums/";
    public static void Generate(string enumName, string[] enumEntries) {
        var folder = Directory.CreateDirectory(EnumFactory.enumsPath); // returns a DirectoryInfo object
        string filePathAndName = "Assets/Scripts/Enums/" + enumName + ".cs"; //The folder Scripts/Enums/ is expected to exist

        using(StreamWriter streamWriter = new StreamWriter(filePathAndName)) {
            //streamWriter.WriteLine("[System.Flags]");
            streamWriter.WriteLine("public enum " + enumName + " : System.Int64 ");
            streamWriter.WriteLine("{");
            for (int i = 0; i < enumEntries.Length; i++) {
                streamWriter.WriteLine("\t" + enumEntries[i] + " = 1 << " + i + ",");
            }
            streamWriter.WriteLine("}");
        }
        AssetDatabase.Refresh();
    }

    public static System.Type GetEnum(string enumName) {
        Type t = Type.GetType(enumName);
        return t;
    }
}
#endif