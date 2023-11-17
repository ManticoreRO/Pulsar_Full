using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Areas/Unhide Area")]
public class UnhideAreaReaction : Reaction
{
    public Area initialArea;
    public Area area;

    public override void ExecuteReaction(GameController controller)
    {
        for (int i = 0; i < initialArea.availableExits.Count; i++)
        {
            if (initialArea.availableExits[i].destination == area)
            {
                initialArea.availableExits[i].isDiscovered = true;
            }
        }
        controller.areaManager.InitDiscoveredExits();
        controller.areaManager.CheckIfFullyExplored();
        GameController.controller.AddTextWithReturn("  You discovered a new place to travel!", Color.green);
        GameController.controller.areaManager.discoveredSecretArea = true;
    }
}
