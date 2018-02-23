using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterControl : MonoBehaviour { // Abandon
    // Delegate
    public delegate void onMonsterReach();
    public delegate void onMonsterStopWalking();
    public delegate void onMonsterHitBlock();
    public onMonsterReach onMonsterReachHandler = null;
    public onMonsterStopWalking onMonsterStopWalkingHandler = null;
    public onMonsterHitBlock onMonsterHitBlockHandler = null;
    // Delegate
    public int speed;
    bool turnLeft=true;
    public bool following = false;
    bool isWalkingTo = false;
    GameObject target;
    Vector2 targetPos;
    // Component
    Animator anim;
    MonsterStatus ms;
    // Component
    // Variable    
    int walkHash = Animator.StringToHash("Walk");
    int hurtHash = Animator.StringToHash("Hurt");
    int dieHash = Animator.StringToHash("Die");    
    // Variable
    // Use this for initialization
    void Start () {
        ms = gameObject.GetComponent<MonsterStatus>();
        anim = this.GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        if (this.GetComponent<MonsterStatus>().isAlive) {// monster ต้องยังไม่ตาย
            if (!following) { // ไม่ได้กำลัง Following
                if (isWalkingTo) { // กำลังเดินอย่างมีจุดหมา
                    int direction = findDirect(); //หาทิศทางระหว่าง monster กับ เป้าหมาย
                    if (distance() <= 1) { // ถ้า ระยะห่างจากเป้าหมายน้อยกว่าหรือเท่ากับ 1
                        // reach to target point;
                        isWalkingTo = false; // set isWalkingTo ให้เป็น false
                        anim.SetBool(walkHash, false); // set walkHash ให้เป็น false
                        if (onMonsterReachHandler != null)
                            onMonsterReachHandler(); // สั่ง onMonsterReachHandler ให้ทำงาน
                    } else {
                        // walk to target point;
                        // สร้างเส้นสีน้ำเงินจาก ตำแหน่ง monster ไปยังจุดที่จะเดินไป
                        Debug.DrawLine(this.transform.position, targetPos,Color.blue);                        
                        anim.SetBool(walkHash, true); //set walkHash ให้เป็น true
                        walk(direction); // สั่งให้ monster เดินไปยังทิศทางของ target
                    }
                }
            } else { // ถ้าเกิด Following อยู่
                if (PlayerStatus.Instance.isAlive) { // ถ้า player ยังไม่ตาย
                    targetPos = target.transform.position; // หาตำแหน่ง ของ target
                    int direction = findDirect(); // หาทิศทางระหว่าง monster กับ target
                    if (distance() <= 3) { 
                        // monster เข้าใกล้ในระยะ 3 ช่อง 
                        following = false; 
                        move(5 * direction); // เดินไปยิงทิศทางของ Target 5 ช่อง
                        anim.SetBool(walkHash, false);
                        // กลับสู่โหมด move ปกติ ไม่ต้อง Continue เพื่อให้เดินชนไปข้างหน้า
                        onMonsterReachHandler += followContinue;
                        // เพิ่ม Follow Continue ซึ่งทำให้กลับมา follow Target ใหม่อีกครั้งหลังจากเดินไปถึงจุดเป้าหมายแล้ว
                    } else {
                        // walk to target point เดินไปหา target
                        // สร้างเส้นสีน้ำเงินจาก ตำแหน่ง monster ไปยังจุดที่จะเดินไป
                        Debug.DrawRay(this.transform.position,targetPos);
                        anim.SetBool(walkHash, true); //set walkHash ให้เป็น true
                        walk(direction);// สั่งให้ monster เดินไปยังทิศทางของ target
                    }
                } else { // Player die :( แงงงง ตายเลยยยย TT^TT
                    stopWalking(); // สั่งให้หยุดเดิน
                }
            }          
        } else { // Monster Die
            anim.SetBool(dieHash, true);
        }
    }

    public void walkTo(Vector2 targetPos) {
        this.targetPos = targetPos;
        isWalkingTo = true;
    }
    public void follow(GameObject target) {
        this.target = target;
        this.targetPos = target.transform.position;
        following = true;
    }
    public void followContinue() {
        following = true;
        onMonsterReachHandler -= followContinue;
    }
    public void move(float x) {
        walkTo(new Vector2(this.transform.position.x + x, 0));
    }
    public void moveBack(float x) {
        walkTo(new Vector2(this.transform.position.x + (x * -findDirect()), 0));
    }
    public void attack(){
        //int damage = GetComponent<PlayerStatus>().getPlayerATK();
        //GameObject.Find("Effect_Attack").SendMessage("reAttacked");        
    }
    public void walk(float direct){
        if (direct >= 0) {
            if (turnLeft) {
                turnLeft = false;
                this.transform.localScale = reDirection();
            }
            this.transform.Translate(new Vector2(speed, 0) * Time.deltaTime);
        } else {
            if (!turnLeft) {
                turnLeft = true;
                this.transform.localScale = reDirection();
            }
            this.transform.Translate(new Vector2(-speed, 0) * Time.deltaTime);
        }
    }
    public void jump(float jumpPower){
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpPower), ForceMode2D.Force);       
        //weaponAnim.SetTrigger(Animator.StringToHash("Jump"));
    }
    public void stopWalking() {
        isWalkingTo = false;
        following = false;
        onMonsterReachHandler -= followContinue;
        if (onMonsterStopWalkingHandler != null)
            onMonsterStopWalkingHandler();
    }
    private void OnTriggerEnter2D(Collider2D col) {
        hitBlock(col);
    }
    void hitBlock(Collider2D col) {
        ms = gameObject.GetComponent<MonsterStatus>();
        if (col.CompareTag("Block_Monster")) {
                moveBack(distance());
                following = false;
                if (onMonsterHitBlockHandler != null)
                    onMonsterHitBlockHandler();
        }
    }
    float distance() {
        return Mathf.Abs(this.transform.position.x - targetPos.x);
    }
    Vector2 reDirection() {
        return new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
    }
    int findDirect() {
        int direction;
        if (transform.position.x > targetPos.x) {
            direction = -1;
        } else {
            direction = 1;
        }
        return direction;
    }
}
