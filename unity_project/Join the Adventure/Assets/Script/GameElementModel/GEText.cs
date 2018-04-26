using System;
using System.Collections;
using System.Collections.Generic;

public class GEText :GameElement {

    private string defLang;
    private Dictionary<string, string> textByLang;

    public GEText(string id, string defLang) : base(id)
    {
        this.defLang = defLang;
        this.textByLang = new Dictionary<string, string>();
    }

    public GEText(string id, string defLang, string lang, string txt): this(id, defLang)
    {
        AddText(lang,txt);
    }
    public void AddText(string lang, string text)
    {
        if (!textByLang.ContainsKey(lang))
        {
            textByLang.Add(lang, text);
        } else
        {
            textByLang[lang] = text;
            Console.WriteLine(String.Format("There are multiple text elements defined with the same lang! TextID: {0}, lang: {1}", Id, lang));
        }
    }

    public string GetText(string lang)
    {
        string ret = null;
        textByLang.TryGetValue(lang, out ret);
        if (ret != null)
        {
            return ret;
        }
        if (!lang.Equals(defLang))
        {
            ret = textByLang[defLang];
            if (ret != null)
            {
                return ret;
            }
        }
        throw new Exception(String.Format("There is no text with the given id and lang or default lang! id: {0}, lang: {1}, deflang: {2}", Id, lang, defLang));
    }

    public string GetText()
    {
        return GetText(defLang);
    }

    public string DefLang
    {
        get
        {
            return defLang;
        }
    }
}
