using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Unlock Terminal log")]
public class UnlockTerminalLogReaction : Reaction
{
    public TerminalLog logToUnlock;

    public override void ExecuteReaction(GameController controller)
    {
        logToUnlock.isLocked = false;
        if (controller.selectedPanelToUse != null)
        {
            //if (!controller.selectedPanelToUse.activeLogs.Contains(logToUnlock))
            //{
                controller.selectedPanelToUse.activeLogs.Add(logToUnlock);
                controller.ShowPanelCommand(controller.selectedPanelToUse);             
            //}
            controller.selectedPanelToUse = null;
        }
    }
}
