using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add/Reactions/Items/Give Items")]
public class GiveItemsReaction : Reaction
{
    public List<Item> items;

    public override void ExecuteReaction(GameController controller)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].isFound = true;
            items[i].visibleWhenUnlit = true;
            //controller.AddTextWithReturn("  You have found something! " + items[i].itemDescriptionWhenFound, Color.green);
            controller.areaManager.InitDiscoveredItems();
            //GameController.controller.PickUpCommand();
        }
    }
}
