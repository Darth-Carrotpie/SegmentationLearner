using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    bool inferenceState = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            inferenceState = !inferenceState;
            if (inferenceState)
                EventCoordinator.TriggerEvent(EventName.System.StartInference(), GameMessage.Write());
            else
                EventCoordinator.TriggerEvent(EventName.System.StopInference(), GameMessage.Write());
        }
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            LabelMaskController.IsolateLabelMaskGroup(0);
        }
        if (Input.GetKeyDown(KeyCode.F1)) {
            LabelMaskController.IsolateLabelMaskGroup(1);
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            LabelMaskController.IsolateLabelMaskGroup(2);
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            LabelMaskController.IsolateLabelMaskGroup(3);
        }
        if (Input.GetKeyDown(KeyCode.F4)) {
            LabelMaskController.IsolateLabelMaskGroup(4);
        }
        // Add more F keys if you made more label groups
        //...

        if (Input.GetKeyDown(KeyCode.Q)) {
            HideBackgroundController.ChangeShowState();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            OverlayColorController.ChangeShowState();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            OverlayColorController.DecreaseOpacity();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            OverlayColorController.IncreaseOpacity();
        }
    }
}