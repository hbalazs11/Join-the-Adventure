using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PersistanceHelper
{
    public static string PersistentDataPath { get; set; }

    private const string SEP = "/";
    private const string InitialGemName = "initial";
    private const string Extension = ".dat";
    private const string ImageStore = "images";
    private const string SavedGamesStore = "saved_games";

    public static bool CheckDirectory(string dirName)
    {
        return Directory.Exists(PersistentDataPath + SEP + dirName);
    }

    public static GameElementManager GetGEM(string dirName, string fileName)
    {
        GameElementManager ret = null;
        string filePath = BuildPath(PersistentDataPath, dirName, fileName);
        if (File.Exists(filePath))
        {
            using (FileStream file = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                file.Position = 0;
                ret = (GameElementManager)bf.Deserialize(file);
            }
        }
        return ret;
    }

    public static GameElementManager GetInitialGEM(string dirName)
    {
        return GetGEM(dirName, InitialGemName + Extension);
    }

    public static void StoreGEM(string dirName, string fileName, GameElementManager gem)
    {
        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        using (file = File.Open(BuildPath(PersistentDataPath, dirName, fileName + Extension), FileMode.OpenOrCreate))
        {
            bf.Serialize(file, gem);
        }
    }

    public static void StoreInitialGEM(string dirName, GameElementManager gem)
    {
        StoreGEM(dirName, InitialGemName, gem);
    }

    public static void StoreImages(string dirName, Dictionary<string, MemoryStream> images)
    {
        foreach(string fileName in images.Keys)
        {
            using (FileStream streamWriter = File.Create(BuildPath(PersistentDataPath, dirName, ImageStore, fileName)))
            {
                images[fileName].Position = 0;
                StreamCopyTo(images[fileName], streamWriter);
            }
        }
    }

    public static byte[] GetImage(string dirName, string imageName)
    {
        return File.ReadAllBytes(BuildPath(PersistentDataPath, dirName, ImageStore, imageName));
    }

    public static void CreateStorage(string dirName)
    {
        if (CheckDirectory(dirName)) return;
        System.IO.Directory.CreateDirectory(BuildPath(PersistentDataPath, dirName));
        System.IO.Directory.CreateDirectory(BuildPath(PersistentDataPath, dirName, ImageStore));
        System.IO.Directory.CreateDirectory(BuildPath(PersistentDataPath, dirName, SavedGamesStore));
    }

    private static string BuildPath(params string[] pathParams)
    {
        string ret = "";
        for(int i = 0; i < pathParams.Length-1; i++)
        {
            ret = ret + pathParams[i] + SEP;
        }
        ret = ret + pathParams[pathParams.Length-1];
        return ret;
    }

    public static void StreamCopyTo(this Stream source, Stream destination, int bufferSize = 4096)
    {
        byte[] array = new byte[bufferSize];
        int count;
        while ((count = source.Read(array, 0, array.Length)) != 0)
        {
            destination.Write(array, 0, count);
        }
    }
}