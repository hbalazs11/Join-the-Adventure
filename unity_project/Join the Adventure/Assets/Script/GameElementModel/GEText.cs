using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GEText : GameElement {
    private const string EMPTYTEXT = "##NOPE##";
    private string defLang;
    private SortedList<string, string> textByLang;
    [NonSerialized]
    ILogger logger;

    public static GEText EMPTY = new GEText(string.Empty, string.Empty, string.Empty, EMPTYTEXT);

    public GEText(string id, string defLang) : base(id)
    {
        this.defLang = defLang;
        this.textByLang = new SortedList<string, string>();
        logger = Injector.Logger;
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
            logger.LogWarn(String.Format("There are multiple text elements defined with the same lang! TextID: {0}, lang: {1}", Id, lang));
        }
    }

    public string GetText(string lang, bool log = true)
    {
        string ret = null;
        textByLang.TryGetValue(lang, out ret);
        if (ret != null)
        {
            return CheckNope(ret);
        }
        if (!lang.Equals(defLang))
        {
            textByLang.TryGetValue(defLang, out ret);
            if (ret != null)
            {
                return CheckNope(ret);
            }
        }
        textByLang.TryGetValue(textByLang.Keys[0], out ret);
        if (ret != null)
        {
            if(log)
                logger.LogError(String.Format("There is no text with the given id and selected or default lang! id: {0}, lang: {1}, deflang: {2}", Id, lang, defLang));
            return ret;
        }
        if (log)
        {
            logger.LogError(String.Format("There is no text with the given id and lang or default lang! id: {0}, lang: {1}, deflang: {2}", Id, lang, defLang));
            throw new Exception(String.Format("There is no text with the given id and lang or default lang! id: {0}, lang: {1}, deflang: {2}", Id, lang, defLang));
        }
        return null;
    }

    public virtual string GetText()
    {
        return GetText(Injector.GameElementManager.CurrentLang);
    }

    private string CheckNope(string txt)
    {
        if (EMPTYTEXT.Equals(txt))
        {
            return string.Empty;
        }
        return txt;
    }

    public string DefLang
    {
        get
        {
            return defLang;
        }
    }

    [Serializable]
    public class GETextEmpty : GEText
    {
        private static GETextEmpty instance;

        public static GETextEmpty Instance
        {
            get
            {
                if (instance == null) instance = new GETextEmpty();
                return instance;
            }
        }

        private GETextEmpty() : base(string.Empty, string.Empty)
        {
        }

        public override string GetText()
        {
            return string.Empty;
        }
    }

    public bool IsEmpty()
    {
        return GetText().Equals(string.Empty);
    }
}
