using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIBack : MenuItem
{

    public MIBack(MenuItemBundle parentBundle) : base(LabelUtility.Instance.GetLabel(LabelNames.BACK), parentBundle)
    {
    }

    protected override void Execute()
    {
        menuController.CurrentBundle = parentBundle;
    }

    public void ExecuteBack()
    {
        Execute();
    }
}
