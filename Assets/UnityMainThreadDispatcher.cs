using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;

    private Queue<Action> actionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UnityMainThreadDispatcher>();
                if (instance == null)
                {
                    GameObject go = new GameObject("UnityMainThreadDispatcher");
                    instance = go.AddComponent<UnityMainThreadDispatcher>();
                }
            }
            return instance;
        }
    }

    private void Update()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }
}
