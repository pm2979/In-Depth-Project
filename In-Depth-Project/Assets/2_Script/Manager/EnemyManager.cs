using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<Enemy> ActiveEnemies = new List<Enemy>();

    public void RegisterEnemy(Enemy enemy)
    {
        ActiveEnemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        ActiveEnemies.Remove(enemy);
    }

    /// <summary>
    /// 플레이어 위치 기준으로 가장 가까운 적을 찾습니다.
    /// </summary>
    public Enemy GetNearestEnemy(Vector3 fromPosition)
    {
        float minSqr = float.MaxValue;
        Enemy nearest = null;

        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            Enemy e = ActiveEnemies[i];
            // 혹시 이미 파괴된 적 참조가 있으면 걸러내기
            if (e == null) { ActiveEnemies.RemoveAt(i--); continue; }

            float sqr = (e.transform.position - fromPosition).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = e;
            }
        }
        return nearest;
    }
}
