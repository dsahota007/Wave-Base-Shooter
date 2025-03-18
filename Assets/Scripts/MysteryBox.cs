using TMPro;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    public TextMeshPro interactionText;

    public SpriteRenderer boxSpriteRenderer;
    public Sprite closedBoxSprite;
    public Sprite openBoxSprite;

    private bool isPlayerNearby = false;
    private bool isBoxOpened = false;
    private float resetTimer = 10f; // 10 seconds until reset

    private void Start()
    {
        // ✅ Hide text at the beginning
        interactionText.gameObject.SetActive(false);

        // ✅ Set starting sprite to closed box
        boxSpriteRenderer.sprite = closedBoxSprite;
    }

    private void Update()
    {
        // ✅ Check if player presses "E" when near the box
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isBoxOpened)
        {
            OpenBox();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isBoxOpened)
        {
            isPlayerNearby = true;
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionText.gameObject.SetActive(false); // ✅ Always hide text on exit
        }
    }

    void OpenBox()
    {
        Debug.Log("Mystery Box Opened!");

        isBoxOpened = true;

        // ✅ Switch to open sprite
        boxSpriteRenderer.sprite = openBoxSprite;

        // ✅ Hide the interaction text COMPLETELY
        interactionText.gameObject.SetActive(false);

        // ✅ Start reset timer
        Invoke(nameof(ResetBox), resetTimer);
    }

    void ResetBox()
    {
        Debug.Log("Mystery Box Reset!");

        isBoxOpened = false;

        // ✅ Switch back to closed sprite
        boxSpriteRenderer.sprite = closedBoxSprite;

        // ✅ Text remains hidden unless the player re-enters the collider
    }
}
