using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Player vitals/Add resistance")]
public class AddResistanceReaction : Reaction
{
    public float valueAdded;

    public override void ExecuteReaction(GameController controller)
    {
        GameData.gameData.GetComponent<PlayerHealth>().environmentDamageResistance += valueAdded;
    }
}
