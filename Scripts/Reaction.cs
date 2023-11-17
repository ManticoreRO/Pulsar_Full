using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reaction : ScriptableObject
{
    public abstract void ExecuteReaction(GameController controller);
}
