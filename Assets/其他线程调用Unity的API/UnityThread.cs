#define ENABLE_UPDATE_FUNCTION_CALLBACK
//#define ENABLE_LATEUPDATE_FUNCTION_CALLBACK       /* 不需要lateupdate和fixedupdate  注释提高性能 */
//#define ENABLE_FIXEDUPDATE_FUNCTION_CALLBACK
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   其他线程调用UnityAPI
/// </summary>
public class UnityThread : MonoBehaviour
{
    //our (singleton) instance
    private static UnityThread instance;

    //Holds actions received from another Thread. Will be coped to actionCopiedQueueUpdateFunc then executed from there
    private static readonly List<Action> actionQueuesUpdateFunc = new List<Action>();
    private static List<Transform> parameterList = new List<Transform>();

// Used to know if whe have new Action function to execute. This prevents the use of the lock keyword every frame
    private static volatile bool noActionQueueToExecuteUpdateFunc = true;

    private static readonly List<Action> actionQueuesLateUpdateFunc = new List<Action>();
    private static volatile bool noActionQueueToExecuteLateUpdateFunc = true;
    private static readonly List<Action> actionQueuesFixedUpdateFunc = new List<Action>();
    private static volatile bool noActionQueueToExecuteFixedUpdateFunc = true;
    private readonly List<Action> actionCopiedQueueFixedUpdateFunc = new List<Action>();

    private readonly List<Action> actionCopiedQueueLateUpdateFunc = new List<Action>();

    //holds Actions copied from actionQueuesUpdateFunc to be executed
    private readonly List<Action> actionCopiedQueueUpdateFunc = new List<Action>();

    public static void InitUnityThread(bool visible = false)
    {
        if (instance != null) return;
        if (Application.isPlaying)
        {
            // add an invisible game object to the scene
            var obj = new GameObject("MainThreadExecuter");
            if (!visible) obj.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<UnityThread>();
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnDisable()
    {
        if (instance == this) instance = null;
    }

#if (ENABLE_UPDATE_FUNCTION_CALLBACK)
    public static void executeCoroutine(IEnumerator action)
    {
        if (instance != null) executeInUpdate(() => instance.StartCoroutine(action));
    }

    public static void executeInUpdate(Action action)
    {
        if (action == null) throw new ArgumentNullException("action");
        lock (actionQueuesUpdateFunc)
        {
            actionQueuesUpdateFunc.Add(action);
            noActionQueueToExecuteUpdateFunc = false;
        }
    }

    public void Update()
    {
        if (noActionQueueToExecuteUpdateFunc) return;
        actionCopiedQueueUpdateFunc.Clear();
        lock (actionQueuesUpdateFunc)
        {
            actionCopiedQueueUpdateFunc.AddRange(actionQueuesUpdateFunc);
            actionQueuesUpdateFunc.Clear();
            noActionQueueToExecuteUpdateFunc = true;
        }

        for (var i = 0; i < actionCopiedQueueUpdateFunc.Count; i++) actionCopiedQueueUpdateFunc[i].Invoke();
    }
#endif
#if (ENABLE_LATEUPDATE_FUNCTION_CALLBACK)
    public static void executeInLateUpdate(Action action)
    {
        if (action == null) throw new ArgumentNullException("action");
        lock (actionQueuesLateUpdateFunc)
        {
            actionQueuesLateUpdateFunc.Add(action);
            noActionQueueToExecuteLateUpdateFunc = false;
        }
    }

    public void LateUpdate()
    {
        if (noActionQueueToExecuteLateUpdateFunc) return;
        actionCopiedQueueLateUpdateFunc.Clear();
        lock (actionQueuesLateUpdateFunc)
        {
            actionCopiedQueueLateUpdateFunc.AddRange(actionQueuesLateUpdateFunc);
            actionQueuesLateUpdateFunc.Clear();
            noActionQueueToExecuteLateUpdateFunc = true;
        }

        for (var i = 0; i < actionCopiedQueueLateUpdateFunc.Count; i++) actionCopiedQueueLateUpdateFunc[i].Invoke();
    }
#endif
#if (ENABLE_FIXEDUPDATE_FUNCTION_CALLBACK)
    public static void executeInFixedUpdate(Action action)
    {
        if (action == null) throw new ArgumentNullException("action");
        lock (actionQueuesFixedUpdateFunc)
        {
            actionQueuesFixedUpdateFunc.Add(action);
            noActionQueueToExecuteFixedUpdateFunc = false;
        }
    }

    public void FixedUpdate()
    {
        if (noActionQueueToExecuteFixedUpdateFunc) return;
        actionCopiedQueueFixedUpdateFunc.Clear();
        lock (actionQueuesFixedUpdateFunc)
        {
            actionCopiedQueueFixedUpdateFunc.AddRange(actionQueuesFixedUpdateFunc);
            actionQueuesFixedUpdateFunc.Clear();
            noActionQueueToExecuteFixedUpdateFunc = true;
        }

        for (var i = 0; i < actionCopiedQueueFixedUpdateFunc.Count; i++) actionCopiedQueueFixedUpdateFunc[i].Invoke();
    }
#endif
}