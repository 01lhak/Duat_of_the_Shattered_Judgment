using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void OnClickStart()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();

        if (GameManager.Instance != null)
            GameManager.Instance.ResetGameData();

        SceneManager.LoadScene(1);
    }

    public void OnClickSaveLoad()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();

        SceneManager.LoadScene(2);
    }

    public void OnClickOptions()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();

        SceneManager.LoadScene(3);
    }

    public void OnClickClose()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}