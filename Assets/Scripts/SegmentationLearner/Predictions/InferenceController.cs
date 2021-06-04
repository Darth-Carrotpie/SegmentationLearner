using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class InferenceController : MonoBehaviour {

    bool runInference = false;
    float targetDeltaTime = 1f;

    public void Start(){
        EventCoordinator.StartListening(EventName.System.StartInference(), OnInfStart);
        EventCoordinator.StartListening(EventName.System.StopInference(), OnInfStop);
    }
    void OnInfStart(GameMessage message){
        runInference = true;
        LabelMaskController.IsolateLabelMaskGroup(0);
        OverlayColorController.Show(true);
        targetDeltaTime = 1f / ConstantsBucket.TargetFPS;
        StartCoroutine(RunInference());
    }
    void OnInfStop(GameMessage message){
        runInference = false;
        LabelTextFactory.HideAllTexts();
    }


    IEnumerator RunInference() {
        float currDeltaTime = 0;
        while(runInference){
            currDeltaTime+=Time.deltaTime;
            if(currDeltaTime > targetDeltaTime){
                currDeltaTime = 0;
                ApiCoordinator.Predict(CameraCapture.CaptureScreenshot());
            }
            yield return null;
        }
    }
}