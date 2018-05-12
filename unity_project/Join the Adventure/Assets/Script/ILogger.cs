
using System;

public interface ILogger
{
    void LogWarn(string msg);
    void LogInfo(string msg);
    void LogError(string msg, Exception e);
    void LogError(string msg);
}

