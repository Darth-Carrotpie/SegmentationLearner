using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiCoordinator : Singleton<ApiCoordinator> {

    float start = 0f;
    float received = 0f;
    float end = 0f;

    public static void Predict(byte[] imageBytes) { //predict_image";
        Instance.start = Time.time;
        string json = ApiJsonHandler.GetPrediction(imageBytes);
        string subMethod = "predict_image_text"; //predict_image";
        Instance.StartCoroutine(Instance.SentAPIRequest(json, subMethod));
    }

    public static void SetLabelColors() {
        string json = ApiJsonHandler.LabelColors();
        string subMethod = "set_label_colors";
        Instance.StartCoroutine(Instance.SentAPIRequest(json, subMethod));
    }

    IEnumerator SentAPIRequest(string json, string subMethod) {
        string url = "http://127.0.0.1:4200/" + subMethod;
        using(UnityWebRequest request = UnityWebRequest.Put(url, json)) {
            //Debug.Log("trying to send a json:"+json);

            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Instance.received = Time.time;

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.Log(request.error);
                Debug.Log("Have you started the API server?");
            } else {
                string resultJson = System.Text.Encoding.Default.GetString(request.downloadHandler.data);
                Debug.Log(resultJson);
                DataResponseClass info = JsonUtility.FromJson<DataResponseClass>(resultJson);
                if (info != null) {
                    if (info.image64 != null) {
                        //Debug.Log("info.confidences:"+info.confidences);
                        //Debug.Log(info);
                        byte[] decodedBytes = Convert.FromBase64String(info.image64);
                        OverlayCoordinator.RenderToPanelSingle(decodedBytes);
                        LabelTextFactory.SetPositions(info.labels);
                    }
                }
                Instance.end = Time.time;
            }
        }
        //Debug.Log("Received dif:"+(received - start));
        //Debug.Log("Unity side dif:"+(end - received));
        Debug.Log("Total dif:"+(end - start));
    }
}