using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    private void Awake()
    {
        // Set hardware cursor off
        // Cursor.visible = false;
    }

    private void Update()
    {
        Camera.main.transform.position = Input.mousePosition;
    }

}
