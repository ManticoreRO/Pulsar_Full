using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Add/Reactions/Areas/Force travel"))]
public class ForceTravelReaction : Reaction
{
    public Area toArea;

    public override void ExecuteReaction(GameController controller)
    {
        GameController.controller.areaManager.ChangeArea(toArea);
        //GameController.controller.ReturnToMainWindowCommand();
    }
}
