using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class LabelTextFactory : Singleton<LabelTextFactory>
{
    public GameObject textPrefab;
    public Dictionary<int, GameObject> labelTexts = new Dictionary<int,GameObject>();
    public List<int> allIds = new List<int>();

    void Start()
    {
        foreach (KeyValuePair<BaseLabel, int> pair in LabelsBucket.GetLabels()) {
            GameObject newText = Instantiate(textPrefab);
            newText.transform.parent = transform;
            newText.GetComponent<RectTransform>().anchoredPosition =  Vector3.zero;
            newText.GetComponent<RectTransform>().offsetMin =  Vector3.zero;
            newText.GetComponent<RectTransform>().offsetMax =  Vector3.zero;

            newText.GetComponent<TextMeshProUGUI>().text = pair.Key.labelName;
            newText.GetComponent<TextMeshProUGUI>().color = pair.Key.encoderColor;
            labelTexts[pair.Value] = newText;
            allIds.Add(pair.Value);
        }
        HideAllTexts();
    }

    public static void HideAllTexts(){
        foreach (KeyValuePair<BaseLabel, int> pair in LabelsBucket.GetLabels()) {
            Instance.HideText(pair.Value);
        }
    }

    public void HideText(int labelId) {
        labelTexts[labelId].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 5000f, 0f);
    }
    public static void SetTextPosition(int labelId, int x, int y) {
        float scaledX = Screen.width / 64f * (x - 32);
        float scaledY = Screen.height / 64f * (32 - y);
        Vector3 newPos = new Vector3(scaledX, scaledY, 0f);
        if(Instance.labelTexts.Keys.Contains(labelId))
            Instance.labelTexts[labelId].GetComponent<RectTransform>().anchoredPosition = newPos;
        else
            Debug.Log("No key in labelTexts dict:"+labelId);
        Debug.Log("Set Positions "+Instance.labelTexts[labelId].GetComponent<TextMeshProUGUI>().text+
                    " scaledX: "+scaledX+ " scaledX: "+scaledY+
                    " x: "+x+ " y: "+y);
    }
    public static void SetPositions(List<LabelResponseClass> labels){
        foreach(LabelResponseClass lClass in labels){
            SetTextPosition(lClass.label, lClass.labelPositionX, lClass.labelPositionY);
        }
        List<int> labelIds = labels.Select(x => x.label).ToList();
        Instance.HideInverted(labelIds);
    }
    void HideInverted(List<int> show){
        List<int> toHide = Instance.allIds.Except(show).ToList();
        //string check = "";
        foreach(int id in toHide){
            //check+= "  "+id;
            HideText(id);
        }
        //Debug.Log(check);
    }
}
