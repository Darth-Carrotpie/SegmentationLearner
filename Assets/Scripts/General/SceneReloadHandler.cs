using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReloadHandler : MonoBehaviour {
    void Start() {
        Application.targetFrameRate = -1;

        if (GameStateView.HasState(GameState.gameReloaded))
            EventCoordinator.TriggerEvent(EventName.System.SceneLoaded(), GameMessage.Write());
    }
}