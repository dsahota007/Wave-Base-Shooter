using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public Text waveSurvivedText;
    public Text totalKillsText;

    void Start()
    {
         

        // ✅ Load the stats from PlayerPrefs
        int waveSurvived = PlayerPrefs.GetInt("WaveSurvived", 1); // Default 1
        int totalKills = PlayerPrefs.GetInt("TotalKills", 0); // Default 0

        // ✅ Update UI text elements
        if (waveSurvivedText != null)
            waveSurvivedText.text = $"You survived until Wave {waveSurvived}";

        if (totalKillsText != null)
            totalKillsText.text = $"Total Kills: {totalKills}";

        Debug.Log($"🛑 Death Screen Loaded | Wave: {waveSurvived} | Kills: {totalKills}");
    }

    public void ReturnToMainMenu()
    {
        MenuManager.gameStarted = false;
        SceneManager.LoadScene("MainMenu");
    }
}
