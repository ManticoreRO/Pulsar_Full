using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters;

[CreateAssetMenu(menuName ="Add/Item")]
public class Item : ScriptableObject, ISerializationCallbackReceiver
{
    public Area foundInArea;

    public string actionVerb = "Use";

    [TextArea]
    public string itemDescription = "";
    [TextArea]
    public string itemDescriptionWhenFound = "";
    [TextArea]
    public string itemInventoryDescription = "";
    [TextArea]
    public string itemOnUseAloneDescription = "";

    public List<BooleanSwitch> foundIfConditions = new List<BooleanSwitch>();

    public float chanceToFind = 100f;
    public bool isSecret = false;
    public bool destroyOnUsed = false;
    public bool visibleWhenUnlit = false;
    public bool changeScreen = false;

    [Space(5)]
    public bool isWeapon = false;
    public int standardDamage = 0;
    [TextArea]
    public string useWeaponText = "";

    [Space(5)]
    public bool found = false;
    [System.NonSerialized]
    public bool isFound;

    public bool destroyed = false;
    [System.NonSerialized]
    public bool isDestroyed;

    public bool inInventory = false;
    [System.NonSerialized]
    public bool isInInventory;

    public bool onlyOnPanels = false;
    public CommandPanel specificPanel;

    public int numberOfUses = 1;
    [System.NonSerialized]
    public int usesLeft;
    [System.NonSerialized]
    public int damage;

    // For items that can be charged with energy
    [System.NonSerialized]
    public float chargedValue;
    public Reaction onChargedReaction;

    [Space(5)]
    public Reaction onPickupReaction;

    [Space(5)]
    [Header("Item usage")]
    public Reaction onUsedAlone;
    [Space(2)]
    public List<Interactible> usableWith = new List<Interactible>();
    public List<Reaction> onUsedWithInteractible = new List<Reaction>();
    public List<BooleanSwitch> onUsedWithCondition = new List<BooleanSwitch>();

    public void OnAfterDeserialize()
    {
        isFound = found;
        isDestroyed = destroyed;
        isInInventory = inInventory;
        usesLeft = numberOfUses;
        chargedValue = 0f;
        damage = standardDamage;
    }

    public void OnBeforeSerialize()
    { }

    public void LoadItemData(ItemSave data)
    {
        isFound = data.isfound;
        isDestroyed = data.isdestroyed;
        isInInventory = data.isininventory;
        usesLeft = data.usesleft;
        chargedValue = data.chargedvalue;
        damage = data.damage;
    }

    public void UseAlone(GameController controller)
    {
        controller.AddTextWithReturn(itemOnUseAloneDescription, Color.cyan);

        if (usesLeft == 0 && !destroyOnUsed)
        {
            controller.secondaryWindowLog.text = "<color=red>  You cannot use this!</color>";
            return;
        }

        if (!onlyOnPanels)
        {
            if (usesLeft > 0 && onUsedAlone!=null) usesLeft -= 1;
            if (onUsedAlone!=null) onUsedAlone.ExecuteReaction(controller);
            if (changeScreen) return;
        }
        else
        if (controller.selectedPanelToUse != null)
        {
            if (usesLeft > 0) usesLeft -= 1;
            if (onUsedAlone!=null) onUsedAlone.ExecuteReaction(controller);
        }
        else
        {
            controller.secondaryWindowLog.text = "<color=red>  You cannot use the " + name + " here!";
        }

        if (usesLeft == 0 && destroyOnUsed)
        {
            // if no more uses, we destroy it
            RemoveItem();
        }

        if (name!="Portable terminal") GameController.controller.ReturnToMainWindowCommand();
    }

    public void Use(GameController controller, Interactible interactible)
    {
        if (usableWith.Contains(interactible) && usesLeft > 0)
        {
            if (onUsedWithCondition.Count == 0) return;
            if (onUsedWithCondition.Count < usableWith.Count) return;
            if (onUsedWithCondition[usableWith.IndexOf(interactible)] != null)
            {
                if (!onUsedWithCondition[usableWith.IndexOf(interactible)].runValue)
                {
                    GameController.controller.secondaryWindowLog.text = "<color=red> This doesn't seem to work! </color>";
                    return;
                }
            }

            // for now we just execute the reactions
            if (onUsedWithInteractible.Count < 1)
            {
                return;
            }
            else
            {
                if (onUsedWithInteractible[usableWith.IndexOf(interactible)] != null)
                {
                    onUsedWithInteractible[usableWith.IndexOf(interactible)].ExecuteReaction(controller);
                }
                else
                {
                    Debug.Log("No reaction for " + interactible.name + " when used with " + name + "!");
                }
            }
            if (usesLeft > 0) usesLeft -= 1;

 

            // if interactible needs to be destroyed, we do that aswell
            if (interactible.destroyOnUse)
            {
                interactible.isDestroyed = true;
                GameController.controller.areaManager.InitDiscoveredInteractibles();
            }

            GameController.controller.ReturnToMainWindowCommand();
        }
        else 
        if (interactible == null && !onlyOnPanels)
        {
            UseAlone(controller);
        }
        else
        if (!onlyOnPanels)
        {
            GameController.controller.secondaryWindowLog.text = "<color=red>  " + name + " cannot be used with " + interactible.name + "!</color>";
        }

        if (usesLeft == 0 && destroyOnUsed)
        {
            // destroy item
            RemoveItem();
        }
    }

    public void UseOnPanel(CommandPanel panel)
    {
        if (!onlyOnPanels)
        {
            GameController.controller.secondaryWindowLog.text = "<color=red>  You cannot use this with terminals!</color>";
            return;
        }

        if (specificPanel != null)
        {
            if (panel == specificPanel)
            {
                UseAlone(GameController.controller);
            }
            else
            {
                GameController.controller.secondaryWindowLog.text = "<color=red>  The terminal seems to refuse the strip... </color>";
            }
        }
        else
        {
            UseAlone(GameController.controller);
        }
    }

    public bool IsUsableAlone()
    {
        return usableWith.Count == 0;
    }

    public bool ConditionsMet()
    {
        if (foundIfConditions.Count == 0) return true;

        bool result = foundIfConditions[0];

        for (int i = 1; i < foundIfConditions.Count; i++)
        {
            result = result && foundIfConditions[i];
        }

        return result;
    }

    public bool TryDiscoverItem(bool isRoomLit)
    {
        float chanceModifier = 0f;
        if (isFound) return true;       
        if (!ConditionsMet()) return false;
        if (isSecret) return false;

        if (!isRoomLit && !visibleWhenUnlit) chanceModifier = -50f;
        float roll = Random.Range(1f, 100f);

        if (roll <= chanceToFind + chanceModifier)
        {
            isFound = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForUseEvents(GameController controller)
    {
    }

    public void PickUpItem(GameController controller)
    {
        isInInventory = true;
        isFound = true;

        controller.secondaryWindowLog.text = "  <color=cyan>"+ name + "</color> picked up!";
        string lcasename = name.ToLower();
        if (lcasename == "portable terminal") controller.portablePickedUp = true;
        controller.areaManager.inventory.Add(this);
        controller.areaManager.discoveredItems.Remove(this);
        if (onPickupReaction != null) onPickupReaction.ExecuteReaction(GameController.controller);
    }

    public string Inspect()
    {
        string textToReturn = " ";
        textToReturn += itemInventoryDescription;

        return textToReturn;
    }

    public void AddCharge(int value)
    {
        usesLeft += value;
        if (usesLeft > 20f)
        {
            usesLeft = 20;
            if (onChargedReaction != null) onChargedReaction.ExecuteReaction(GameController.controller);
        }
    }

    public void RemoveItem()
    {
        isInInventory = false;
        isDestroyed = true;
        GameController.controller.areaManager.inventory.Remove(this);
    }

    public string AttackEnemy(Enemy enemy)
    {
        string result = "";

        // check enemy resistances
        if (enemy.enemyWeaknesses.Contains(this) && enemy.isUnlockedWeakness[enemy.enemyWeaknesses.IndexOf(this)])
        {
            // test if we can use this
            if (usesLeft > 0)
            {
                // do damage
                int damageDealt = (int)Random.Range(0, damage);
                enemy.currentEnemyArmor -= damageDealt;
                if (damageDealt != 0)
                {
                    if (damageDealt != standardDamage - 1)
                    {
                        result += "\n" + useWeaponText + "\n<color=green>  You damage " + enemy.enemyName + " for " + damageDealt.ToString() + " armor. </color>";
                    }
                    else
                    {
                        damageDealt = 2 * Random.Range(1, standardDamage);
                        result += "\n" + useWeaponText + "\n<color=green>  CRITICAL HIT! You damage " + enemy.enemyName + " for " + damageDealt.ToString() + " armor. </color>";
                        // there is a small chance for the item to lose a charge
                        int loseChargeRoll = Random.Range(0, 100);
                        if (loseChargeRoll <= 5)
                        {
                            result += "\n  <color=red>" + name + " is less durable!</color>";
                            usesLeft -= 1;
                        }
                    }
                }
                else
                {
                    result += "\n" + useWeaponText + "\n<color=red>  You miss! </color>";
                }
            }
            else
            {
                result += "\n<color=red>" + name + " is unusable!</color>";
            }
        }
        else
        {
            result += "\n" + useWeaponText + "\n<color=red>  Unfortunately, this doesn't seem to do any damage!</color>";
            // give the player a chance to find a weakness if this weapon is on the weakness list
            if (enemy.enemyWeaknesses.Contains(this))
            {
                int roll = Random.Range(0, 100);
                if (roll <= 5)
                {
                    // find weakness
                    enemy.isUnlockedWeakness[enemy.enemyWeaknesses.IndexOf(this)] = true;
                    // show weakness
                    result += "\n<color=green>  " + enemy.weaknessText[enemy.enemyWeaknesses.IndexOf(this)] + "</color>";
                }
            }
        }

        return result;
    }

}
