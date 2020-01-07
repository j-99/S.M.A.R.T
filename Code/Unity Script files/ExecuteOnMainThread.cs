/**************************************************************************************
 
 ** Developed by Team LemonSky **
 ** Hack Your Reality Hackathon 2019 **
 
**************************************************************************************/

//Script to execute a command on the Main Thread

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public class ExecuteOnMainThread : MonoBehaviour
{

    public readonly static ConcurrentQueue<Action> RunOnMainThread = new ConcurrentQueue<Action>();

    void Update()
    {
        if (!RunOnMainThread.IsEmpty)
        {
            while (RunOnMainThread.TryDequeue(out Action action))
            {
                action.Invoke();
            }
        }
    }

}