using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Reactions/_Chain Reaction")]
public class ChainReaction : Reaction
{
    public List<Reaction> chainedReactions = new List<Reaction>();

    public override void ExecuteReaction(GameController controller)
    {
        for (int i = 0; i < chainedReactions.Count; i++)
        {
            chainedReactions[i].ExecuteReaction(controller);
        }
    }
}
