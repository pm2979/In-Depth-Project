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
                        // 회복 효과
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
        // 돈 소모
        selectedSlot.quantity++;
        useBtn.SetActive(true);
        UpdateUI();
    }

    public void OnEnhanceButton()
    {

    }
}
