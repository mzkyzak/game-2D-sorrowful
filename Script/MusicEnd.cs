using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditMusicController : MonoBehaviour
{
    [Header("Main Music")]
    public AudioSource mainMusic;          // Musik utama credit scene
    public string nextSceneName = "MainMenu"; // Scene tujuan setelah musik selesai

    [Header("Timed SFX")]
    public AudioSource sfxSource;          // AudioSource untuk efek tambahan
    public AudioClip[] sfxClips;           // List SFX yang ingin dimainkan
    public float[] triggerTimes;           // Waktu (detik) kapan tiap SFX muncul

    private bool[] sfxPlayed;              // Penanda SFX yang sudah dimainkan

    void Start()
    {
        // Cek setup dasar
        if (mainMusic == null)
        {
            Debug.LogWarning("Main music belum diset di Inspector!");
            return;
        }

        // Inisialisasi flag
        sfxPlayed = new bool[sfxClips.Length];

        // Mainkan musik utama
        mainMusic.Play();
    }

    void Update()
    {
        if (mainMusic == null) return;

        float currentTime = mainMusic.time;

        // 🔊 Cek dan mainkan SFX di waktu tertentu
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (!sfxPlayed[i] && currentTime >= triggerTimes[i])
            {
                if (sfxSource != null && sfxClips[i] != null)
                {
                    sfxSource.PlayOneShot(sfxClips[i]);
                    sfxPlayed[i] = true;
                }
            }
        }

        // 🎬 Ganti scene setelah musik selesai
        if (!mainMusic.isPlaying && mainMusic.time > 0f)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
