using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Add/Reactions/Player vitals/Modify health"))]

public class ModifiyHealthReaction : Reaction
{

    public float value;

    public override void ExecuteReaction(GameController controller)
    {
        GameData.gameData.GetComponent<PlayerHealth>().ModifyHealth(value);
    }
}
