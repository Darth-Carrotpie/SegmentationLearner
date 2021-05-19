using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ModelApiCoordinator : Singleton<ModelApiCoordinator> {
    public static void Predict(byte[] imageBytes) {
        Instance.StartCoroutine(Instance.GetPrediction(imageBytes));
    }
    IEnumerator GetTestPrediction(byte[] imageBytes) {
        string encodedText = Convert.ToBase64String(imageBytes);

        string url = "http://127.0.0.1:4200/t"; //predict_image";
        DataClass dataClass = new DataClass();
        dataClass.mime = "mimey";
        dataClass.image64 = Convert.ToBase64String(imageBytes);
        //dataClass.imageBytes = imageBytes;

        string json = JsonUtility.ToJson(dataClass);
        Debug.Log("send json: " + json);
        using(UnityWebRequest request = UnityWebRequest.Put(url, json)) {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.Log(request.error);
            } else {
                // Show results as text
                Debug.Log(request);
                Debug.Log(request.downloadHandler.text);

                // Or retrieve results as binary data
                //byte[] results = request.downloadHandler.data;

                //byte[] decodedBytes = Convert.FromBase64String (results);
                //string decodedText = Encoding.UTF8.GetString(results);

                //byte[] result = request.downloadHandler.data;
                string resultJson = System.Text.Encoding.Default.GetString(request.downloadHandler.data);
                Debug.Log(resultJson);
                DataClass info = JsonUtility.FromJson<DataClass>(resultJson);
                Debug.Log("mime:" + info.mime);
                //OverlayCoordinator.RenderToPanel(results);
            }
        }
    }
    IEnumerator GetPrediction(byte[] imageBytes) {
        string encodedText = Convert.ToBase64String(imageBytes);

        string url = "http://127.0.0.1:4200/predict_image_text"; //predict_image";
        DataClass dataClass = new DataClass();
        dataClass.mime = "mimey";
        dataClass.image64 = Convert.ToBase64String(imageBytes);
        string json = JsonUtility.ToJson(dataClass);

        using(UnityWebRequest request = UnityWebRequest.Put(url, json)) {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.Log(request.error);
                Debug.Log("Have you started the API server?");
            } else {
                //byte[] decodedBytes = Convert.FromBase64String (results);
                //string decodedText = Encoding.UTF8.GetString(results);

                string resultJson = System.Text.Encoding.Default.GetString(request.downloadHandler.data);
                DataClass info = JsonUtility.FromJson<DataClass>(resultJson);
                Debug.Log("mime:" + info.mime);
                Debug.Log("image64:" + info.image64);
                Debug.Log("labels:" + info.labels);
                Debug.Log("confidences:" + info.confidences);
                byte[] decodedBytes = Convert.FromBase64String(info.image64);
                OverlayCoordinator.RenderToPanel(decodedBytes);
            }
        }
    }
}

[Serializable]
public class DataClass {
    public string mime;
    //public byte[] imageBytes;
    public string image64;
    public int[] labels;
    public float[] confidences;
}