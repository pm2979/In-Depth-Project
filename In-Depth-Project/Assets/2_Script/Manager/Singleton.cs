using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component // T�� �ݵ�� Component Ŭ������ ����
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // FindObjectOfType<T>()�� ������ Ÿ���� ù��°�� �ε�� ������Ʈ�� �˻�

                if (_instance == null) // �˻��� ������Ʈ�� ������
                {
                    GameObject obj = new GameObject(); // ���ο� GameObject�� ����
                    obj.name = typeof(T).Name; // GameObject�� �̸��� TŸ������
                    _instance = obj.AddComponent<T>(); // ������Ʈ �߰�
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T; // ���� Awake�� ��� �ڽ��� �ν��Ͻ��� ����
            DontDestroyOnLoad(gameObject); // �� ��ȯ���� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
