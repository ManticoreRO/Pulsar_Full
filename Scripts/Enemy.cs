using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Add/Enemy")]
public class Enemy : ScriptableObject, ISerializationCallbackReceiver
{
    public string enemyName = "enemy name";

    [Space(5)]
    [TextArea]
    public string enemyDescription = "";

    [Space(5)]
    public int enemyArmor;
    [System.NonSerialized]
    public int currentEnemyArmor;
    public bool activated = false;

    [Space(5)]
    public List<BooleanSwitch> appearConditions = new List<BooleanSwitch>();
    public List<Area> areasToAppearIn = new List<Area>();
    public List<int> chanceToSpawn = new List<int>();

    [Space(5)]
    public List<Item> enemyWeaknesses = new List<Item>();
    public List<bool> unlockedWeaknesses = new List<bool>();
    [System.NonSerialized]
    public List<bool> isUnlockedWeakness;
    [TextArea]
    public List<string> weaknessText = new List<string>();

    [Space(5)]
    public List<string> weaponsUsed = new List<string>();
    public List<int> weaponsDamage = new List<int>();
    public List<float> weaponUseChance = new List<float>();

    [Space(5)]
    [Header("Rewards")]
    public List<Item> loot = new List<Item>();
    public List<float> chanceToDrop = new List<float>();

    [Space(5)]
    [TextArea]
    public string runAwayDescription = "";
    [TextArea]
    public string defeatDescription = "";

    [System.NonSerialized]
    public bool isDestroyed;
    [System.NonSerialized]
    public bool isActivated;

    public AudioClip battleMusic;

    public void OnAfterDeserialize()
    {
        currentEnemyArmor = enemyArmor;
        isDestroyed = false;
        isUnlockedWeakness = new List<bool>();
        for (int i = 0; i < unlockedWeaknesses.Count; i++)
        {
            isUnlockedWeakness.Add(unlockedWeaknesses[i]);
        }
        isActivated = activated;
    }

    public void OnBeforeSerialize()
    { }

    public void LoadEnemyData(EnemySave data)
    {
        currentEnemyArmor = data.currentenemyarmor;
        isDestroyed = data.isdestroyed;
        isActivated = data.isactivated;
        isUnlockedWeakness = new List<bool>();

        for (int i = 0; i < data.unlockedweaknesses.Count; i++)
        {
            isUnlockedWeakness.Add(data.unlockedweaknesses[i]);
        }
    }

    public bool IsDead()
    {
        return currentEnemyArmor <= 0;
    }

    public bool TryEscape()
    {
        // cant escape if too low
        if (enemyArmor <= 1) return false;

        int chance = Random.Range(0, currentEnemyArmor);
        return chance <= currentEnemyArmor / 4;
    }

    public string AttackPlayer()
    {
        string result = "";
        int selectedWeapon = -1;

        for (int i = 0; i < weaponUseChance.Count; i++)
        {
            float roll = Random.Range(0, 100f);
            if (roll <= weaponUseChance[i] && selectedWeapon == -1)
            {
                selectedWeapon = i;
            }
        }

        if (selectedWeapon != -1)
        {
            int damageDealt = Random.Range(0, weaponsDamage[selectedWeapon]);
            // attack player
            if (damageDealt != 0)
            {
                // is this critical
                if (damageDealt != weaponsDamage[selectedWeapon] - 1)
                {
                    result = "\n  " + enemyName + "<color=red> attacks with " + weaponsUsed[selectedWeapon] + " for " + damageDealt.ToString() + " damage!</color>";
                }
                else
                {
                    damageDealt = 2 * Random.Range(1, weaponsDamage[selectedWeapon]);
                    result = "\n  " + enemyName + "<color=red> attacks with " + weaponsUsed[selectedWeapon] + " for " + damageDealt.ToString() + " CRITICAL DAMAGE!</color>";
                }
            }
            else
            {
                result = "\n  " + enemyName + "<color=red> attacks with " + weaponsUsed[selectedWeapon] + " but misses!</color>";
            }
            // remove shield
            GameData.gameData.GetComponent<PlayerHealth>().current -= weaponsDamage[selectedWeapon];

        }
        else
        {
            result = "\n" + enemyName + " gets into defensive position!";
            currentEnemyArmor += 1;
        }

        
        return result;
    }
}
