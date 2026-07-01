using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportZone : MonoBehaviour
{
    [Header("설정")]
    public string targetSceneName;
    public float spawnX;
    public float spawnY;
    public GameObject interactionIcon;

    private bool playerInside = false;

    void Start()
    {
        if (interactionIcon)
            interactionIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (interactionIcon)
            interactionIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (interactionIcon)
            interactionIcon.SetActive(false);
    }

    private void Update()
    {
        // 인트로 중에는 입력 차단
        if (!FadeInEffect.isInputAllowed)
            return;

        // 설정창 또는 인벤토리가 열려 있으면 입력 차단
        if (GameManager.IsInputLocked)
            return;

        if (playerInside && Input.GetKeyDown(KeyCode.Space))
        {
            if (interactionIcon)
                interactionIcon.SetActive(false);

            TeleportData.spawnPosition = new Vector3(spawnX, spawnY, 0);
            TeleportData.hasSpawnPosition = true;

            SceneManager.LoadScene(targetSceneName);
        }
    }
}