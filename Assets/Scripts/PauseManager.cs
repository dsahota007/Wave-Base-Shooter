using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; //  Drag PauseMenu UI here in Inspector
    private bool isPaused = false;
    private bool isMusicPaused = false;
    private AudioSource audioSource; // 🎵 Reference to AudioManager's AudioSource

    void Start()
    {
        // Get AudioSource from AudioManager
        audioSource = FindObjectOfType<AudioManager>().GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //  Press ESC to toggle pause
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); //  Show pause menu
        Time.timeScale = 0f; //  Stop the game
        isPaused = true;
        FindObjectOfType<MouseCursorManager>().SetMenuCursor();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); //  Hide pause menu
        Time.timeScale = 1f; //  Resume game
        isPaused = false;
        FindObjectOfType<MouseCursorManager>().SetGameCursor();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; //  Ensure the game isn't paused
        MenuManager.gameStarted = false;
        SceneManager.LoadScene("MainMenu");
    }

    // 🎵 Toggle Music Button Function
    public void ToggleMusic()
    {
        if (audioSource == null) return; // Safety check

        isMusicPaused = !isMusicPaused;

        if (isMusicPaused)
            audioSource.Pause(); // 🔇 Pause music
        else
            audioSource.Play();  // 🔊 Resume music
    }
}
