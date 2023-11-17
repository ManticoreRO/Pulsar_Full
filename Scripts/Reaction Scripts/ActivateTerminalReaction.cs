using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Add/Reactions/Activate terminal"))]
public class ActivateTerminalReaction : Reaction
{
    public CommandPanel whichOne;

    public override void ExecuteReaction(GameController controller)
    {
        whichOne.isActivated = true;

    }
}
