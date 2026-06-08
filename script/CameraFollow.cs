using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Tarik objek Astronot ke sini nanti
    public float smoothing = 5f;
    public Vector3 offset = new Vector3(0, 0, -10); // Biar kamera nggak nabrak badan astronot

    void FixedUpdate()
    {
        if (target != null)
        {
            // Menentukan posisi tujuan (posisi astronot + jarak aman)
            Vector3 targetPosition = target.position + offset;

            // Gerakan kamera halus mengejar astronot
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }
}