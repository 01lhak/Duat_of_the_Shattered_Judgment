using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
    public void GoToScene0()
    {
        SFXManager.Instance.PlayClick();
        SceneManager.LoadScene(0);
    }
}