using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Fungsi untuk pindah level berdasarkan nama scene
    public void PindahLevel(string namaLevel)
    {
        // Selalu pastikan waktu berjalan normal (1) sebelum pindah
        // Biar kalau pindah dari menu Pause, game-nya nggak macet
        Time.timeScale = 1f;

        SceneManager.LoadScene(namaLevel);
    }

    // Fungsi tambahan untuk tombol Quit
    public void KeluarGame()
    {
        Debug.Log("Game Keluar...");
        Application.Quit();
    }
}