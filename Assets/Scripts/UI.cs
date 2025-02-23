using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Image healthBarFill; // ✅ Drag the HealthBarFill image from Canvas into this slot
    public Text waveNumberText;
    public Text enemiesLeftText;
    public Text totalKillsText;

    private int totalKills = 0;
    private int enemiesLeft = 0;
    private int waveNumber = 1;


    void Start()
    {
        UpdateUI();
    }

    public void SetWaveNumber(int wave)
    {
        waveNumber = wave;
        UpdateUI();
    }

    public void SetEnemiesLeft(int count)
    {
        enemiesLeft = count;
        UpdateUI();
    }

    public void IncreaseKillCount()
    {
        totalKills++;
        UpdateUI();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
            Debug.Log($"🟢 Health Bar Updated: {currentHealth}/{maxHealth} = {healthBarFill.fillAmount}");
        }
        else
        {
            Debug.LogError("❌ HealthBarFill is NULL! Make sure it's assigned in the UI.");
        }
    }

    private void UpdateUI()
    {
        if (waveNumberText != null) waveNumberText.text = $"Wave - {waveNumber}";
        if (enemiesLeftText != null) enemiesLeftText.text = $"Alive - {enemiesLeft}";
        if (totalKillsText != null) totalKillsText.text = $"{totalKills}";
    }
    public int GetTotalKills()
    {
        return totalKills;  // ✅ Return total kills for the death screen
    }

}
