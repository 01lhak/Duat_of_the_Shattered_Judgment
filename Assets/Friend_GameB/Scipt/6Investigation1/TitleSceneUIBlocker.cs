using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneUIBlocker : MonoBehaviour
{
    public GameObject uiCanvas;

    private void Update()
    {
        if (uiCanvas == null)
            return;

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex >= 0 && sceneIndex <= 3)
        {
            if (uiCanvas.activeSelf)
                uiCanvas.SetActive(false);
        }
        else
        {
            if (!uiCanvas.activeSelf)
                uiCanvas.SetActive(true);
        }
    }
}