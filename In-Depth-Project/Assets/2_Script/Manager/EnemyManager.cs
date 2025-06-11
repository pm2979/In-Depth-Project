using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<Enemy> activeEnemies = new List<Enemy>();

    public void RegisterEnemy(Enemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        GetNearestEnemy(GameManager.Instance.Player.transform.position);
        activeEnemies.Remove(enemy);
    }

    public Enemy GetNearestEnemy(Vector3 fromPosition) // 플레이어 위치 기준으로 가장 가까운 적을 찾기
    {
        float minSqr = float.MaxValue;
        Enemy nearest = null;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy enemy = activeEnemies[i];

            if (enemy == null) { activeEnemies.RemoveAt(i--); continue; } // 혹시 이미 파괴된 적 참조가 있으면 걸러내기

            float sqr = (enemy.transform.position - fromPosition).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = enemy;
            }
        }
        return nearest;
    }

    public bool ActiveEnemyNull()
    {
        return activeEnemies == null;
    }
}
