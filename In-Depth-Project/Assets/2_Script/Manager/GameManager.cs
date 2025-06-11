using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string uiSceneName = "UI";
    bool isLoaded = false;
    private UIManager uiManager;
    public Player Player { get; private set; }
    public CurrencyManager CurrencyManager { get; private set; }
    public StageManager StageManager { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<Player>();
        CurrencyManager = GetComponentInChildren<CurrencyManager>();
        StageManager = GetComponentInChildren<StageManager>();
        Time.timeScale = 0.0f;
        LoadUI();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if(uiManager == null) 
        { 
            StartGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        uiManager.SetPlayGame();
        StageManager.Init();
    }

    public void LoadUI()
    {
        if (isLoaded) return;
        SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive)
            .completed += op => {
                isLoaded = true;
                Debug.Log($"UI 씬 '{uiSceneName}' 로딩 완료");
            };
    }

    public void UnloadUI()
    {
        if (!isLoaded) return;
        SceneManager.UnloadSceneAsync(uiSceneName)
            .completed += op => {
                isLoaded = false;
                Debug.Log($"UI 씬 '{uiSceneName}' 언로딩 완료");
            };
    }
}
