using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APISetLabelsController : MonoBehaviour {
    void Start() {
        EventCoordinator.StartListening(EventName.UI.LabelMaskChanged(), OnMaskChanged);
    }

    void OnMaskChanged(GameMessage msg) {
        ApiCoordinator.SetLabelColors();
    }
}