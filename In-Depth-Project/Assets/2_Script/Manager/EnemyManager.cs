using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]private List<Enemy> activeEnemies = new List<Enemy>();

    private Coroutine waveRoutine;
    private bool enemySpawnComplite;
    private MonswerSpawnData monsterSpawnData;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    [Header("스폰 영역 범위")]
    public Vector3 halfExtents = new Vector3(10, 5, 10);

    public void AddEnemy(Enemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void RemoveEnemyOnDeath(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        if (enemySpawnComplite && activeEnemies.Count == 0)
            GameManager.Instance.StageManager.EndOfWave();
    }

    public Enemy GetNearestEnemy(Vector3 fromPosition) // 플레이어 위치 기준으로 가장 가까운 적을 찾기
    {
        float minSqr = float.MaxValue;
        Enemy nearest = null;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy enemy = activeEnemies[i];

            if (enemy.IsDie == true) 
            { 
                activeEnemies.RemoveAt(i--); 
                continue; 
            }

            float sqr = (enemy.transform.position - fromPosition).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = enemy;
            }
        }
        return nearest;
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplite = false;
        yield return new WaitForSeconds(timeBetweenSpawns);

        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            EnemySpawn(monsterSpawnData.monsterType);
        }

        enemySpawnComplite = true;
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    public void StartStage(StageInstance stageInstance)
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        waveRoutine = StartCoroutine(SpawnStart(stageInstance));
    }

    private IEnumerator SpawnStart(StageInstance stageInstance)
    {
        enemySpawnComplite = false;
        yield return new WaitForSeconds(timeBetweenWaves);

        WaveData waveData = stageInstance.currentStaggeInfo.waves[stageInstance.currentWave];

        for (int i = 0; i < waveData.monsters.Length; i++)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            monsterSpawnData = waveData.monsters[i];
            for (int j = 0; j < monsterSpawnData.spawnCount; j++)
            {
                EnemySpawn(monsterSpawnData.monsterType);
            }
        }

        enemySpawnComplite = true;
    }

    private void EnemySpawn(GameObject enemyPrefab) // 적 스폰 범위
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-halfExtents.x, halfExtents.x),
            Random.Range(-halfExtents.y, halfExtents.y),
            Random.Range(-halfExtents.z, halfExtents.z)
        );
        Vector3 spawnPos = this.transform.position + randomOffset;
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        EnemyManager.Instance.AddEnemy(enemy);
    }

    void OnDrawGizmosSelected() // Scene 뷰에서 박스 영역 시각화
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position, halfExtents * 2);
    }
}
