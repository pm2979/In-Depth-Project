using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] public TextMeshProUGUI currencyText;
    [SerializeField] public Slider hpSlider;

    private void Start()
    {
        GameManager.Instance.CurrencyManager.LoadCurrency();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.HpUI, UpdateHPSlider);
        EventBus.Subscribe(EventType.CurrencyUI, UpdateCurrencyText);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.HpUI, UpdateHPSlider);
        EventBus.Unsubscribe(EventType.CurrencyUI, UpdateCurrencyText);
    }

    public void UpdateHPSlider(float percentage)
    {
        hpSlider.value = percentage;
    }

    public void UpdateCurrencyText(float coin)
    {
        currencyText.text = coin.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}


