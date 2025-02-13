using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    Texture2D cursor1;

    [SerializeField]
    Texture2D cursor2;

    private void Update()
    {
        Vector2 cursorHotspot = new Vector2(10, 10);

        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursor1, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }

}
