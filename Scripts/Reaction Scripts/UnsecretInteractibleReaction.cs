using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add/Reactions/Interactibles/Unsecret interactible")]
public class UnsecretInteractibleReaction : Reaction
{
    public bool showFindingMessage = true;
    public Interactible interactible;

    public override void ExecuteReaction(GameController controller)
    {
        interactible.properties.secret = false;
    }
}
