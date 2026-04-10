using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAfterSound(sceneName));
    }

    private IEnumerator LoadSceneAfterSound(string sceneName)
    {
        if (clickSound != null)
        {
            // mainin suara
            audioSource.PlayOneShot(clickSound);
            // tunggu durasi suara pakai waktu real, bukan timeScale
            yield return new WaitForSecondsRealtime(clickSound.length);
        }

        // aktifin time lagi biar scene baru jalan normal
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        StartCoroutine(QuitAfterSound());
    }

    private IEnumerator QuitAfterSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSecondsRealtime(clickSound.length);
        }

        Debug.Log("Game Quit!");
        Application.Quit();
    }
}
