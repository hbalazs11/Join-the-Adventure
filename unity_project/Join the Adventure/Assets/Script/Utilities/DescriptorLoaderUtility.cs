using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DescriptorLoaderUtility  {

	public static IEnumerator LoadTestDescriptor(string fileName, EventHandler<EventArgs> OnFinished, EventHandler<MainMenuController.ExceptionEventArgs> OnError)
    {
        byte[] zipBytes = null;
        string filePath = Application.streamingAssetsPath + "/" + fileName;
        WWW www = new WWW(filePath);
        yield return www;
        zipBytes = www.bytes;
        try
        {
            Dictionary<string, MemoryStream> descFiles = ZipUtility.ExtractZipFile(zipBytes);
            List<string> xmlNames = GetXmlNames(descFiles);
            Dictionary<string, MemoryStream> imgResources = GetImgs(descFiles);
            Injector.DescriptorProcessor.ProcessImageResources(imgResources);
            List<GameDescriptor> gDescriptors = ReadXmls(xmlNames, descFiles);
            Injector.DescriptorProcessor.ProcessMultipleGameDescriptor(gDescriptors);
            OnFinished(null, EventArgs.Empty);
            yield break;
        }
        catch (System.Exception e)
        {
            OnError(null, new MainMenuController.ExceptionEventArgs("Error occoured during descriptor loading!", e));
            yield break;
        }
    }

    public static void LoadDescriptor(string filePath, EventHandler<EventArgs> OnFinished, EventHandler<MainMenuController.ExceptionEventArgs> OnError)
    {
        byte[] zipBytes = null;
        
        zipBytes = File.ReadAllBytes(filePath);
        
        try
        {
            Dictionary<string, MemoryStream> descFiles = ZipUtility.ExtractZipFile(zipBytes);
            List<string> xmlNames = GetXmlNames(descFiles);
            Dictionary<string, MemoryStream> imgResources = GetImgs(descFiles);
            Injector.DescriptorProcessor.ProcessImageResources(imgResources);
            List<GameDescriptor> gDescriptors = ReadXmls(xmlNames, descFiles);
            Injector.DescriptorProcessor.ProcessMultipleGameDescriptor(gDescriptors);
            OnFinished(null, EventArgs.Empty);
        }
        catch (System.Exception e)
        {
            OnError(null, new MainMenuController.ExceptionEventArgs("Error occoured during descriptor loading!", e));
        }
    }

    private static List<GameDescriptor> ReadXmls(List<string> xmlNames, Dictionary<string, MemoryStream> descFiles)
    {
        List<GameDescriptor> ret = new List<GameDescriptor>();
        foreach(string xmlName in xmlNames)
        {
            descFiles[xmlName].Position = 0;
            StreamReader sr = new StreamReader(descFiles[xmlName]);
            string xmlTxt = sr.ReadToEnd();

            //string xmlTxt = Encoding.UTF8.GetString(descFiles[xmlName].ToArray());

            GameDescriptor gd = GameDescriptor.Deserialize(xmlTxt);
            ret.Add(gd);
        }
        return ret;
    }

    private static List<string> GetXmlNames(Dictionary<string, MemoryStream> descriptors)
    {
        List<string> ret = new List<string>();
        foreach (string name in descriptors.Keys)
        {
            if (name.EndsWith(".xml"))
            {
                ret.Add(name);
            }
        }
        return ret;
    }

    private static Dictionary<string, MemoryStream> GetImgs(Dictionary<string, MemoryStream> descriptors)
    {
        Dictionary<string, MemoryStream> ret = new Dictionary<string, MemoryStream>();
        foreach (string name in descriptors.Keys)
        {
            if (name.EndsWith(".jpg") || name.EndsWith(".png"))
            {
                ret.Add(name, descriptors[name]);
            }
        }
        return ret;
    }

}
