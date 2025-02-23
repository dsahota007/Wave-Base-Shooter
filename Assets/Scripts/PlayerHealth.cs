using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public float regenCooldown = 3f; // ✅ Time between health regen
    private float lastRegenTime; // ✅ Tracks last regen tick

    private bool isDead = false; // Prevent multiple deaths
    void Start()
    {
        currentHealth = maxHealth;
        FindObjectOfType<UI>().UpdateHealthBar(currentHealth, maxHealth);
        lastRegenTime = Time.time; // ✅ Start regen timer
    }

    void Update()
    {
        HandleRegeneration();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"⚠️ Player took {damage} damage! Health: {currentHealth}");

        FindObjectOfType<UI>().UpdateHealthBar(currentHealth, maxHealth);
        lastRegenTime = Time.time; // ✅ Reset regen timer

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void HandleRegeneration()
    {
        if (currentHealth < maxHealth && Time.time >= lastRegenTime + regenCooldown)
        {
            currentHealth += 1; // ✅ Restore exactly 1 HP
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            FindObjectOfType<UI>().UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log($"💚 Health regenerated! New health: {currentHealth}");
            lastRegenTime = Time.time; // ✅ Reset timer after regen
        }
    }

     

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("💀 Player Died! Saving stats & Fading to Black...");

        // ✅ Get stats from WaveSpawner & UI
        int waveSurvived = FindObjectOfType<WaveSpawner>().GetCurrentWave();
        int totalKills = FindObjectOfType<UI>().GetTotalKills();

        // ✅ Store in PlayerPrefs before switching scenes
        PlayerPrefs.SetInt("WaveSurvived", waveSurvived);
        PlayerPrefs.SetInt("TotalKills", totalKills);
        PlayerPrefs.Save(); // ✅ Force save the data

        // ✅ Disable player movement & shooting
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        PlayerShooting playerShooting = GetComponent<PlayerShooting>(); // If you have a shooting script

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerShooting != null) playerShooting.enabled = false;

        // ✅ Disable all UI elements (wave, kills, health bar)
        GameObject inGameUI = GameObject.Find("IngameUI");
        if (inGameUI != null)
        {
            inGameUI.SetActive(false);
        }

        // ✅ Make player flash white before fade
        StartFlashingEffect();

        // ✅ Call FadeToBlack (handled in DeathScreenUI)
        FindObjectOfType<DeathScreenUI>().FadeToBlack();
    }

    private void StartFlashingEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Debug.Log("⚡ Player Flashing White...");
            InvokeRepeating(nameof(FlashWhite), 0f, 0.1f);
            Invoke(nameof(StopFlashingEffect), 2.2f); // Stop flashing after 1.2 sec
        }
    }

    private void FlashWhite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = spriteRenderer.color == Color.white ? new Color(1, 1, 1, 0.2f) : Color.white; // Toggle between white & slightly transparent
        }
    }

    private void StopFlashingEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        CancelInvoke(nameof(FlashWhite)); // Stop flashing effect
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // Reset to normal
        }
    }

}
