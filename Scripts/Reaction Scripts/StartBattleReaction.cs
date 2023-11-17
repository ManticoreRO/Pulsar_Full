using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add/Reactions/Misc/Start battle")]
public class StartBattleReaction : Reaction
{
    public Enemy enemy;
    public bool activateEnemy;
    public bool startBattleNow = true;

    public override void ExecuteReaction(GameController controller)
    {
        if (!enemy.isDestroyed)
        {
            enemy.isActivated = activateEnemy;
            if (startBattleNow) GameController.controller.BattleScreenCommand(enemy);
        }
    }
}
