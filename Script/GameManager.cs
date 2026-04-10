using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject panelPause;
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PauseButton()
    {
        PlayClickSound();
        panelPause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        PlayClickSound();
        panelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
