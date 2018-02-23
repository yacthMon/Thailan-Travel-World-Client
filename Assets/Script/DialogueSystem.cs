using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour {
    public delegate void OnClickDialogue();
    public OnClickDialogue OnClickDialogueHandle;

    Vector2 screenPosition;
    public bool active { get; set; }
    public int type { get; set; }
    public string message { get; set; }
    public string acceptString { get; set; }
    public string declineString { get; set; }
    public GUIStyle style;
    public GameObject กัล { get; set; }
    public Button btnAccept;
    public Button btnDecline;
    public bool isDecide;
    public bool fade;
    public bool startFade;
    [SerializeField]
    int defaultFontSize;
    public static DialogueSystem Instance;

    void Start () {
        if (!Instance) {
            Instance = this;
        }
        screenPosition = new Vector2(Screen.width, Screen.height);
        hideDecide();
        defaultFontSize = style.fontSize;
    }
    public void setAutoClose() {
        btnAccept.onClick.AddListener(hideDecide);
        btnDecline.onClick.AddListener(hideDecide);
    }
    public void hideDecide() {
        isDecide = false;
        btnAccept.gameObject.SetActive(false);
        btnDecline.gameObject.SetActive(false);
    }
	public void showDecide() {
        isDecide = true;
        btnAccept.gameObject.SetActive(true);
        btnDecline.gameObject.SetActive(true);
    }
    public void showDecideWithDelay(float t) {
        isDecide = true;
        Invoke("showDecide", t);
    }
    private void OnGUI() {                
        if (active) {
            screenPosition = new Vector2(Screen.width, Screen.height);
            GUI.depth = -10;
            style.fontSize = Mathf.RoundToInt((float)(defaultFontSize * (Screen.width) / 1280)); ;
            style.wordWrap = true;
            //Dialogue Box
             GUI.Box(new Rect((float)(screenPosition.x / 4),
                 (float)(screenPosition.y * (3f / 4f)),
                 (float)(screenPosition.x / 2),
                 (float)(screenPosition.y / 4)),
                 message,style);
            //Button
            if (GUI.Button(new Rect((float)(screenPosition.x / 4) ,
                 (float)(screenPosition.y * (3f / 4f)) ,
                 (float)(screenPosition.x / 2) ,
                 (float)(screenPosition.y / 4)) , "" , GUIStyle.none)) {
                if (OnClickDialogueHandle!=null) {
                    OnClickDialogueHandle();
                }
            }
        }
    }
}
