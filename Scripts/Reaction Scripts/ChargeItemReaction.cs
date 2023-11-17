using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Items/Charge item")]
public class ChargeItemReaction : Reaction
{
    public Item whichItem;
    public int value;

    public override void ExecuteReaction(GameController controller)
    {
        whichItem.AddCharge(value);
    }
}
