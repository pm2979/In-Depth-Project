using System;
using UnityEngine;

public enum ItemType
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
    public GameObject equipPrefab;
}

