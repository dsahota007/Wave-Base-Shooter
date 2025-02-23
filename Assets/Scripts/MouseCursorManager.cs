using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D gameCursor;
    [SerializeField] private Texture2D menuCursor;

    private bool isGameCursorActive = false; // ✅ Tracks if cursor is set

    private void Start()
    {
        SetMenuCursor(); // ✅ Default to menu cursor
    }

    private void Update()
    {
        if (MenuManager.gameStarted && !isGameCursorActive)
        {
            SetGameCursor();
            isGameCursorActive = true; // ✅ Prevent redundant calls
        }
        else if (!MenuManager.gameStarted && isGameCursorActive)
        {
            SetMenuCursor();
            isGameCursorActive = false;
        }
    }

    public void SetGameCursor()
    {
        Cursor.SetCursor(gameCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetMenuCursor()
    {
        Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.Auto);
    }
}
