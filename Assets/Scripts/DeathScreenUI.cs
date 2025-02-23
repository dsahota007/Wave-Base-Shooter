using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour
{
    public Image fadeImage;  // The fade effect image
    public GameObject inGameUI; 

    private bool startFade = false;
    private float fadeSpeed = 0.5f;
    private float alpha = 0;
    private float fadeTimer = 0;
    private bool sceneChangeTriggered = false;

    void Start()
    {
        Debug.Log("✅ DeathScreenUI Initialized!");

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0); // Start transparent
        }
        else
        {
            Debug.LogError("❌ ERROR: `fadeImage` is NULL! Assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (startFade)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));

            if (alpha >= 1)
            {
                fadeTimer += Time.deltaTime; // Start countdown after fade

                if (fadeTimer >= 2f && !sceneChangeTriggered)  // ✅ Change scene after 2 sec
                {
                    sceneChangeTriggered = true;  // Ensure it happens only once
                    SceneManager.LoadScene("DeathScreen");
                    FindObjectOfType<MouseCursorManager>().SetMenuCursor();
                }
            }
        }
    }

    public void FadeToBlack()
    {
        Debug.Log("🚨 Starting Fade to Black!");

        // ✅ Hide the in-game UI when fade starts
        if (inGameUI != null)
        {
            inGameUI.SetActive(false);
            Debug.Log("🛑 In-Game UI Disabled");
        }
        else
        {
            Debug.LogWarning("⚠️ `inGameUI` is NOT assigned in the Inspector!");
        }

        startFade = true;
    }
}
