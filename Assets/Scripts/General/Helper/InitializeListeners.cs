using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeListeners : MonoBehaviour {
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.MoveGameObjectToScene(transform.root.gameObject, SceneManager.GetActiveScene());
        foreach (Component c in GetComponentsInChildren<Component>(true))
        {
            System.Action init = Generate.FindMethodAction(c, "Init");
            string sceneBound = (string)Generate.FindComponentVariable(c, "sceneToInit");
            if (sceneBound == SceneManager.GetActiveScene().name)
                init();
            else
                if (sceneBound == "")
                    init();
        }
    }
}
