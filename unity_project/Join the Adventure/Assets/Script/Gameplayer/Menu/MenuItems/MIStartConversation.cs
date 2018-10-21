using System.Collections;
using System.Collections.Generic;

public class MIStartConversation : MenuItem
{

    private GENpc npc;

    public MIStartConversation(string menuText, MenuItemBundle parentBundle, GENpc npc) : base(menuText, parentBundle)
    {
        this.npc = npc;
    }

    protected override void Execute()
    {
        throw new System.NotImplementedException();
    }
}
