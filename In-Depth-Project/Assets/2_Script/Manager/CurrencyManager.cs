using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    // 재화 잔액
    [SerializeField]private int currency;

    // 재화 추가
    public void Add(int amount)
    {
        if (amount <= 0) return;
        currency += amount;
        SaveCurrency();
        EventBus.Publish(EventType.CurrencyUI, currency);
    }

    // 재화 사용 
    public bool Spend(int amount)
    {
        if (amount <= 0 || currency < amount)
            return false;

        currency -= amount;
        SaveCurrency();
        EventBus.Publish(EventType.CurrencyUI, currency);
        return true;
    }

    // PlayerPrefs에 저장
    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("CurrencySave", currency);
        PlayerPrefs.Save();
    }

    // PlayerPrefs에서 불러오기
    public void LoadCurrency()
    {
        currency = PlayerPrefs.GetInt("CurrencySave", 0);
        EventBus.Publish(EventType.CurrencyUI, currency);
    }
}
