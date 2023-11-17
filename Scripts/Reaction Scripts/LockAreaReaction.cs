using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Areas/Lock area")]
public class LockAreaReaction : Reaction
{
    public Area areaToLock;
    public Area motherArea;

    public override void ExecuteReaction(GameController controller)
    {
        // we just hide all the exits and make them secret
        for (int i = 0; i < motherArea.availableExits.Count; i++)
        {
            Exit currentExit = motherArea.availableExits[i];

            if (currentExit.destination == areaToLock)
            {
                currentExit.isDiscovered = false;
                currentExit.isLocked = true;
              
                GameController.controller.areaManager.InitDiscoveredExits();
            }
        }
    }
}
