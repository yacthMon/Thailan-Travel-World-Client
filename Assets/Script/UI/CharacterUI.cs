using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour {
    //Vector2 screenPosition; // ตำแหน่งบนหน้าจอ ณ จุดๆที่กำหนด
    public Vector2 offset; // ตำแหน่งที่ช่วยให้ UI สามารถแสดงผลได้ตรง
    //public Vector2 lblSize; // ขนาดของ Text
    //public Vector2 boxSize; // ขนาดของ Box (BG)
    public string targetName; // ข้อความ Text
    //public GUIStyle style; // Style ของ UI
    public bool player; // เป็น Player 
    public bool monster; // เป็น Monster 
    public bool NPC; // เป็น NPC
    //public int UID; // เลข ID
    public bool levelOn; // โชว์ Level ไหม
    public bool HPOn;
    /*public int defaultFontSize;// ขนาด Font
    public int dept;*/    
    private RectTransform nameTag;
    private PlayerStatus ps;    
    private MonsterStatus ms;
    private GameObject nameTagGO;
    private Slider hpBar;
    private RectTransform hpTransform;
    private string content;
    private Vector2 size;

    void Start() {
        //defaultFontSize = style.fontSize;
        if (NPC) //ถ้าเป็น NPC ให้ targetName ดึงชื่อมาจาก NPCStatus
            targetName = GetComponent<NPCStatus>().NPCName;
        else {
            if (player) { // ถ้าเป็น Player ให้ targetName ดึงชื่อมาจาก PlayerStatus
                ps = GetComponent<PlayerStatus>();
                targetName = ps.playerName;
            } else if (monster) { // ถ้าเป็น Monster ให้ targetName ดึงชื่อมาจาก monterStatus
                ms = GetComponent<MonsterStatus>();
                targetName = ms.monsterName;
            }
        }
        //OnEnable();
        nameTagGO = Instantiate(Resources.Load<GameObject>("UI/Prefab/NameTag"));
        nameTag = nameTagGO.GetComponent<RectTransform>();
        nameTagGO.transform.SetParent(GameObject.Find("NameTags").transform , false);
        content = (levelOn ? player ?
            "lv. " + ps.level : "lv. " + ms.GetMonsterLevel()
            : "") + " " + targetName;
        if (HPOn && !NPC) {
            GameObject hpBarGO = Instantiate(Resources.Load<GameObject>("UI/Prefab/HPBar"));
            hpBarGO.transform.SetParent(nameTagGO.transform, false);
            hpTransform = hpBarGO.GetComponent<RectTransform>();
            hpBar = hpBarGO.GetComponent<Slider>();
            hpTransform.sizeDelta = new Vector2(120 , 18);
        }

        GUIContent s = new GUIContent(content);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 14;
        // Compute how large the UI needs to be.
        size = style.CalcSize(s);
        
    }
    /*
    void FixedUpdate() {
        if (player) {
            screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            screenPosition.y = Screen.height - screenPosition.y;
        }
    }
    void Update() {
        if (!player) {
            screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            screenPosition.y = Screen.height - screenPosition.y;
        }
    }
    */
    private void OnGUI() {         
        float offsetSize = 1.3f; // so UI will not fit with content (add margin)
        nameTag.sizeDelta = new Vector2(size.x * offsetSize , size.y * offsetSize);
        nameTag.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(size.x , size.y * offsetSize);
        nameTag.transform.GetChild(0).GetComponent<Text>().text = content;
        nameTag.position = (Vector2)this.transform.position + offset;                
        if (HPOn && !NPC) {
            hpTransform.position = (Vector2)this.transform.position + offset;
            if (ps || ms) {
                hpBar.maxValue = player ? ps.maxHP : ms.GetMonsterMaxHP();
                hpBar.value = player ? ps.playerHP : ms.GetMonsterHP();
            }

        }
    }

    private void OnDestroy() {
        Destroy(nameTagGO);
    }

    private void OnDisable() {
        if (nameTagGO)
            nameTagGO.SetActive(false);
    }

    private void OnEnable() {
        if(nameTagGO)
            nameTagGO.SetActive(true);
    }


    public void OldNameTag() {/*
        string content = (levelOn ? player ?
            "lv. " + GetComponent<PlayerStatus>().level : "lv. " + GetComponent<MonsterStatus>().monsterLevel
            : "") + " " + targetName;
        GUIContent s = new GUIContent(content);
        style = GUI.skin.box;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 14;
        style.fontSize = Mathf.RoundToInt((float)(defaultFontSize * (Screen.width) / 1280)); //Responsive Size
        // Compute how large the UI needs to be.
        Vector2 size = style.CalcSize(s);
        float sizex = 1.2f * size.x; // add margin
        float sizey = 1.1f * size.y; // add margin
        GUI.Box(new Rect(screenPosition.x -(sizex / 2),
            screenPosition.y + Mathf.RoundToInt((float)(Offset.y * (Screen.width) / 1280)),
            sizex, sizey), content,style);
            */
    }
}
