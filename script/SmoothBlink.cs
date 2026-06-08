using UnityEngine;
using TMPro;

public class SmoothBlink : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public float speed = 2.0f; // Kecepatan halus

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Rumus matematika untuk bikin nilai naik turun halus antara 0 dan 1
        float alpha = (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f;

        // Pasang nilai alpha itu ke warna teks
        Color c = textComponent.color;
        c.a = alpha;
        textComponent.color = c;
    }
}