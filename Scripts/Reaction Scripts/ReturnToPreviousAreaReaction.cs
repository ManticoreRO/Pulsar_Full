using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Areas/Return to previous area")]
public class ReturnToPreviousAreaReaction : Reaction
{
    public override void ExecuteReaction(GameController controller)
    {
        GameController.controller.areaManager.ChangeArea(GameController.controller.areaManager.previousArea);
    }
}
