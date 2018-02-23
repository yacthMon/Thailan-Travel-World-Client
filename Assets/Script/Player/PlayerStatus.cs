using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class PlayerStatus : MonoBehaviour {
    //Singleton
    public static PlayerStatus Instance;
    //Singleton
    // Delegate
    public delegate void onKillMonster(int monsterID);
    public onKillMonster onKillMonsterHandler;
    public delegate void onPlayerDie();
    public onPlayerDie onPlayerDieHandler;
    public delegate void onStatusChange();
    public onStatusChange onStatusChangeHandler;
    // Delegate
    // Player Infomation
    public string playerName;
    public int playerID;
    public string gender;
    public string job;
    // Player Information
    // Stats
    public int playerHP;
    public int maxHP;
    public int playerSP;
    public int maxSP;
    public int level;
    public int atk;
    public int def;
    public int playerEXP;
    public int maxEXP;
    public bool isAlive; 
    public bool isGodMode=false;
    public bool isPlayer;
    private bool isOnlineOtherPlayer;
    public string currentMap;
    // Stats 
    // Collection Place
    
    private List<PlaceData> placeList = new List<PlaceData>();
    //
    // Component
    public Animator anim;
    // Component
    // UI
    public GameObject uiRespawn;
    public Slider hpSlider;
    public Text hpText;
    public Slider spSlider;
    public Text spText;
    public Slider expSlider;
    public Text expText;
    public Text levelText;
    // UI
    // Equipment
    public string head,body,weapon;
    // Equipment
    // Variable
    int hurtHash = Animator.StringToHash("Hurt");
    int dieHash = Animator.StringToHash("Die");
    int respawnHash = Animator.StringToHash("Respawn");
    public bool talking;
    // Variable

    void Start() {        
        if(Instance == null)
            Instance = this;
        isAlive = true;
        if (isPlayer) { // เซ็ท Status ของ Player ให้ UI
            levelText.text = "Level : " + level + "  " + playerName ;
            expSlider.maxValue = maxEXP;
            expSlider.value = playerEXP;
            expText.text = playerEXP + " / " + maxEXP;
            hpSlider.maxValue = maxHP;
            hpSlider.value = playerHP;
            hpText.text = playerHP + " / " + maxHP;
            spSlider.maxValue = maxSP;
            spSlider.value = playerSP;
            spText.text = playerSP + " / " + maxSP;
        }
        onStatusChangeHandler += updateUI;
        CheckDeath();
    }
    
    //Override
    private void OnTriggerStay2D(Collider2D col) {
        if (col.CompareTag("MonsterOffset") && isAlive && isGodMode == false) {
            // Attacked by MonsterOffset
            MonsterStatus ms = col.GetComponentInParent<MonsterStatus>();
            if (ms.isAlive){
                hurt(ms.GetMonsterATK());
                GetComponent<Control>().delayAttack();
                ActionTool.KnockBack(this.gameObject, col.transform.parent.gameObject, new Vector2(200, 200));
            }
        }
    }

    //Override
    public int getPlayerATK() {
        return this.atk;
    }

    public void updateUI() {
        // ใช้สำหรับ Update UI ให้มีค่าเท่ากับ Status ปัจจุบัน
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;        
        hpText.text = playerHP + " / " + maxHP;
        spSlider.maxValue = maxSP;
        spSlider.value = playerSP;        
        spText.text = playerSP + " / " + maxSP;
        expSlider.maxValue = maxEXP;
        expSlider.value = playerEXP;        
        expText.text = playerEXP + " / " + maxEXP;
        levelText.text = "Level : " + level + "  " + playerName;
    }

    public void killedMonster(int monsterID) {
        if (onKillMonsterHandler != null)
            onKillMonsterHandler(monsterID);
        else {
            Debug.Log("On kill monster Event is null");
        }
    }

    public static void copyStatus(PlayerStatus ps1, PlayerStatus ps2) {
        ps1.level = ps2.level;
        ps1.maxHP = ps2.maxHP;
        ps1.maxSP = ps2.maxSP;
        ps1.maxEXP = ps2.maxEXP;
        ps1.playerEXP = ps2.playerEXP;
        ps1.atk = ps2.atk;
        ps1.def = ps2.def;
    }

    public void startGodMode() {
        isGodMode = true;
        Invoke("endGodMode", 2.5f);
    }

    public void endGodMode() {
        isGodMode = false;
    }

    public void hurt(int damage) {
        if (isGodMode == false) { // ถ้าเกิดไม่ใช่ GodMode
            damage = damage - def; // คำนวณ Damage ที่เข้ามาตรงๆ
            // คำนวณ Damage แก้บัค Damage เราติดลบแล้วไปฮีลให้ Monster    
            playerHP -= damage > 0 ? damage : 1;
            // ข้างบน ^ และ ข้างล่าง v มีความหมายเหมือนกันน๊ะ !!
            // playerHP = playerHP - damage;
            /* damage > 0 ? damage : 0 คืออะไร ?? มันคือ 
                if (damage > 0) { return damage; } else { return 0; } 
                มันเป้นการใช้ Short if คือเขียน if สั้นๆ ณ ตรงนี้ ถ้า damage มากกว่า 0 damage จะมีค่าเท่าเดิม แต่ถ้า Damage น้อยกว่า 0 (ติดลบ)
                Damage จะมีค่าเป็น 0 ไม่งั้นถ้า Damage ติดลบ ไปลบกับเลือดของ Monster จะกลายเป็นว่า ไปฮีลให้ Monster นะ
             */
            if (playerHP < 0) playerHP = 0;  // ถ้า playerHP หลังจากคำนวณแล้วน้อยกว่า 0 ก็ให้เซ็ทเป็น 0 เพื่อไม่ให้ติดลบ          
            //updateUI(); // สั่งให้ UpdateUI OLD : ใส่ updateUI ใน onStatusChangeHandler แล้ว
            CheckDeath(); // เรียกใช้ CheckDeath() เพื่อเช็คว่า player จะตายไหม
            startGodMode(); // เร่มใช้งาน godMode (เพื่อไม่ให้รับ Damage ชั่วคราว)
            anim.SetTrigger(hurtHash); //ให้เล่น animation hurt
            if (onStatusChangeHandler != null)
                onStatusChangeHandler();
        }
    }

    private void CheckDeath() {
        if (playerHP <= 0) {// Player die here
            isAlive = false;
            GetComponent<Control>().setControlable(false);
            anim.SetBool(dieHash, true);
            Invoke("showRespawnButton", 4);
            if (onPlayerDieHandler != null)
                onPlayerDieHandler();
        }
    }

    public void showRespawnButton() {
        uiRespawn.gameObject.SetActive(true);
    }

    public void getEXP(int exp) {
        playerEXP = playerEXP + exp;
        expText.text = playerEXP + " / " + maxEXP;
        expSlider.value = playerEXP;
        if (playerEXP >= maxEXP) 
            levelUp();
        if (onStatusChangeHandler != null)
            onStatusChangeHandler();
    }

    public void AddHP(int amount) {
        playerHP += amount + playerHP >= maxHP ? maxHP - playerHP : amount;
        if (onStatusChangeHandler != null)
            onStatusChangeHandler();
    }

	public void levelUp(){
                	
        level = level + 1; // up level
		maxHP = maxHP + 20; // increase maxHP
		playerHP = maxHP; // recover HP
		hpSlider.maxValue = maxHP;// resize HP Bar
		maxSP = maxSP + 10;// increase maxSP
		playerSP = maxSP;// recovery SP
		spSlider.maxValue = maxSP;// resize SP Bar
		atk = atk + 5; // increase Attack
		def = def + 3; // increase Defense
        // -- upgrade status
        int expRemain = playerEXP - maxEXP; // min EXP total with last maxEXP
        //expRemain = expRemain <= 0 ? 0 : expRemain; // if expRemain < 0 then expRemain should be 0 or not change
        if(expRemain <= 0) {
            expRemain = 0;
        }
        playerEXP = 0; // set Player EXP back to zero       
        maxEXP = maxEXP + 50; // increase maxEXP
        expSlider.maxValue = maxEXP;// resize EXP Bar
        getEXP(expRemain); // get EXP Again from EXP Remain        
        if(onStatusChangeHandler!=null)
            onStatusChangeHandler();        
        Debug.Log("Levelup");
    }

    public void recoverStatus() {
        playerHP = maxHP;
        playerSP = maxSP;
        //updateUI(); // OLD ใส่ใน status change handler แล้ว
        if (onStatusChangeHandler != null)
            onStatusChangeHandler();
    }

    public void doRespawn() {
        if (!isAlive) {
            recoverStatus();
            anim.SetBool(dieHash, false);
            anim.SetTrigger(respawnHash);            
            GetComponent<Control>().setControlable(true);
            isAlive = true;
            anim.ResetTrigger(respawnHash);
        }
    }

    public void updateLayer(int customSort) {
        //change layer used [3 layer] layer 0 : soul,body | layer 1 : cloth,shoe | layer 2 : hair, face, weapon
        Debug.Log("Sort : " + customSort);
        foreach (Transform spritePart in this.transform.GetChild(0)){
            SpriteRenderer sr = spritePart.GetComponent<SpriteRenderer>();
            if (sr != null) {
                sr.sortingOrder += (customSort*3);
            }
        }

    }

    public void AddPlaceToList(PlaceData data) {
        placeList.Add(data);
    }

    public List<PlaceData> GetPlaceList(string city) {
        // Remember this error place is empty because placeList was made to SerializeField
        return placeList.FindAll(place => place.city == city);
    }
    
    public bool CheckExistCheckin(int placeID) {
        return placeList.Exists(place => place.placeID == placeID);
    }

    public void SetToOnlinePlayer() {
        this.isOnlineOtherPlayer = true;
    }

    public bool IsOnlineOtherPlayer() {
        return this.isOnlineOtherPlayer;
    }
}