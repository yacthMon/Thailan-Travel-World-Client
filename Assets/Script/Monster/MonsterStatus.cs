using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour {
    // delegate
    public delegate void onMonsterDie();
    public delegate void onMonsterHurt(GameObject attacker);
    public delegate void onMonsterDestroy();
    public onMonsterHurt onMonsterHurtHandler = null;
    public onMonsterDie onMonsterDieHandler = null;
    public onMonsterDestroy onMonsterDestroyHandler = null;
    // delegate
    // Information
    private int ID; // ID of monster for control by server
    public int monsterID;
    public string monsterName;
    // Information
    // Status
    [SerializeField]
    private int monsterLevel, monsterHP, monsterMaxHP, monsterATK, monsterDEF, monsterSpeed;
    public bool isAlive = true;
    public bool isGodMode = false;
    public bool isBoss;
    // Status
    // Component
    public Animator anim;
    // Component
    // Variable
    int walkHash = Animator.StringToHash("Walk");
    int hurtHash = Animator.StringToHash("Hurt");
    int dieHash = Animator.StringToHash("Die");
    // Variable
    // voice
    AudioSource sfx;
    List<AudioClip> hurtSound = new List<AudioClip>();
    List<AudioClip> deathSound = new List<AudioClip>();
    // voice    
    void Start() {        
        // load Sound        
        sfx = this.gameObject.GetComponents<AudioSource>()[0];
        if (sfx) { 
            AudioClip[] files = Resources.LoadAll<AudioClip>("Sound/Monster Voice/"+monsterName);
            foreach (AudioClip a in files) {
                //Debug.Log("Sound : " + a.name);
                if (a.name.Contains("Hurt")) {
                    //Debug.Log("Add to hurt");
                    hurtSound.Add(a);
                } else if(a.name.Contains("Death")) {
                    //Debug.Log("Add to death");
                    deathSound.Add(a);
                }
            }
        }
    }

    public void SetID(int id) {
        this.ID = id;
    }

    public void SetMonsterSpeed(int speed) {
        this.monsterSpeed = speed;
    }

    public void AddInformation(int id, string name) {
        this.monsterID = id; this.monsterName = name;
    }

    public void AddStatus(int level,int hp,int maxHP , int atk, int def) {
        this.monsterLevel = level;this.monsterHP = hp; this.monsterMaxHP = maxHP; this.monsterATK = atk; this.monsterDEF = def;
    }

    public int GetMonsterLevel() {
        return this.monsterLevel;
    }

    public int GetMonsterSpeed() {
        return this.monsterSpeed;
    }

    public string GetMonsterName() {
        return this.monsterName;
    }

    public int GetMonsterATK() {
        return this.monsterATK;
    }

    public int GetMonsterMaxHP() {
        return this.monsterMaxHP;
    }

    public int GetMonsterHP() {
        return this.monsterHP;
    }

    public void getHurt(GameObject pgo) { // get hurt by player /// ABANDON (OFFLINE)
        if (isAlive) {            
            sfx.clip = getRandomHurtSound();            
            anim.SetBool(walkHash, false);
            if (isBoss) {                
                PlayerStatus ps = PlayerStatus.Instance;
                int damage = ps.getPlayerATK() - monsterDEF;
                monsterHP -= damage > 0 ? damage : 0;
                anim.SetTrigger(hurtHash);
                ActionTool.KnockBack(this.gameObject,pgo,new Vector2(100,100));
                if (monsterHP <= 0) {// Monster die here
                    sfx.clip = getRandomDeathSound();
                    isAlive = false;
                    anim.SetBool(dieHash , true);                    
                    ps.killedMonster(this.monsterID);
                    this.transform.GetChild(0).gameObject.SetActive(false);
                    if (onMonsterDieHandler != null) {
                        onMonsterDieHandler();
                    }
                    Invoke("destroyItSelf" , 3f);
                }
            }
            // Online
            int knockback = pgo.transform.position.x > this.transform.position.x ? -1 : 1;
            DGTMainController.Instance.RequestSendMonsterHurt(this.ID, knockback);
            if (onMonsterHurtHandler != null) {
                onMonsterHurtHandler(pgo);
            }
        }
        sfx.Play();
    }

    public void doHurt(int damage , int hpLeft , int knockback) { // hurt by other online player
        //show damage taken
        anim.SetBool(walkHash , false);
        anim.SetTrigger(hurtHash);
        this.monsterHP = hpLeft;
        ActionTool.KnockBack(this.gameObject , new Vector2(100*knockback,100));
    }

    public void Eliminate() {
        if(sfx)
            sfx.clip = getRandomDeathSound();
        isAlive = false;
        anim.SetBool(dieHash , true);        
        //ps.killedMonster(this.gameObject.GetComponent<MonsterStatus>());
        this.transform.GetChild(0).gameObject.SetActive(false);
        if (onMonsterDieHandler != null) {
            onMonsterDieHandler();
        }
        Invoke("destroyItSelf" , 3f);
    }

    private void destroyItSelf() {
        OnlineMonsterController.Instance.RemoveMonster(this.ID);
        if (onMonsterDestroyHandler != null)
            onMonsterDestroyHandler();
        Destroy(gameObject);
    }
    public AudioClip getRandomHurtSound() {
        AudioClip a = null;        
        if(hurtSound.Count>0)
        a = hurtSound[Random.Range(0, hurtSound.Count - 1)];
        return a;
    }
    public AudioClip getRandomDeathSound() {
        AudioClip a = null;
        if (deathSound.Count > 0)
            a = deathSound[Random.Range(0,deathSound.Count-1)];
        return a;
    }

}
