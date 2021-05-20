using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelMaskController : Singleton<LabelMaskController> {

    public static void IsolateLabelMaskGroup(int groupId) {
        if (groupId == 0){
            LabelMaskCoordinator.SetAllStates(true);
        } else {
            LabelMaskCoordinator.SetAllStates(false);
            if (groupId == 1)
                Instance.EnableLabelGroup(LabelsBucket.BuildingLabels);
            if (groupId == 2)
                Instance.EnableLabelGroup(LabelsBucket.FurnitureLabels);
            if (groupId == 3)
                Instance.EnableLabelGroup(LabelsBucket.ItemLabels);
            if (groupId == 4)
                Instance.EnableLabelGroup(LabelsBucket.AnimalsLabels);
        }
        EventCoordinator.TriggerEvent(EventName.UI.LabelMaskChanged(), GameMessage.Write());
    }
    void EnableLabelGroup(List<BaseLabel> labelList) {
        foreach (BaseLabel labelName in labelList) {
            LabelMaskCoordinator.SetState(labelName.labelName, true);
        }
    }
}