using UnityEngine;
public class NameListAttribute : PropertyAttribute {
    public System.Type propType;

    public string[] FullList {
        get;
        private set;
    }

    public NameListAttribute(System.Type aType, string methodName) {
        var method = aType.GetMethod(methodName);
        if (method != null) {
            FullList = method.Invoke(null, null)as string[];
        } else {
            Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + aType);
        }
        propType = aType;
    }

}