using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/Terminals/Portable Terminal")]
public class OpenPortableReaction : Reaction
{
    public override void ExecuteReaction(GameController controller)
    {
        controller.PortableTerminalCommand();
    }
}
