using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Time/Add time")]
public class AddTimeReaction : Reaction {

    public int ticksToAdd;

    public override void ExecuteReaction(GameController controller)
    {
        GameController.controller.AddTime(ticksToAdd);
    }
}
