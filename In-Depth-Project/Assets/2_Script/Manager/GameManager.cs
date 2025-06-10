using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Tooltip("UI ¾À ÀÌ¸§")]
    public string uiSceneName = "UI";
    bool isLoaded = false;
    [SerializeField] private UIManager uiManager;
    public Player Player { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<Player>();
        Time.timeScale = 0.0f;
        LoadUI();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        
        if(uiManager == null ) 
        { 
            StartGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        uiManager.SetPlayGame();
    }

    public void LoadUI()
    {
        if (isLoaded) return;
        SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive)
            .completed += op => {
                isLoaded = true;
                Debug.Log($"UI ¾À '{uiSceneName}' ·Îµù ¿Ï·á");
            };
    }

    public void UnloadUI()
    {
        if (!isLoaded) return;
        SceneManager.UnloadSceneAsync(uiSceneName)
            .completed += op => {
                isLoaded = false;
                Debug.Log($"UI ¾À '{uiSceneName}' ¾ð·Îµù ¿Ï·á");
            };
    }
}
