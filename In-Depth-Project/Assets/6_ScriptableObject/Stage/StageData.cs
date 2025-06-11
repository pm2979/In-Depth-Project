using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Stage")]
public class StageData : ScriptableObject
{
    public StageInfo[] Stages;
}

[System.Serializable]
public class StageInfo
{
    public int stageKey;
    public WaveData[] waves;

}

[System.Serializable]
public class WaveData
{
    public MonswerSpawnData[] monsters;
}

[System.Serializable]
public class MonswerSpawnData
{
    public GameObject monsterType;
    public int spawnCount;
}
