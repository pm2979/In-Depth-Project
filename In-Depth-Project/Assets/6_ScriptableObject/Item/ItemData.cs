using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemDataConsumable
{
    public ConditionType type;
    public float value;
}

[System.Serializable]
public class EnhancementLevelData
{
    public int level;
    public AbilityType type;
    public int power;
    public int price;
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    public int price;

    [Header("Equip")]
    public List<EnhancementLevelData> equipLevels;
    public int currentEnhancementLevel = 0;

}

