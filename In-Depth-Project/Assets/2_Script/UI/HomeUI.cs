using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [SerializeField] private Button startButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        startButton.onClick.AddListener(OnClickStartButton);
    }

    public void OnClickStartButton()
    {
        GameManager.Instance.StartGame();
    }

    protected override UIState GetUIState()
    {
        return UIState.Home;
    }
}

