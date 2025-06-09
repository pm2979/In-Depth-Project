using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] public TextMeshProUGUI waveText;
    [SerializeField] public Slider hpSlider;

    private void Start()
    {
        //UpdateHPSlider(1);
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventType.HPUI, UpdateHPSlider);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.HPUI, UpdateHPSlider);
    }

    public void UpdateHPSlider(float percentage)
    {
        hpSlider.value = percentage;
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = wave.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}


