using System.Collections;
using System.Collections.Generic;

public class MIConversation : MenuItem
{

    private GENpc npc;
    private MenuItemBundle bundleInstance;
    private GENpc.GEConversation actualConv;

    public MIConversation(string menuText, MenuItemBundle parentBundle, GENpc npc) : base(menuText, parentBundle)
    {
        this.npc = npc;
    }

    protected override void Execute()
    {
        Description descriptionPanel = Description.GetInstance();
        descriptionPanel.ClearDescription();
        if (bundleInstance == null || (actualConv == null || actualConv != npc.ActiveConversation))
        {
            if(actualConv != null)
                actualConv.OnActivationChange -= parentBundle.RefreshOnEvent;
            actualConv = npc.ActiveConversation;
            bundleInstance = MenuItemBundleFactroy.CreateConversationLineBundle(LabelUtility.Instance.GetLabel(LabelNames.CONVERSATIONWITH) + npc.NameText.GetText(), parentBundle, actualConv.FirstLine, npc.NameText.GetText());
            
            actualConv.OnActivationChange += parentBundle.RefreshOnEvent;
        }
        bundleInstance.ExecuteSideEffects();
        menuController.CurrentBundle = bundleInstance;
    }

    public override void SetActive(bool value)
    {
        if (value)
        {
            base.SetActive(npc.ActiveConversation != null);
        }
        else
        {
            base.SetActive(value);
        }
    }
}
