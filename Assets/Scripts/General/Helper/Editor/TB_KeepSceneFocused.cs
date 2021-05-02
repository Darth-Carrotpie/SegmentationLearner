#if TRUE && UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
internal static class TB_KeepSceneFocused {
    private const bool forceFocusSceneOnPlay = true;
    private static SceneView sceneWindow;
    private static EditorWindow gameWindow;
    private static bool sceneNeedFocus;
    private static bool oneFrameSkipped;
    private static System.Action focusFunc;
    static TB_KeepSceneFocused() {
        FindSceneWindow();
        FindGameWindow();
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private static void FindSceneWindow() {
        if (sceneWindow != null)return;
        var sceneWindows = Resources.FindObjectsOfTypeAll<SceneView>();
        if (sceneWindows != null && sceneWindows.Length > 0)sceneWindow = sceneWindows[0];
    }
    private static readonly System.Type gameWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.PlayModeView");
    private static void FindGameWindow() {
        if (gameWindow != null)return;
        var gameWindows = Resources.FindObjectsOfTypeAll(gameWindowType);
        if (gameWindows != null && gameWindows.Length > 0)gameWindow = (EditorWindow)gameWindows[0];
    }
    private static void StoreSceneNeedFocus() {
        FindSceneWindow();
        if (sceneWindow != null)sceneNeedFocus = sceneWindow.hasFocus;
        else sceneNeedFocus = false;
    }
    private static void OnPlayModeStateChanged(PlayModeStateChange stateChange) {
        switch (stateChange) {
            case PlayModeStateChange.ExitingEditMode:
                StoreSceneNeedFocus();
                break;
            case PlayModeStateChange.EnteredPlayMode:
                EditorApplication.pauseStateChanged += OnPauseStateChanged;
                if (EditorSettings.enterPlayModeOptionsEnabled &&
                    EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload)) {
                    if (sceneNeedFocus)FocusSceneWindow();
                } else {
                    if (forceFocusSceneOnPlay)FocusSceneWindow();
                }
                break;
            case PlayModeStateChange.ExitingPlayMode:
                StoreSceneNeedFocus();
                EditorApplication.pauseStateChanged -= OnPauseStateChanged;
                break;
            case PlayModeStateChange.EnteredEditMode:
                FindSceneWindow();
                if (sceneWindow != null) {
                    if (sceneNeedFocus) {
                        if (!sceneWindow.hasFocus)FocusSceneWindow();
                    } else {
                        FindGameWindow();
                        if (gameWindow != null)FocusGameWindow();
                    }

                }
                break;
        }
    }
    private static void OnPauseStateChanged(PauseState pauseState) {
        switch (pauseState) {
            case PauseState.Paused:
                StoreSceneNeedFocus();
                if (!sceneNeedFocus) {
                    FindGameWindow();
                    if (gameWindow != null && gameWindow.hasFocus)FocusOnUpdate(FocusGameWindow);
                } else FocusOnUpdate(FocusSceneWindow);
                break;
            case PauseState.Unpaused:
                if (sceneNeedFocus)FocusOnUpdate(FocusSceneWindow);
                break;
        }
    }
    private static void FocusOnUpdate(System.Action focusFunc) {
        TB_KeepSceneFocused.focusFunc = focusFunc;
        oneFrameSkipped = false;
        EditorApplication.update += OnUpdateFocusFunc;
    }
    private static void OnUpdateFocusFunc() {
        if (oneFrameSkipped) {
            EditorApplication.update -= OnUpdateFocusFunc;
            focusFunc();
        }
        oneFrameSkipped = true;
    }
    private static void FocusSceneWindow() {
        FindSceneWindow();
        if (sceneWindow != null)sceneWindow.Focus();
    }
    private static void FocusGameWindow() {
        FindGameWindow();
        if (gameWindow != null)gameWindow.Focus();
    }
}
#endif