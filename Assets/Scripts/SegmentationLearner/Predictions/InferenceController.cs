using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class InferenceController : MonoBehaviour {

    bool runInference = false;
    int skipFrame = 60;

    public void Start(){
        EventCoordinator.StartListening(EventName.System.StartInference(), OnInfStart);
        EventCoordinator.StartListening(EventName.System.StopInference(), OnInfStop);
    }
    void OnInfStart(GameMessage message){
        runInference = true;
        LabelMaskController.IsolateLabelMaskGroup(0);
        OverlayColorController.Show(true);
        StartCoroutine(RunInference());
    }
    void OnInfStop(GameMessage message){
        runInference = false;
        LabelTextFactory.HideAllTexts();
    }


    IEnumerator RunInference() {
        int frameCout = 0;
        while(runInference){
            frameCout++;
            if(frameCout > skipFrame){
                frameCout = 0;
                ApiCoordinator.Predict(CameraCapture.CaptureScreenshot());
            }
            yield return null;
        }
    }
}