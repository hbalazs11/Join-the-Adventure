using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class GameElementManager : IGameElementManager {

    [NonSerialized]
    public List<string> savedGameNames;

    public static string persistentDataPath;

    public static GameElementManager initialGEM;

    private string currentLang;
    private string defLang;
    public List<string> AvailableLangs { get; set; }

    private GEPlayer player;
    private GEGameProperties gameProperties;

    private Dictionary<string, GERoom> rooms;

    private Dictionary<string, GEText> texts;

    private Dictionary<string, GEProperty> properties;

    private Dictionary<string, GEItem> items;

    private Dictionary<string, GEMenuItem> menuItems;

    private Dictionary<string, GENpc> npcs;

    private Dictionary<string, GEGameEnd> gameEnds;

    private Dictionary<string, GENpc.GEConversation> npcConvs;

    private Dictionary<string, GENpc.GELine> npcConvLines;
    

    public string GameStorageName { get; set; }

    public GERoom CurrentRoom { get; set; }

    public GameElementManager(string gameStorageName)
    {
        GameStorageName = gameStorageName;
        Init();
    }

    private void Init()
    {
        rooms = new Dictionary<string, GERoom>();
        texts = new Dictionary<string, GEText>();
        properties = new Dictionary<string, GEProperty>();
        items = new Dictionary<string, GEItem>();
        menuItems = new Dictionary<string, GEMenuItem>();
        npcs = new Dictionary<string, GENpc>();
        gameEnds = new Dictionary<string, GEGameEnd>();
        npcConvs = new Dictionary<string, GENpc.GEConversation>();
        npcConvLines = new Dictionary<string, GENpc.GELine>();
        AvailableLangs = new List<string>();
        savedGameNames = new List<string>();
    }

    public void SetFirstRoom()
    {
        CurrentRoom = rooms[gameProperties.firstRoomId];
    }

    public static GameElementManager GetInitialGEM()
    {
        if(initialGEM == null)
        {
            initialGEM = PersistanceHelper.GetInitialGEM(Injector.GameElementManager.GameStorageName);
        }
        return initialGEM;
    }

    public GEText GetTextElement(string id, bool log = true)
    {
        if(id == null)
        {
            return GEText.GETextEmpty.Instance;
        }
        GEText ret = GetFromDic(texts, id, log);
        return ret ?? GEText.GETextEmpty.Instance;
    }


    public void AddTextElement(GEText text)
    {
        AddToDic(texts, text);
    }

    public string GetText(string id)
    {
        return GetText(id, currentLang, false);
    }

    public string GetText(string id, string lang, bool log = true)
    {
        GEText text = GetTextElement(id);
        if (text == null || text.IsEmpty())
        {
            if (log)
            {
                Injector.Logger.LogWarn("There is no GEText with the given id! " + id);
                //throw new Exception(String.Format("There is no text with the given id! id: {0}", id));
            }
            return null;
        }
        return text.GetText(lang, log);
    }

    public GEText AddText(string id, string txt, string lang)
    {
        GEText text = GetTextElement(id, false);
        if (text == null || text.IsEmpty())
        {
            text = new GEText(id, defLang, lang, txt);
            AddTextElement(text);
        } else
        {
            text.AddText(lang, txt);
        }
        if (!AvailableLangs.Contains(lang))
        {
            AvailableLangs.Add(lang);
        }
        return text;
    }

    public GEText AddText(string id, string txt)
    {
        return AddText(id, txt, defLang);
    }

    public GEProperty GetProperty(string id)
    {
        return GetFromDic(properties, id);
    }

    public void AddProperty(GEProperty property)
    {
        AddToDic(properties, property);
    }

    public GEItem GetItem(string id)
    {
        return GetFromDic(items, id);
    }

    public void AddItem(GEItem item)
    {
        AddToDic(items, item);
    }

    public GERoom GetRoom(string id)
    {
        return GetFromDic(rooms, id);
    }

    public void AddRoom(GERoom room)
    {
        AddToDic(rooms, room);
    }

    public GEMenuItem GetMenuItem(string id)
    {
        return GetFromDic(menuItems, id);
    }

    public void AddMenuItem(GEMenuItem menuItem)
    {
        AddToDic(menuItems, menuItem);
    }

    public GENpc GetNPC(string id)
    {
        return GetFromDic(npcs, id);
    }

    public void AddNPC(GENpc npc)
    {
        AddToDic(npcs, npc);
    }

    public GEGameEnd GetGameEnd(string id)
    {
        return GetFromDic(gameEnds, id);
    }

    public void AddGameEnd(GEGameEnd gameEnd)
    {
        AddToDic(gameEnds, gameEnd);
    }

    public GENpc.GEConversation GetNpcConv(string id)
    {
        return GetFromDic(npcConvs, id);
    }

    public void AddNpcConv(GENpc.GEConversation conv)
    {
        AddToDic(npcConvs, conv);
    }

    public GENpc.GELine GetNpcConvLine(string id)
    {
        return GetFromDic(npcConvLines, id);
    }

    public void AddNpcConvLine(GENpc.GELine line)
    {
        AddToDic(npcConvLines, line);
    }

    private void AddToDic<T>(Dictionary<string,T> dictionary, T value) where T : GameElement
    {
        string id = value.Id;
        if (!dictionary.ContainsKey(id))
        {
            dictionary.Add(id, value);
        }
        else
        {
            dictionary[id] = value;
            Injector.Logger.LogWarn("There are multiple text elements defined with the same id! The duplicated id: " + id);
        }
    }

    private T GetFromDic<T>(Dictionary<string, T> dictionary, string key, bool logNullWarn = true) where T : GameElement
    {
        T value = null;
        dictionary.TryGetValue(key, out value);
        if(logNullWarn && value == null)
        {
            Injector.Logger.LogWarn("There is no content with the given id in the GameElementManager! id: " + key);
        }
        return value;
    }
    

    public IActivatable GetActivatableGameElement(string id)
    {
        //menuitem, item, npc, exit, neightbour
        
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        if (menuItems.ContainsKey(id))
        {
            return menuItems[id];
        }
        if (gameEnds.ContainsKey(id))
        {
            return gameEnds[id];
        }
        if (npcs.ContainsKey(id))
        {
            return npcs[id];
        }
        if (npcConvs.ContainsKey(id))
        {
            return npcConvs[id];
        }
        //...
        return null;
    }

    public GameElement GetGameElement(string id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        if (menuItems.ContainsKey(id))
        {
            return menuItems[id];
        }
        if (rooms.ContainsKey(id))
        {
            return rooms[id];
        }
        if (texts.ContainsKey(id))
        {
            return texts[id];
        }
        if (properties.ContainsKey(id))
        {
            return properties[id];
        }
        if (npcs.ContainsKey(id))
        {
            return npcs[id];
        }
        if (npcConvs.ContainsKey(id))
        {
            return npcConvs[id];
        }
        if (npcConvLines.ContainsKey(id))
        {
            return npcConvLines[id];
        }
        if (gameEnds.ContainsKey(id))
        {
            return gameEnds[id];
        }
        return null;
    }

    public GEPlayer Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    internal GEGameProperties GameProperties
    {
        get
        {
            return gameProperties;
        }

        set
        {
            gameProperties = value;
        }
    }

    public string DefLang
    {
        get
        {
            return defLang;
        }

        set
        {
            defLang = value;
            if(currentLang == null)
            {
                currentLang = defLang;
            }
        }
    }

    public string CurrentLang
    {
        get
        {
            return currentLang;
        }

        set
        {
            currentLang = value;
        }
    }
}
