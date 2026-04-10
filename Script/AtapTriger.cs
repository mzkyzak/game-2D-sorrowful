using UnityEngine;
using UnityEngine.Tilemaps;

public class AtapTrigger : MonoBehaviour
{
    [Header("Tilemap atap yang mau di-fade")]
    public TilemapRenderer tilemapRenderer;
    [Header("Kecepatan fade (semakin tinggi, makin cepat)")]
    public float fadeSpeed = 5f;

    private bool playerInside = false;
    private Color targetColor;
    private Color currentColor;

    void Start()
    {
        if (tilemapRenderer == null)
        {
            Debug.LogWarning("TilemapRenderer belum diisi di " + gameObject.name);
        }
    }

    void Update()
    {
        if (tilemapRenderer == null) return;

        
        targetColor = playerInside ? new Color(1, 1, 1, 0.3f) : new Color(1, 1, 1, 1f);

       
        currentColor = tilemapRenderer.material.color;

     
        tilemapRenderer.material.color = Color.Lerp(currentColor, targetColor, Time.deltaTime * fadeSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
