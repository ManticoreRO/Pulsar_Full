using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add/Reactions/Areas/Light current area")]
public class LightCurrentAreaReaction : Reaction
{
    public override void ExecuteReaction(GameController controller)
    {
        GameController.controller.areaManager.currentArea.isLitByItem = true;
        controller.AddTextWithReturn("  You can see better now, as the place is lit!\n" + GameController.controller.areaManager.currentArea.whenLit, Color.cyan);
    }
}
