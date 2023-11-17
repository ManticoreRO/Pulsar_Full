using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Add/Reactions/Terminals/Transfer Data"))]
public class TransferDataReaction : Reaction
{
    public override void ExecuteReaction(GameController controller)
    {
        // we will do this with the current opened terminal
        controller.currentlyShownPanel.TransferDataToPortable();
    }
}
