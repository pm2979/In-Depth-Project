using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlot;

    [Header("��� �г�")]
    [SerializeField] private List<UISlot> weaponSlots;

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

    private ItemData selectedItem;
    private UISlot selectedSlot;

    private void Start()
    {
        UpdateItemSlotsUI();
        UpdateWeaponSlotsUI();
        ClearSelctedItemWindow();
        inventoryPanel.SetActive(false);
    }

    private void UpdateWeaponSlotsUI() // WeaponUI �ʱ� ����
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            // ������ ����
            weaponSlots[i].Set();
        }
    }

    private void UpdateItemSlotsUI() // ItemUI �ʱ� ����
    {
        uiSlots = new List<UISlot>();

        for (int i = 0; i < itemData.Count; i++)
        {
            // �ν���Ʈ ����
            UISlot slot = Instantiate(itemSlot, itemSlotsParent)
                             .GetComponent<UISlot>();
            // ������ ����
            slot.item = itemData[i];
            slot.Set();

            // ����Ʈ�� �߰�
            uiSlots.Add(slot);
        }
    }

    void UpdateUI() // UI ������Ʈ
    {
        selectedSlot.Set();
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

    public void SelectItem(UISlot uiSlot) // �κ��丮 ������ UI Ŭ��
    {
        selectedSlot = uiSlot;
        selectedItem = uiSlot.item;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useBtn.SetActive(selectedItem.type == ItemType.Consumable && uiSlot.quantity > 0);
        purchaseBtn.SetActive(selectedItem.type == ItemType.Consumable);
        enhanceBtn.SetActive(selectedItem.type == ItemType.Equipable);
    }

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConditionType.Hp:
                        // ȸ�� ȿ��
                        break;
                }
            }
            selectedSlot.quantity--;
            useBtn.SetActive(selectedSlot.quantity > 0);
            UpdateUI();
        }
    }

    public void OnPurchaseButton()
    {
        // �� �Ҹ�
        selectedSlot.quantity++;
        useBtn.SetActive(true);
        UpdateUI();
    }

    public void OnEnhanceButton()
    {

    }
}
