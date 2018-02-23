using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour {

    float deltaTime = 0.0f;
    public string customText;
    public string gpsText = "GPS Text";
    public static fps Instance;
    void Update() {
        if (Instance == null) {
            Instance = this;
        }
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI() {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps) " + Screen.width+"x"+ Screen.height + " " + customText, msec, fps);
        GUI.Label(rect, text, style);
        GUI.Label(new Rect(w/2, 0, w, h * 2 / 100), gpsText, style);

    }
}
