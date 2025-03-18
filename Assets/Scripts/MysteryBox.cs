using UnityEngine;
using TMPro;

public class MysteryBox : MonoBehaviour
{
    public TextMeshProUGUI interactionText; // Assign the text in the Inspector
    public float interactionDistance = 2f;
    private bool isPlayerNearby = false;

    private void Start()
    {
        interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            // Show text when the player is near
            interactionText.gameObject.SetActive(true);

            // ✅ Open box when player presses 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenBox();
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void OpenBox()
    {
        Debug.Log("Mystery Box Opened!");
        // ✅ Add opening logic here (like giving player a reward)
    }
}
