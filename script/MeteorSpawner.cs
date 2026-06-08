using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // Masukkan prefab meteor di sini
    public float intervalMuncul = 2f; // Muncul setiap 2 detik
    public float rentangX = 10f; // Seberapa lebar area jatuhnya

    void Start()
    {
        // Memanggil fungsi MunculkanMeteor secara berulang
        InvokeRepeating("MunculkanMeteor", 1f, intervalMuncul);
    }

    void MunculkanMeteor()
    {
        // Tentukan posisi acak di sumbu X
        float randomX = Random.Range(transform.position.x - rentangX, transform.position.x + rentangX);
        Vector3 posisiMuncul = new Vector3(randomX, transform.position.y, 0);

        // Munculkan meteornya!
        Instantiate(meteorPrefab, posisiMuncul, Quaternion.identity);
    }
}