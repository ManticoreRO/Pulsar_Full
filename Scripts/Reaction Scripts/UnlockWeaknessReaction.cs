using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Unlock enemy weakness")]
public class UnlockWeaknessReaction : Reaction
{
    public Enemy whichEnemy;
    public int whichWeakness;

    public override void ExecuteReaction(GameController controller)
    {
        if (whichEnemy.isUnlockedWeakness[whichWeakness]) return;

        whichEnemy.isUnlockedWeakness[whichWeakness] = true;
        GameController.controller.AddTextWithReturn("  You have found a weakness for " + whichEnemy.enemyName + "! " + whichEnemy.weaknessText[whichWeakness], Color.green);
    }
}
