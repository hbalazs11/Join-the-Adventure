using System;
using System.Collections;
using System.Collections.Generic;

public class GameElementManager : IGameElementManager {

    private string currentLang;
    private string defLang;

    private GEPlayer player;

    private Dictionary<string, GERoom> rooms;

    private Dictionary<string, GEText> texts;

    private Dictionary<string, GEProperty> properties;

    private Dictionary<string, GEItem> items;

    private Dictionary<string, GEMenuItem> menuItems;


    public GameElementManager()
    {
        rooms = new Dictionary<string, GERoom>();
        texts = new Dictionary<string, GEText>();
        properties = new Dictionary<string, GEProperty>();
    }
    public GEText GetTextElement(string id)
    {
        return GetFromDic(texts, id);
    }


    public void AddTextElement(GEText text)
    {
        AddToDic(texts, text);
    }

    public string GetText(string id)
    {
        return GetText(id, currentLang);
    }

    public string GetText(string id, string lang)
    {
        GEText text = GetTextElement(id);
        if (text == null)
        {
            Console.WriteLine("There is no GEText with the given id! " + id);
            throw new Exception(String.Format("There is no text with the given id! id: {0}", id));
        }
        return text.GetText(lang);
    }

    public GEText AddText(string id, string txt, string lang)
    {
        GEText text = GetTextElement(id);
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
            Console.WriteLine("There are multiple text elements defined with the same id! " + id);
        }
    }

    private T GetFromDic<T>(Dictionary<string, T> dictionary, string key) where T : GameElement
    {
        T value = null;
        dictionary.TryGetValue(key, out value);
        return value;
    }



}
