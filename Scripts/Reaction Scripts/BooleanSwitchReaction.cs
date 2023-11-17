using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Boolean switches/Boolean switch reaction")]
public class BooleanSwitchReaction : Reaction
{

    public List<BooleanSwitch> valuesToSwitch = new List<BooleanSwitch>();
    public bool specificValueSwitch = false;

    public List<bool> specificValue = new List<bool>();

    public override void ExecuteReaction(GameController controller)
    {
        for (int i = 0; i < valuesToSwitch.Count; i++)
        {
            if (!specificValueSwitch)
            {
                valuesToSwitch[i].runValue = !valuesToSwitch[i].runValue;
            }
            else
            {
                valuesToSwitch[i].runValue = specificValue[i];
            }
        }

    }
}
