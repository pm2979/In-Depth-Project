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
    [SerializeField] private TextMeshProUGUI selectedPriceName;
    [SerializeField] private TextMeshProUGUI selectedPriceValue;
    [SerializeField] private TextMeshProUGUI selectedWeaponLevel;
    [SerializeField] private GameObject useBtn;
    [SerializeField] private GameObject purchaseBtn;
    [SerializeField] private GameObject enhanceBtn;

    private ItemData selectedItem;
    private UISlot selectedSlot;
    private CurrencyManager currencyManager;
    private Weapon weapon;
    private Player player;

    private void Start()
    {
        currencyManager = GameManager.Instance.CurrencyManager;
        player = GameManager.Instance.Player;
        weapon = player.Weapon;

        UpdateItemSlotsUI();
        UpdateWeaponSlotsUI();
        ClearSelctedItemWindow();
        inventoryPanel.SetActive(false);
        WeaponPower();
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
        selectedPriceValue.text = string.Empty;
        selectedPriceName.enabled = false;
        selectedWeaponLevel.text = string.Empty;

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
        selectedPriceValue.text = string.Empty;
        selectedWeaponLevel.text = string.Empty;
        selectedPriceName.enabled = false;

        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++) // ǥ��
            {
                selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
                selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
            }

            selectedPriceName.enabled = true;
            selectedPriceValue.text = selectedItem.price.ToString();
        }
        else if(selectedItem.type == ItemType.Equipable)
        {
            selectedStatName.text += selectedItem.equipLevels[selectedItem.currentEnhancementLevel].type.ToString();
            selectedStatValue.text += selectedItem.equipLevels[selectedItem.currentEnhancementLevel].power.ToString();

            selectedWeaponLevel.text = $" + {selectedItem.currentEnhancementLevel}";

            if(selectedItem.equipLevels[selectedItem.currentEnhancementLevel].price > 0)
            {
                selectedPriceName.enabled = true;
                selectedPriceValue.text = selectedItem.equipLevels[selectedItem.currentEnhancementLevel].price.ToString();
            }
        }

        useBtn.SetActive(selectedItem.type == ItemType.Consumable && uiSlot.quantity > 0);
        purchaseBtn.SetActive(selectedItem.type == ItemType.Consumable);
        enhanceBtn.SetActive(selectedItem.type == ItemType.Equipable);
    }

    public void OnUseButton() // ��� ��ư
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConditionType.Hp:
                        player.Condition.Heal(selectedItem.consumables[i].value);
                        break;
                }
            }
            selectedSlot.quantity--;
            useBtn.SetActive(selectedSlot.quantity > 0);
            UpdateUI();
        }
    }

    public void OnPurchaseButton()  // ���� ��ư
    {
        if(currencyManager.Spend(selectedItem.price))
        {
            selectedSlot.quantity++;
            useBtn.SetActive(true);
            UpdateUI();
        }
    }

    public void OnEnhanceButton() // ��ȭ ��ư
    {
        if (selectedItem.currentEnhancementLevel + 1 == selectedItem.equipLevels.Count) return;

        if (currencyManager.Spend(selectedItem.equipLevels[selectedItem.currentEnhancementLevel].price))
        {
            selectedItem.currentEnhancementLevel++;
            SelectItem(selectedSlot);
            WeaponPower();
        }
    }

    public void WeaponPower()
    {
        int power = 0;

        for(int i = 0; i < weaponSlots.Count; i++)
        {
            power += weaponSlots[i].item.equipLevels[weaponSlots[i].item.currentEnhancementLevel].power;

        }

        weapon.SetWeaponPower(power);
    }
}
