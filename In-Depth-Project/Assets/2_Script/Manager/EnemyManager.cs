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

    public Enemy GetNearestEnemy(Vector3 fromPosition) // �÷��̾� ��ġ �������� ���� ����� ���� ã��
    {
        float minSqr = float.MaxValue;
        Enemy nearest = null;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy enemy = activeEnemies[i];

            if (enemy == null) { activeEnemies.RemoveAt(i--); continue; } // Ȥ�� �̹� �ı��� �� ������ ������ �ɷ�����

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
