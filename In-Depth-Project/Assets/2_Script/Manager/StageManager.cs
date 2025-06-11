using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageData stageData;

    private EnemyManager enemyManager;

    private StageInstance currentStageInstance;

    public void Init()
    {
        enemyManager = EnemyManager.Instance;

        currentStageInstance = new StageInstance(0, 0);
        StartStage(currentStageInstance);
    }

    public void EndOfWave()
    {
        StartNextWaveInStage();
    }

    public void StartStage(StageInstance stageInstance)
    {

        StageInfo stageInfo = GetStageInfo(stageInstance.stageKey);

        if (stageInfo == null)
        {
            Debug.Log("스테이지 정보가 없습니다.");
            currentStageInstance = null;
            return;
        }

        stageInstance.SetStageInfo(stageInfo);

        enemyManager.StartStage(currentStageInstance);
    }

    public void StartNextWaveInStage()
    {
        if (currentStageInstance.CkeckEntOfWave())
        {
            currentStageInstance.currentWave++;
            StartStage(currentStageInstance);
        }
        else
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        if (currentStageInstance == null)
            return;

        currentStageInstance.stageKey += 1;
        currentStageInstance.currentWave = 0;
        StartStage(currentStageInstance);
    }


    private StageInfo GetStageInfo(int stageKey) // 스테이지 정보 가져오기
    {
        foreach (var stage in stageData.Stages)
        {
            if (stage.stageKey == stageKey) return stage;
        }

        return null;
    }
}

[System.Serializable]
public class StageInstance
{
    public int stageKey;
    public int currentWave;
    public StageInfo currentStaggeInfo;

    public StageInstance(int stageKey, int currentWave)
    {
        this.stageKey = stageKey;
        this.currentWave = currentWave;
    }

    public void SetStageInfo(StageInfo stageInfo)
    {
        currentStaggeInfo = stageInfo;
    }

    public bool CkeckEntOfWave()
    {
        if (currentStaggeInfo == null) return false;

        if (currentWave >= currentStaggeInfo.waves.Length - 1) return false;

        return true;
    }

}
