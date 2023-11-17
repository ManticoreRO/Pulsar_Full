using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Player vitals/Modify energy")]
public class ModifyEnergyReaction : Reaction
{
    public float energyValue = 0f;

    public override void ExecuteReaction(GameController controller)
    {
        GameData.gameData.GetComponent<PlayerHealth>().ModifyEnergy(energyValue);
    }
}
