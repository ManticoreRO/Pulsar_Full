using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Interactibles/Unhide interactible")]
public class UnhideInteractibleReaction : Reaction
{
    public bool showFindingMessage = true;
    public Interactible interactible;

    public override void ExecuteReaction(GameController controller)
    {
        interactible.isFound = true;
        //if (showFindingMessage) controller.AddTextWithReturn("\n  You have found something! " + interactible.properties.descriptionWhenFound, Color.green);
        controller.areaManager.InitDiscoveredInteractibles();
        //controller.InspectCommand();
    }
}
