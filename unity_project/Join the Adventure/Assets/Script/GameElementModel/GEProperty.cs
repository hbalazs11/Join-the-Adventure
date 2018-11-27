using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class GEProperty : GameElement{

    private GEText name;
    private double value;
    private double? minValue = null;
    private double? maxValue = null;
    private double defValue;

    public GEProperty(string id, GEText name) : base(id)
    {
        this.name = name;
        this.defValue = 0f;
        this.value = 0f;
    }

    public GEProperty(string id, GEText name, double defValue) : base(id)
    {
        this.name = name;
        this.defValue = defValue;
        this.value = defValue;
    }

    public GEProperty(string id, GEText name, double defValue, double minValue, double? maxValue) : this(id, name, defValue) 
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public GEText Name
    {
        get
        {
            return name;
        }
        set
        {
            this.name = value;
        }
    }

    public double Value
    {
        get
        {
            return value;
        }

        set
        {
            if (minValue != null && value < minValue)
            {
                this.value = (double)minValue;
                return;
            }
            if (maxValue != null && value > maxValue)
            {
                this.value = (double)maxValue;
                return;
            }
            this.value = value;
        }
    }

    public void IncValue(double amount)
    {
        Value += amount;
    }

    public void DecValue(double amount)
    {
        Value -= amount;
    }

    public double? MinValue
    {
        get
        {
            return minValue;
        }

        set
        {
            minValue = value;
        }
    }

    public double? MaxValue
    {
        get
        {
            return maxValue;
        }

        set
        {
            maxValue = value;
        }
    }

    public double DefValue
    {
        get
        {
            return defValue;
        }

        set
        {
            defValue = value;
        }
    }

    public static List<GEProperty> GetPropertiesWithNames(IList<GEProperty> properties)
    {
        List<GEProperty> ret = new List<GEProperty>();
        if (properties == null) return ret;
        foreach(GEProperty prop in properties)
        {
            if (prop.Name.GetText() != string.Empty)
            {
                ret.Add(prop);
            }
        }
        return ret;
    }

    public static string GetPropertyDescText(IList<GEProperty> properties)
    {
        StringBuilder sb = new StringBuilder();
        foreach(GEProperty prop in properties)
        {
            sb.Append(prop.Name.GetText()).Append(": ").Append(prop.Value).Append("\\n");
        }
        sb.Remove(sb.Length - 2, 2);
        return sb.ToString();
    }
}
