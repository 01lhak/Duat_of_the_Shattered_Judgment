using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필수적으로 포함해야 합니다.

public class NPCSceneChanger : MonoBehaviour
{
    // 이동할 씬의 이름을 인스펙터 창에서 직접 지정할 수 있게 합니다.
    [SerializeField] private string targetSceneName = "Scene_01_Intro";

    // 플레이어가 NPC에게 말을 걸거나 상호작용 버튼을 눌렀을 때 호출할 함수
    public void InteractAndChangeScene()
    {
        // 입력한 이름의 씬이 빌드 세팅에 등록되어 있는지 확인 후 이동합니다.
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("이동할 씬 이름이 지정되지 않았습니다!");
        }
    }

    // 예시: 만약 별도의 대화 시스템 없이 NPC에게 다가가 부딪히기만 해도 넘어가게 하려면 아래 주석을 해제하세요.
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractAndChangeScene();
        }
    }
    */
}
