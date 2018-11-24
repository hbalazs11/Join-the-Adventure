using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class PersistanceHelper
{
    public static string PersistentDataPath { set { StoredGamesPath = value + SEP + STORED_GAMES_DIR; } }
    private static string StoredGamesPath { get; set; }
    private static readonly string SEP = System.IO.Path.DirectorySeparatorChar.ToString();
    private const string STORED_GAMES_DIR = "stored_games";
    private const string InitialGemName = "initial";
    private const string Extension = ".dat";
    private const string ImageStore = "images";
    private const string SavedGamesStore = "saved_games";
    private const string AutoSaveName = "autosave_";
    private const string DateFormat = "yyyyMMdd-HHmmss";

    public static bool CheckGameDirectory(string dirName)
    {
        return Directory.Exists(StoredGamesPath + SEP + dirName);
    }

    public static GameElementManager GetGEM(string dirName, string fileName, bool isSavedGame = false)
    {
        GameElementManager ret = null;
        string filePath;
        if (isSavedGame)
        {
            filePath = BuildPath(StoredGamesPath, dirName, SavedGamesStore, fileName + Extension);
        } else
        {
            filePath = BuildPath(StoredGamesPath, dirName, fileName + Extension);
        }
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
        return GetGEM(dirName, InitialGemName, false);
    }

    public static void StoreGEM(string dirName, string fileName, GameElementManager gem, bool isSavedGame = false)
    {
        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        string filePath;
        if (isSavedGame)
        {
            filePath = BuildPath(StoredGamesPath, dirName, SavedGamesStore, fileName + Extension);
        }
        else
        {
            filePath = BuildPath(StoredGamesPath, dirName, fileName + Extension);
        }
        using (file = File.Open(filePath, FileMode.Create))
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
            using (FileStream streamWriter = File.Create(BuildPath(StoredGamesPath, dirName, ImageStore, fileName)))
            {
                images[fileName].Position = 0;
                StreamCopyTo(images[fileName], streamWriter);
            }
        }
    }

    public static byte[] GetImage(string dirName, string imageName)
    {
        return File.ReadAllBytes(BuildPath(StoredGamesPath, dirName, ImageStore, imageName));
    }

    public static void StoreSavedGameGEM(string dirName, string fileName, GameElementManager gem)
    {
        StoreGEM(dirName, fileName, gem, true);
    }

    public static string StoreAutoSavedGameGEM(string dirName, GameElementManager gem)
    {
        string savedGameName = AutoSaveName + GetCurrentTime();
        StoreGEM(dirName, savedGameName, gem, true);
        ManageAutoSavedGames(dirName);
        return savedGameName;
    }

    public static string StoreSaveStationSavedGameGEM(string dirName, string saveStationId, GameElementManager gem)
    {
        string saveName = saveStationId + "_" + GetCurrentTime();
        StoreGEM(dirName, saveName, gem, true);
        return saveName;
    }

    private static void ManageAutoSavedGames(string dirName)
    {
        List<string> savedGameNames = GetSavedGameNames(dirName, AutoSaveName);
        if (savedGameNames.Count < 5) return;
        savedGameNames.Sort();
        RemoveStoredGEM(dirName, savedGameNames[0], true);
    }

    public static void RemoveStoredGEM(string dirName, string gemName, bool isSavedGame = false)
    {
        string filePath;
        if (isSavedGame)
        {
            filePath = BuildPath(StoredGamesPath, dirName, SavedGamesStore, gemName + Extension);
        }
        else
        {
            filePath = BuildPath(StoredGamesPath, dirName, gemName + Extension);
        }
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

    }

    public static void RemoveStoredGame(string gameName)
    {
        Directory.Delete(BuildPath(StoredGamesPath, gameName), true);
    }

    private static string GetCurrentTime()
    {
        return DateTime.Now.ToString(DateFormat);
    }

    public static List<string> GetSavedGameNames(string dirName, string namePrefix = "")
    {
        string[] files = Directory.GetFiles(BuildPath(StoredGamesPath, dirName, SavedGamesStore), namePrefix + "*" + Extension);
        for (int i = 0; i < files.Length; i++)
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        List<string> ret = new List<string>(files);
        return ret;
    }

    public static List<string> GetStoredGameNames()
    {
        if (!Directory.Exists(StoredGamesPath))
        {
            Directory.CreateDirectory(StoredGamesPath);
        }
        string[] dirs = Directory.GetDirectories(StoredGamesPath);
        for (int i = 0; i < dirs.Length; i++)
            dirs[i] = Path.GetFileName(dirs[i]);
        List<string> ret = new List<string>(dirs);
        return ret;
    }

    public static GameElementManager GetSavedGameGEM(string dirName, string savedGameName)
    {
        return GetGEM(dirName, savedGameName, true);
    }

    public static void CreateStorage(string dirName)
    {
        if (CheckGameDirectory(dirName)) return;
        System.IO.Directory.CreateDirectory(StoredGamesPath);
        System.IO.Directory.CreateDirectory(BuildPath(StoredGamesPath, dirName));
        System.IO.Directory.CreateDirectory(BuildPath(StoredGamesPath, dirName, ImageStore));
        System.IO.Directory.CreateDirectory(BuildPath(StoredGamesPath, dirName, SavedGamesStore));
    }

    private static string BuildPath(params string[] pathParams)
    {
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < pathParams.Length-1; i++)
        {
            sb.Append(pathParams[i]).Append(SEP);
        }
        sb.Append(pathParams[pathParams.Length - 1]);
        return sb.ToString();
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