using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    // ��ȭ �ܾ�
    [SerializeField]private int currency;

    // ��ȭ �߰�
    public void Add(int amount)
    {
        if (amount <= 0) return;
        currency += amount;
        SaveCurrency();
        EventBus.Publish(EventType.CurrencyUI, currency);
    }

    // ��ȭ ��� 
    public bool Spend(int amount)
    {
        if (amount <= 0 || currency < amount)
            return false;

        currency -= amount;
        SaveCurrency();
        EventBus.Publish(EventType.CurrencyUI, currency);
        return true;
    }

    // PlayerPrefs�� ����
    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("CurrencySave", currency);
        PlayerPrefs.Save();
    }

    // PlayerPrefs���� �ҷ�����
    public void LoadCurrency()
    {
        currency = PlayerPrefs.GetInt("CurrencySave", 0);
        EventBus.Publish(EventType.CurrencyUI, currency);
    }
}
