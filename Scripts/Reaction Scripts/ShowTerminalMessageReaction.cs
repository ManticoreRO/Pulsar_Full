using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Show terminal message")]
public class ShowTerminalMessageReaction : Reaction
{

    [TextArea]
    public string message;

    public override void ExecuteReaction(GameController controller)
    {
        if (controller.currentlyShownPanel == null || controller.activeWindow != controller.panelWindow) return;

        controller.StopAllCoroutines();
        controller.StartCoroutine(controller.currentlyShownPanel.ShowLog(controller.panelMessageText, message));
 
    }

}
