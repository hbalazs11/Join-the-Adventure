using System;
using System.Collections;
using System.Collections.Generic;

public class GameElementManager : IGameElementManager {

    private ILogger logger;

    private string currentLang;
    private string defLang;

    private GEPlayer player;
    private GEGameProperties gameProperties;

    private Dictionary<string, GERoom> rooms;

    private Dictionary<string, GEText> texts;

    private Dictionary<string, GEProperty> properties;

    private Dictionary<string, GEItem> items;

    private Dictionary<string, GEMenuItem> menuItems;

    private Dictionary<string, GENpc> npcs;

    private Dictionary<string, GEGameEnd> gameEnds;

    public GERoom CurrentRoom { get; set; }

    public GameElementManager()
    {
        rooms = new Dictionary<string, GERoom>();
        texts = new Dictionary<string, GEText>();
        properties = new Dictionary<string, GEProperty>();
        items = new Dictionary<string, GEItem>();
        menuItems = new Dictionary<string, GEMenuItem>();
        npcs = new Dictionary<string, GENpc>();
        gameEnds = new Dictionary<string, GEGameEnd>();
        logger = Injector.Logger;
    }

    public void SetFirstRoom()
    {
        CurrentRoom = rooms[gameProperties.firstRoomId];
    }

    public GEText GetTextElement(string id, bool log = true)
    {
        if(id == null)
        {
            return null;
        }
        return GetFromDic(texts, id, log);
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
        if (text == null)
        {
            if (log)
            {
                logger.LogWarn("There is no GEText with the given id! " + id);
                //throw new Exception(String.Format("There is no text with the given id! id: {0}", id));
            }
            return null;
        }
        return text.GetText(lang, log);
    }

    public GEText AddText(string id, string txt, string lang)
    {
        GEText text = GetTextElement(id, false);
        if (text == null)
        {
            text = new GEText(id, defLang, lang, txt);
            AddTextElement(text);
        } else
        {
            text.AddText(lang, txt);
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
            logger.LogWarn("There are multiple text elements defined with the same id! The duplicated id: " + id);
        }
    }

    private T GetFromDic<T>(Dictionary<string, T> dictionary, string key, bool logNullWarn = true) where T : GameElement
    {
        T value = null;
        dictionary.TryGetValue(key, out value);
        if(logNullWarn && value == null)
        {
            logger.LogWarn("There is no content with the given id in the GameElementManager! id: " + key);
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
