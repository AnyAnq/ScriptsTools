using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnityThreadTest : MonoBehaviour
{
    private void Awake()
    {
        UnityThread.InitUnityThread();
    }

    void Start()
    {
        Thread thread = new Thread(new ThreadStart(AddEvent));
        thread.Start();
    }

    public void AddEvent()
    {
        UnityThread.executeInUpdate(ChangeValue);
    }
    
    public void ChangeValue()
    {
        transform.position = Vector3.zero;
    }
    
}
