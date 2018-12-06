using System;
using UnityEngine;

class Logger : ILogger
{
    public void LogError(string msg, Exception e)
    {
        Debug.LogError(msg + "\\n" + e.Message + "\\n" + e.StackTrace);
    }

    public void LogError(string msg)
    {
        Debug.LogError(msg);
    }

    public void LogInfo(string msg)
    {
        Debug.Log(msg);
    }

    public void LogWarn(string msg)
    {
        Debug.LogWarning(msg);
    }
}

