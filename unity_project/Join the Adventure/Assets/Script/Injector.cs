using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injector  {

    private static GameElementManager gameElementManager;
    private static ILogger logger;

    public static IDescriptorReader DescriptorReader
    {
        get
        {
            return new DescriptorReader();
        }
    }

    public static GameElementManager GameElementManager
    {
        get
        {
            if(gameElementManager == null)
            {
                gameElementManager = new GameElementManager();
            }
            return gameElementManager;
        }
    }

    public static IDescriptorProcessor DescriptorProcessor
    {
        get
        {
            return new DescriptorProcessor();
        }
    }

    public static ILogger Logger
    {
        get
        {
            if (logger == null)
            {
                logger = new Logger();
            }
            return logger;
        }
    }
}
