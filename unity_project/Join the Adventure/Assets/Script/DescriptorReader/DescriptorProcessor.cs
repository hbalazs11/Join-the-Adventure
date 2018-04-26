using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptorProcessor : IDescriptorProcessor
{
    private GameElementManager elementManager;

    private string defLang;

    public DescriptorProcessor()
    {
        elementManager = Injector.GameElementManager;
    }


    public void ProcessGameDescriptor(GameDescriptor gameDescriptor)
    {
        ProcessTexts(gameDescriptor.Texts); //TODO ellenőrizni hogy null, vagy üres!
    }

    public void ProcessMultipleGameDescriptor(GameDescriptor[] gameDescriptors)
    {
        foreach(GameDescriptor descripor in gameDescriptors)
        {
            ProcessGameDescriptor(descripor);
        }
    }

    private Dictionary<string, GEText> ProcessTexts(TextsType texts)
    {
        Dictionary<string, GEText> processedTexts = new Dictionary<string, GEText>();
        if(texts == null)
        {
            return processedTexts;
        }
        string currentDefLang = texts.defLang ?? defLang;
        foreach(TextsTypeText text in texts.Text)
        {
            processedTexts.Add(text.id, elementManager.AddText(text.id, text.lang ?? currentDefLang, text.Value));
        }
        return processedTexts;
    }

    private Dictionary<string, GEProperty> ProcessProperties(PropertiesType properties)
    {
        Dictionary<string, GEProperty> processedProperties = new Dictionary<string, GEProperty>();
        if(properties == null)
        {
            return processedProperties;
        }
        foreach(PropertiesTypeProperty property in properties.Property)
        {
            GEText propertyName = elementManager.GetTextElement(property.nameTextId);
            GEProperty newProperty = new GEProperty(property.id, propertyName, property.defValue, property.minValue, property.maxValue);
            elementManager.AddProperty(newProperty);
            processedProperties.Add(property.id, newProperty);
        }
        return processedProperties;
    }

    private Dictionary<string, GEItem> ProcessItems(ItemsType items)
    {
        Dictionary<string, GEItem> processedItems = new Dictionary<string, GEItem>();
        if (items == null) return processedItems;
        foreach(ItemsTypeItem item in items.Item)
        {
            Dictionary<string, GEMenuItem> menuItems = ProcessMenuItems(item.MenuItems);
            Dictionary<string, GEProperty> properties = ProcessProperties(item.Properties);
            Dictionary<string, GEText> texts = ProcessTexts(item.Texts);
            GEText itemName = elementManager.GetTextElement(item.nameTextId);
            GEText description = elementManager.GetTextElement(item.descTextId);
            GEItem newItem = new GEItem(item.id, itemName, item.activeAtStart, item.equipable, description)
            {
                MenuItems = menuItems,
                Properties = properties,
                Texts = texts
            };
            elementManager.AddItem(newItem);
        }
        return processedItems;
    }

    private Dictionary<string, GEMenuItem> ProcessMenuItems(MenuItemsType menuItems)
    {
        Dictionary<string, GEMenuItem> processedMenuItems = new Dictionary<string, GEMenuItem>();
        if (menuItems == null) return processedMenuItems;

        //TODO
        return null;
    }
}
