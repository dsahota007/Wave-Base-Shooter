using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gameStarted = false; // ✅ Tracks game state

    public void StartGame()
    {
        gameStarted = true; // ✅ Mark game as started
        SceneManager.LoadScene("MainScene");
    }
    public void BackToMenu()
    {
        gameStarted = false;  
        SceneManager.LoadScene("MainMenu");
    }
    public void HowTo()
    {
        gameStarted = false;  
        SceneManager.LoadScene("HowToScene");
    }

}
