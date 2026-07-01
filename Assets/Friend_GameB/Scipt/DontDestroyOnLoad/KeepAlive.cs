using UnityEngine;

public class KeepAlive : MonoBehaviour
{
    private void Awake()
    {
        // 이미 씬에 'GlobalManagers'가 있다면 새로 생성된 건 삭제함
        if (FindObjectsByType<KeepAlive>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // 이 보따리를 파괴하지 말고 다음 씬까지 가져가라!
        }
    }
}