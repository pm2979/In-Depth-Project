using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlot;

    [Header("��� �г�")]
    [SerializeField] private List<ItemData> weaponData;

    [Header("������ �г�")]
    [SerializeField] private List<ItemData> itemData;
    [SerializeField] private Transform itemSlotsParent;
    private List<UISlot> uiSlots;

    [Header("���� �г�")]
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private TextMeshProUGUI selectedStatName;
    [SerializeField] private TextMeshProUGUI selectedStatValue;
    [SerializeField] private GameObject useBtn;
    [SerializeField] private GameObject purchaseBtn;
    [SerializeField] private GameObject enhanceBtn;


    private void Start()
    {
        UpdateItemSlotsUI();
        ClearSelctedItemWindow();
        inventoryPanel.SetActive(false);
    }

    private void UpdateItemSlotsUI() // UI �ʱ� ����
    {
        uiSlots = new List<UISlot>();

        for (int i = 0; i < itemData.Count; i++)
        {
            // �ν���Ʈ ����
            UISlot slot = Instantiate(itemSlot, itemSlotsParent)
                             .GetComponent<UISlot>();
            // ������ ����
            slot.Set(itemData[i], 0);

            // ����Ʈ�� �߰�
            uiSlots.Add(slot);
        }
    }

    public void OnClickInventoryButton() // �κ��丮 ����
    {
        if(!IsOpen())
        {
            inventoryPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if(IsOpen())
        {
            inventoryPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public bool IsOpen()
    {
        return inventoryPanel.activeInHierarchy;
    }

    public void ClearSelctedItemWindow() // UI ���� �ʱ�ȭ
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useBtn.SetActive(false);
        purchaseBtn.SetActive(false);
        enhanceBtn.SetActive(false);
    }

    public void SelectItem(ItemData itemData) // �κ��丮 ������ UI Ŭ��
    {
        selectedItemName.text = itemData.displayName;
        selectedItemDescription.text = itemData.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < itemData.consumables.Length; i++)
        {
            selectedStatName.text += itemData.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += itemData.consumables[i].value.ToString() + "\n";
        }

        useBtn.SetActive(itemData.type == ItemType1.Consumable);
        purchaseBtn.SetActive(itemData.type == ItemType1.Consumable);
        enhanceBtn.SetActive(itemData.type == ItemType1.Equipable);
    }
}
