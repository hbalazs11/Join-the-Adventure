using System.Collections;
using System.Collections.Generic;

public class MIAnswer : MenuItem
{
    GENpc.GEAnswer geAnswer;
    MenuItemBundle bundleInstance;
    private string answerTo;

    public MIAnswer(string menuText, GENpc.GEAnswer answer, MenuItemBundle parentBundle, string answerTo) : base(menuText, parentBundle)
    {
        this.geAnswer = answer;
        this.answerTo = answerTo;
    }

    protected override void Execute()
    {
        string actionResultText = geAnswer.Execute();
        if(actionResultText != null)
        {
            Description.GetInstance().AddDescriptionText(actionResultText);
        }
        if (bundleInstance == null)
        {
            bundleInstance = MenuItemBundleFactroy.CreateConversationLineBundle(parentBundle.Name, parentBundle, geAnswer.NextLine, answerTo);
        }
        bundleInstance.ExecuteSideEffects();
        menuController.CurrentBundle = bundleInstance;
    }

    public override void SetActive(bool value)
    {
        if (value)
        {
            base.SetActive(geAnswer.IsActive() == true);
        }
        else
        {
            base.SetActive(value);
        }
    }
}
