using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LabelUtility  {

    private const string DEF_LABELINI_PATH = "Assets/Resources/labels.ini";

    private GameElementManager elementManager;
    private ILogger logger;
    private Dictionary<string, string> data;
    private bool isStarted = false;

    private static LabelUtility instance;

    private LabelUtility()
    {
        elementManager = Injector.GameElementManager;
        logger = Injector.Logger;
    }

    public static LabelUtility Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LabelUtility();
            }
            return instance;
        }
    }

    private void Start(string path)
    {
        data = new Dictionary<string, string>();
        foreach (var row in File.ReadAllLines(path))
            data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
        isStarted = true;
    }

    private void Start()
    {
        Start(DEF_LABELINI_PATH);
    }

    public string GetLabel(string key)
    {
        if (!isStarted)
        {
            Start();
        }
        string res = elementManager.GetText(key);
        if (res != null) return res;
        res = data[key];
        if (res != null) return res;
        logger.LogError("There is no defined label with the given key! " + key);
        return "";
    }


}
