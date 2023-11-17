using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractibleSave
{
    public bool isfound;
    public bool isdestroyed;
    public List<bool> extraLoreData = new List<bool>();

    public InteractibleSave()
    { }

    public InteractibleSave(Interactible interactible)
    {
        isfound = interactible.isFound;
        isdestroyed = interactible.isDestroyed;
    }
}
