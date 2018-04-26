using System.Collections;
using System.Collections.Generic;

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

    public GEProperty(string id, GEText name, double defValue, double minValue, double maxValue) : this(id, name, defValue) 
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
    }

    public double Value
    {
        get
        {
            return value;
        }

        set
        {
            if (minValue != null && value < minValue) return;
            if (maxValue != null && value > maxValue) return;
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
}
