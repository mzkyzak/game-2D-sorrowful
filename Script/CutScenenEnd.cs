using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour
{
    public string sceneName; // nama scene tujuan

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
