using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptorReader : IDescriptorReader
{
    public GameDescriptor ReadDescriptor(string path)
    {
        return GameDescriptor.LoadFromFile(path);
    }

    public GameDescriptor[] ReadMultipleDescriptor(string[] paths)
    {
        GameDescriptor[] ret = new GameDescriptor[paths.Length];
        int i = 0;
        foreach(string path in paths)
        {
            ret[i++] = ReadDescriptor(path);
        }
        return ret;
    }
}
