using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("�� ������")]
    public GameObject enemyPrefab;

    [Header("���� ���� ����(�ݰ�)")]
    public Vector3 halfExtents = new Vector3(10, 5, 10);

    [Header("�� ���� ������ �� ��")]
    public int spawnCount = 10;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
            SpawnInBox();
    }

    void SpawnInBox()
    {
        // �� �ະ�� -halfExtents ~ +halfExtents ���� ����
        Vector3 randomOffset = new Vector3(
            Random.Range(-halfExtents.x, halfExtents.x),
            Random.Range(-halfExtents.y, halfExtents.y),
            Random.Range(-halfExtents.z, halfExtents.z)
        );
        Vector3 spawnPos = this.transform.position + randomOffset;
        Enemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        EnemyManager.Instance.RegisterEnemy(enemy);
    }

    // Scene �信�� �ڽ� ���� �ð�ȭ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position, halfExtents * 2);
    }
}
