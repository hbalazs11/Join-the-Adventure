﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIOpenList<T> : MenuItem where T : GameElement
{
    List<T> gameElements;
    MenuItemBundle bundleInstance;

    public static MIOpenList<T> CreateMIOpenList(string menuText, MenuItemBundle parentBundle, IList<T> gameElements){
        MIOpenList<T> newMI = new MIOpenList<T>(menuText, parentBundle);
        newMI.SetGameElement(gameElements);
        return newMI;
    }

    private MIOpenList(string menuText, MenuItemBundle parentBundle) : base(menuText, parentBundle)
    {
    }

    protected override void Execute()
    {
        if(gameElements == null)
        {
            return;
        }
        if (bundleInstance == null)
        {
            if (typeof(T) == typeof(GEItem))
            {
                List<GEItem> items = new List<GEItem>(gameElements as IList<GEItem>);
                bundleInstance = MenuItemBundleFactroy.CreateListBundle(menuText, items, parentBundle);
            }
            else if (typeof(T) == typeof(GENeighbour))
            {
                List<GENeighbour> neighbours = new List<GENeighbour>(gameElements as IList<GENeighbour>);
                bundleInstance = MenuItemBundleFactroy.CreateListBundle(menuText, neighbours, parentBundle);
            }
            else if (typeof(T) == typeof(GENpc))
            {
                List<GENpc> npcs = new List<GENpc>(gameElements as IList<GENpc>);
                bundleInstance = MenuItemBundleFactroy.CreateListBundle(menuText, npcs, parentBundle);
            }
        }
        menuController.CurrentBundle = bundleInstance;
    }

    private void SetGameElement(IList<T> gameElements)
    {
        this.gameElements = new List<T>(gameElements);
    }
}
