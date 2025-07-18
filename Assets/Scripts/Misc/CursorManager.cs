using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        Cursor.visible = false;

        if (Application.isPlaying)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void Update()
    {
        Vector2 cursorPos = Input.mousePosition;
        image.rectTransform.position = cursorPos;
        //if (!Application.isPlaying) { return; }

        //Cursor.visible = false;
    }
}
