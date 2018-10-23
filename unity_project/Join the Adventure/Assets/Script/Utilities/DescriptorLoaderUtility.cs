using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DescriptorLoaderUtility  {

	public static IEnumerator LoadDescriptor(string path, string name, EventHandler<EventArgs> OnFinished, EventHandler<MainMenuController.ExceptionEventArgs> OnError)
    {
        //string filePath = Path.Combine(path, name).ToString();
        string filePath = path + "/" + name;
        WWW www = new WWW(filePath);
        yield return www;

        try
        {
            Dictionary<string, MemoryStream> descFiles = ZipUtility.ExtractZipFile(www.bytes);
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
