using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
public abstract class Singleton<T> : Singleton where T : MonoBehaviour {
    #region  Fields
    [CanBeNull]
    private static T _instance;

    [NotNull]
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object Lock = new object();

    [SerializeField]
    private bool _persistent = false;
    #endregion

    #region  Properties
    [NotNull]
    public static T Instance {
        get {
            //Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Got This Instance Correctly!");

            if (Quitting) {
                //Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                // ReSharper disable once AssignNullToNotNullAttribute
                return null;
            }
            lock(Lock) {
                if (_instance != null)
                    return _instance;
                _instance = (T)FindObjectOfType(typeof(Singleton<T>)); //typeof(Singleton<T>));
                //Debug.Log($"Found an instance of [{nameof(Singleton)}<{typeof(T)}>]");
                if (_instance != null) {
                    //Debug.Log($"Found an instance of [{nameof(_instance)}<{typeof(T)}>]");
                    (_instance as Singleton<T>).OnInit();
                    return _instance;
                }
                //Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {FindObjectsOfType<T>()} were found. The first instance found will be used, and all others will be destroyed.");
                //(_instance as Singleton<T>).OnInit();
                return null;
            }

            //Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found.");
            //_instance = new GameObject($"({nameof(Singleton)}){typeof(T)}").AddComponent<T>();
            //(_instance as Singleton<T>).OnInit();
            return null;
        }
    }
    #endregion

    #region  Methods
    private void Awake() {
        //Debug.Log("Singleton:Awake: " + $"[{nameof(Singleton)}<{typeof(T)}>]");
        //potential problem here, on Build application...
        if (_persistent) {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                DontDestroyOnLoad(gameObject);
#else 
            DontDestroyOnLoad(gameObject);
#endif
        }
        T tempInstance = (T)Instance;
        if (tempInstance != this) {
            //Debug.Log("Destroying: this type: " + this.GetType() + " inst: " + tempInstance.GetType());
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) {
                Destroy(this.gameObject);
            }
#else 
            Destroy(this.gameObject);
#endif
        }
        OnAwake();
    }

    protected virtual void OnAwake() {}
    protected virtual void OnInit() {}
    #endregion
}
public abstract class Singleton : MonoBehaviour {
    #region  Properties
    public static bool Quitting { get; set; }
    #endregion

    #region  Methods
    void OnApplicationQuit() {
        Quitting = true;
    }

    #endregion
}