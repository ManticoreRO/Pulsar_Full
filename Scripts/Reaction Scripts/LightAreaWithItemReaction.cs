using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Areas/Light area with item")]
public class LightAreaWithItemReaction : Reaction
{
    public Area areaToLight;

    public override void ExecuteReaction(GameController controller)
    {
        areaToLight.isLitByItem = true;
        controller.AddTextWithReturn("\n  You can see better now, as the place is lit!\n" + areaToLight.whenLit, Color.cyan);
    }
}
