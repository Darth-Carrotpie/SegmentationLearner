using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class CountItemsDialog : MonoBehaviour
{
    public TextMeshProUGUI text;
    void OnEnable()
    {
        int total = CountItems("labels/") + CountItems("screenshots/");
        text.text = "Total: "+total+" items...";
    }

    int CountItems(string subfolder){
        string itemPath = Application.dataPath + "/Data~/" + subfolder;
        var picFile = Directory.GetFiles(itemPath);
        return picFile.Length;
    }

}
