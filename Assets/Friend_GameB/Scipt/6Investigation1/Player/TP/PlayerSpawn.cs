using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("=== PlayerSpawn Start ===");
        Debug.Log("hasSpawnPosition : " + TeleportData.hasSpawnPosition);

        if (TeleportData.hasSpawnPosition)
        {
            Debug.Log("저장된 위치 : " + TeleportData.spawnPosition);

            transform.position = TeleportData.spawnPosition;

            Debug.Log("이동 후 위치 : " + transform.position);

            TeleportData.hasSpawnPosition = false;
        }
        else
        {
            Debug.Log("저장된 위치가 없습니다.");
        }
    }
}