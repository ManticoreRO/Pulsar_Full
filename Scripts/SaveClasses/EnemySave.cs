using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySave
{
    public int currentenemyarmor;
    public bool isdestroyed;
    public bool isactivated;
    public List<bool> unlockedweaknesses = new List<bool>();

    public EnemySave()
    { }

    public EnemySave(Enemy enemy)
    {
        currentenemyarmor = enemy.currentEnemyArmor;
        isdestroyed = enemy.isDestroyed;
        isactivated = enemy.isActivated;
        for (int i = 0; i < enemy.isUnlockedWeakness.Count; i++)
        {
            unlockedweaknesses.Add(enemy.isUnlockedWeakness[i]);
        }
    }
}
