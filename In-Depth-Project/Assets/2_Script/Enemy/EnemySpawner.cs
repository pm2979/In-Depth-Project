using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹")]
    public GameObject enemyPrefab;

    [Header("스폰 영역 범위(반경)")]
    public Vector3 halfExtents = new Vector3(10, 5, 10);

    [Header("한 번에 생성할 적 수")]
    public int spawnCount = 10;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
            SpawnInBox();
    }

    void SpawnInBox()
    {
        // 각 축별로 -halfExtents ~ +halfExtents 사이 랜덤
        Vector3 randomOffset = new Vector3(
            Random.Range(-halfExtents.x, halfExtents.x),
            Random.Range(-halfExtents.y, halfExtents.y),
            Random.Range(-halfExtents.z, halfExtents.z)
        );
        Vector3 spawnPos = this.transform.position + randomOffset;
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        EnemyManager.Instance.RegisterEnemy(enemy);
    }

    // Scene 뷰에서 박스 영역 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position, halfExtents * 2);
    }
}
