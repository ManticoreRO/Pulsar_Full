using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Item Combination")]
public class ItemCombination : ScriptableObject
{
    public List<Item> ingredients = new List<Item>();

    public Item result;

    public Reaction reactionOnCombine;

    public bool Combine()
    {
        List<Item> selecteditems = GameController.controller.selectedItemsToCombine;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (!selecteditems.Contains(ingredients[i]) || ingredients[i].usesLeft == 0) return false;
        }

        // if we are here, then we can combine
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].usesLeft > 0) ingredients[i].usesLeft -= 1;

            if (ingredients[i].usesLeft == 0 && ingredients[i].destroyOnUsed)
            {
                ingredients[i].RemoveItem();
            }
        }

        // we add the new item to the inventory
        // if any of the parts are already in the inventory, we delete them
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].isInInventory && ingredients[i] == result)
            {
                GameController.controller.areaManager.inventory.Remove(ingredients[i]);
            }
        }
        GameController.controller.areaManager.inventory.Add(result);
        result.isInInventory = true;

        // we execute the reaction if not null
        if (reactionOnCombine != null) reactionOnCombine.ExecuteReaction(GameController.controller);

        
        return true;
    }
}
