using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

public class ModelApiCoordinator : Singleton<ModelApiCoordinator>
{
    public static void Predict(byte[] imageBytes)
    {
        Instance.StartCoroutine(Instance.GetPrediction(imageBytes));
    }
    IEnumerator GetPrediction(byte[] imageBytes)
    {
        string encodedText = Convert.ToBase64String(imageBytes);

        string url = "http://127.0.0.1:4200/predict_image";
        var wwwForm = new WWWForm();
        wwwForm.AddField("data", encodedText);
        //DataClass dataClass = new DataClass();
        //dataClass.data = encodedText;
        //string postData = JsonUtility.ToJson(dataClass);
        //byte[] bytes = GetBytes(postData);
        wwwForm.AddBinaryData("screenshot.png", imageBytes, "image/png");
        //var request = new UnityWebRequest(url, "POST");
        var request = UnityWebRequest.Post(url, wwwForm);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Show results as text
            Debug.Log(request);
            Debug.Log(request.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = request.downloadHandler.data;

            //byte[] decodedBytes = Convert.FromBase64String (results);
            string decodedText = Encoding.UTF8.GetString(results);

            OverlayCoordinator.RenderToPanel(results);
        }
    }
}
[Serializable]
public class DataClass
{
    public string data;
}
