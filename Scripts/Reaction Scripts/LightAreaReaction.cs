using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Areas/Light Area")]
public class LightAreaReaction : Reaction
{
    public Area areaToLight;

    public override void ExecuteReaction(GameController controller)
    {
        areaToLight.isLit = true;
        controller.AddTextWithReturn("  You can see better now, as the place is lit!" + areaToLight.whenLit, Color.cyan);
    }
}
