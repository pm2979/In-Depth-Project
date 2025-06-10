using System;
using UnityEngine;

public enum ItemType1
{
    Equipable,
    Consumable,
}

[Serializable]
public class ItemDataConsumable
{
    public ConditionType type;
    public float value;
}

[Serializable]
public class EquipItemCost
{
    public ItemData item;  // 필요한 아이템
    public int amount;     // 필요한 수량
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType1 type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}

