using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Pengaturan Gerak")]
    public float moveForce = 15f;
    public float maxSpeed = 5f;
    public float jumpForce = 8f;
    public float linearDrag = 0.5f;

    [Header("Ukuran Karakter")]
    public float skalaAsli = 0.1922933f;

    [Header("UI System (TMP)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI livesText;

    [Header("Menus (Drag Panel ke Sini)")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    [Header("Statistik Karakter")]
    public int lives = 3;
    private int score = 0;
    private int highscore = 0;
    private bool isInvincible = false;
    private bool isPaused = false;

    private Animator anim; // Tambahkan ini

    private Rigidbody2D rb;
    private float inputHorizontal;
    private bool isGrounded;

    void Start()
    {
        // Pastikan waktu berjalan normal saat mulai
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.8f;
        rb.linearDamping = linearDrag;
        transform.localScale = new Vector2(skalaAsli, skalaAsli);
        highscore = PlayerPrefs.GetInt("HighScore", 0);

        // Pastikan panel mati saat start
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);

        anim = GetComponent<Animator>();

        UpdateUI();
    }

    void Update()
    {
        // Cek input Pause (Tombol ESC atau P)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (isPaused) return; // Jika pause, karakter tidak bisa gerak

        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (inputHorizontal > 0)
            transform.localScale = new Vector3(skalaAsli, skalaAsli, 1);
        else if (inputHorizontal < 0)
            transform.localScale = new Vector3(-skalaAsli, skalaAsli, 1);

        // KIRIM DATA KE ANIMATOR
        // Mathf.Abs agar nilainya selalu positif meski jalan ke kiri
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("Speed", Mathf.Abs(inputHorizontal));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed)
            rb.AddForce(new Vector2(inputHorizontal * moveForce, 0));
    }

    // --- LOGIKA MENU ---

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Menghentikan waktu game
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Menjalankan waktu kembali
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // WAJIB: Balikin waktu ke normal sebelum restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Pastikan nama scene menu sesuai
    }

    void ShowGameOver()
    {
        isPaused = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Freeze game
    }

    // --- LOGIKA NYAWA ---

    void TakeDamage()
    {
        lives -= 1;
        UpdateUI();

        if (lives <= 0) ShowGameOver(); // Panggil Game Over, bukan restart otomatis
        else StartCoroutine(BecomeInvincible());
    }

    // ... (Fungsi Jump, UpdateUI, OnTrigger, dll tetap sama seperti sebelumnya) ...
    // Pastikan di RestartLevel() diganti panggilannya ke ShowGameOver() jika perlu.

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (highScoreText != null) highScoreText.text = "Highscore: " + highscore;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(1.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        isInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Baterai"))
        {
            score += 1;
            if (score > highscore) { highscore = score; PlayerPrefs.SetInt("HighScore", highscore); }
            UpdateUI();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Mati") && !isInvincible)
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("DeadZone")) ShowGameOver(); // Jatuh jurang langsung Game Over
        if (other.CompareTag("Finish")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}