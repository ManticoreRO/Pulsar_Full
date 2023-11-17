using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add/Reactions/Interactibles/Hide interactible")]
public class HideInteractibleReaction : Reaction
{

    public bool showFindingMessage = true;
    public Interactible interactible;

    public override void ExecuteReaction(GameController controller)
    {
        if (!interactible.isFound) return;

        interactible.isFound = false;
        //if (showFindingMessage) controller.AddTextWithReturn("\n  You have found something! " + interactible.properties.descriptionWhenFound, Color.green);
        controller.areaManager.discoveredInteractibles.Remove(interactible);
        controller.areaManager.InitDiscoveredInteractibles();
        //controller.InspectCommand();
    }

}
