using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeInformation : MonoBehaviour {
    Vector2 screenPosition;
    public bool active { get; set; }
    public string message { get; set; }

	void Update () {
        screenPosition = new Vector2(Screen.width, Screen.height);
    }
    private void OnGUI()
    {
        if (active)
        {
            GUI.TextArea(new Rect((float)(screenPosition.x * (1f / 4f)),
                (float)(screenPosition.y * (1f / 8f)),
                (float)(screenPosition.x * (1f / 2f)), 
                (float)(screenPosition.y * (3f / 8f))), message);
        }
    }
}
