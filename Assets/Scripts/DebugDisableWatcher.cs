using UnityEngine;
using System;

public class DebugDisableWatcher : MonoBehaviour
{
    void OnDisable()
    {
        Debug.LogWarning($"{name} was DISABLED! Full Stack Trace:\n" + new System.Diagnostics.StackTrace(true));
    }
}
