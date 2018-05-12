using System;
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
        if(gameElements == null || gameElements.Count == 0)
        {
            return;
        }
        if (bundleInstance == null)
        {
            if (gameElements[0] is GEItem)
            {
                List<GEItem> items = new List<GEItem>(gameElements as IList<GEItem>);
                bundleInstance = MenuItemBundleFactroy.CreateListBundle(menuText, items, parentBundle);
            }
            else if(gameElements[0] is GENeighbour)
            {
                List<GENeighbour> neighbours = new List<GENeighbour>(gameElements as IList<GENeighbour>);
                bundleInstance = MenuItemBundleFactroy.CreateListBundle(menuText, neighbours, parentBundle);
            }
        }
        menuController.CurrentBundle = bundleInstance;
    }

    private void SetGameElement(IList<T> gameElements)
    {
        this.gameElements = new List<T>(gameElements);
    }
}
