using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    private InventoryUI inventoryUI;
    public ItemData item;
    public TextMeshProUGUI quantityText;
    public Image icon;

    public int quantity;

    void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity >= 1 ? quantity.ToString() : string.Empty;
    }

    public void OnClickButton() // ���� Ŭ��
    {
        inventoryUI.SelectItem(this);
    }
}
