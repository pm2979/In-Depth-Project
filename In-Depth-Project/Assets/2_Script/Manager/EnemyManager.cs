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
    /// �÷��̾� ��ġ �������� ���� ����� ���� ã���ϴ�.
    /// </summary>
    public Enemy GetNearestEnemy(Vector3 fromPosition)
    {
        float minSqr = float.MaxValue;
        Enemy nearest = null;

        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            Enemy e = ActiveEnemies[i];
            // Ȥ�� �̹� �ı��� �� ������ ������ �ɷ�����
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
