using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlot;

    [Header("장비 패널")]
    [SerializeField] private List<UISlot> weaponSlots;

    [Header("아이템 패널")]
    [SerializeField] private List<ItemData> itemData;
    [SerializeField] private Transform itemSlotsParent;
    private List<UISlot> uiSlots;

    [Header("정보 패널")]
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

    private void UpdateWeaponSlotsUI() // WeaponUI 초기 설정
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            // 데이터 설정
            weaponSlots[i].Set();
        }
    }

    private void UpdateItemSlotsUI() // ItemUI 초기 설정
    {
        uiSlots = new List<UISlot>();

        for (int i = 0; i < itemData.Count; i++)
        {
            // 인스턴트 생성
            UISlot slot = Instantiate(itemSlot, itemSlotsParent)
                             .GetComponent<UISlot>();
            // 데이터 설정
            slot.item = itemData[i];
            slot.Set();

            // 리스트에 추가
            uiSlots.Add(slot);
        }
    }

    void UpdateUI() // UI 업데이트
    {
        selectedSlot.Set();
    }

    public void OnClickInventoryButton() // 인벤토리 오픈
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

    public void ClearSelctedItemWindow() // UI 정보 초기화
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

    public void SelectItem(UISlot uiSlot) // 인벤토리 아이템 UI 클릭
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
            for (int i = 0; i < selectedItem.consumables.Length; i++) // 표시
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

    public void OnUseButton() // 사용 버튼
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

    public void OnPurchaseButton()  // 구매 버튼
    {
        if(currencyManager.Spend(selectedItem.price))
        {
            selectedSlot.quantity++;
            useBtn.SetActive(true);
            UpdateUI();
        }
    }

    public void OnEnhanceButton() // 강화 버튼
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
