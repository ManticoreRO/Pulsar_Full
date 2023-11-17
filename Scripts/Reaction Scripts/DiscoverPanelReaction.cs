using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Terminals/Discover terminal")]
public class DiscoverPanelReaction : Reaction
{
    public CommandPanel panel;

    public override void ExecuteReaction(GameController controller)
    {
        panel.isDiscovered = true;
        //controller.AddTextWithReturn("\n  You have found a terminal!", Color.green);
        controller.areaManager.InitDiscoveredPanels();
        //controller.InspectCommand();
    }
}
