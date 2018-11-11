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
            actualConv = npc.ActiveConversation;
            bundleInstance = MenuItemBundleFactroy.CreateConversationLineBundle(LabelUtility.Instance.GetLabel(LabelNames.CONVERSATIONWITH) + npc.NameText.GetText(), parentBundle, npc.ActiveConversation.FirstLine, npc.NameText.GetText());
        }
        bundleInstance.ExecuteSideEffects();
        menuController.CurrentBundle = bundleInstance;
    }
}
