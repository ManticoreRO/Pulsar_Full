using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Add/Reactions/Misc/Random chance"))]
public class RandomChanceReaction : Reaction
{

    public float chanceToHappen = 100f;
    public Reaction reaction;

    public override void ExecuteReaction(GameController controller)
    {
        float roll = Random.Range(1f, 100f);

        if (roll <= chanceToHappen)
            reaction.ExecuteReaction(controller);
    }
}
