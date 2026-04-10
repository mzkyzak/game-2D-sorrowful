using UnityEngine;
using TMPro;
using System.Collections;

public class CreditScroll : MonoBehaviour
{
    public float scrollSpeed = 20f;       // kecepatan scroll
    public float startDelay = 1f;         // delay sebelum scroll mulai
    public TMP_Text creditText;           // taruh TMP text disini
    public AudioSource bgMusic;           // musik latar

    void Start()
    {
        if (bgMusic != null)
            bgMusic.Play();

        if (creditText != null)
            StartCoroutine(StartScrolling());
    }

    private IEnumerator StartScrolling()
    {
        yield return new WaitForSeconds(startDelay);

        while (creditText.transform.position.y < 2000f) // batas scroll, adjust sesuai kebutuhan
        {
            creditText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
