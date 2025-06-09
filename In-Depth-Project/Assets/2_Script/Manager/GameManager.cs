using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Tooltip("빌드 세팅에 추가한 UI 씬 이름")]
    public string uiSceneName = "UI";
    bool isLoaded = false;
    [SerializeField] private UIManager uiManager;

    public override void Awake()
    {
        base.Awake();

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
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        uiManager.SetPlayGame();
    }

    public void LoadUI()
    {
        if (isLoaded) return;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive)
            .completed += op => {
                isLoaded = true;
                Debug.Log($"UI 씬 '{uiSceneName}' 로딩 완료");
            };
    }

    public void UnloadUI()
    {
        if (!isLoaded) return;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(uiSceneName)
            .completed += op => {
                isLoaded = false;
                Debug.Log($"UI 씬 '{uiSceneName}' 언로딩 완료");
            };
    }
}
