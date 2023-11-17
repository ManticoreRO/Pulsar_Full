using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSave
{
    public bool isfound;
    public bool isdestroyed;
    public bool isininventory;
    public int usesleft;
    public float chargedvalue;
    public int damage;
    public List<bool> extraLoreData = new List<bool>();

    public ItemSave()
    {

    }

    public ItemSave(Item item)
    {
        isfound = item.isFound;
        isdestroyed = item.isDestroyed;
        isininventory = item.isInInventory;
        usesleft = item.usesLeft;
        chargedvalue = item.chargedValue;
        damage = item.damage;
    }
}
