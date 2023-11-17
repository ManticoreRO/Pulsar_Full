using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Visuals/Show Message")]
public class ShowMessageReaction : Reaction
{
    [TextArea]
    public string messageToShow = "";
    public bool returnToLogWindow = false;

    public override void ExecuteReaction(GameController controller)
    {
        GameController.controller.AddTextWithReturn(messageToShow, Color.yellow);
        if (returnToLogWindow)
            GameController.controller.ReturnToMainWindowCommand();
    }
}
