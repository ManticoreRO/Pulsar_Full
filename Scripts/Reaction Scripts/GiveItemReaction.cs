using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Items/Give Item")]
public class GiveItemReaction : Reaction
{
    public Item item;

    public override void ExecuteReaction(GameController controller)
    {
        item.isFound = true;
        item.visibleWhenUnlit = true;
        //controller.AddTextWithReturn("\n  You have found something! " + item.itemDescriptionWhenFound, Color.green);
        controller.areaManager.InitDiscoveredItems();
        //GameController.controller.PickUpCommand();
    }
}
