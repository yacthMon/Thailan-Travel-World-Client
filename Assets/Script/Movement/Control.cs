using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class Control : MonoBehaviour {
    Animator anim;
    AnimatorStateInfo stateInfo;
    
    // Animator
    int jumpHash = Animator.StringToHash("Jump");
    int walkHash = Animator.StringToHash("Walk");
    int attackHash = Animator.StringToHash("Attack");
    int onGround = Animator.StringToHash("OnGround");
    // Animator
    // Component
    // Component
    // Custom Script
    PlayerStatus pS;
    // Custom Script
    // Setting
    public int speed;
    public int jumpPower;
    public CheckFloor ck;
    public GameObject PlayerProps;
    // Setting
    bool turnLeft = true;
    public bool controlable;
    bool cooldownJump = false;
    public GameObject platform_Stand;
    public bool platformActive = true;
    public static Control Instance;
    void Start() {
        if (!Instance)
            Instance = this;
        anim = PlayerProps.GetComponent<Animator>();
        foreach(Transform child in this.transform) {
            if(child.name == "Platform_Stand") {
                platform_Stand = child.gameObject;
                return;
            }
        }
    }
    
    // Update is called once per frame
    void Update() {
        if (controlable) { // เช็คว่า สามารถ control ได้ไหม
            if (!cooldownJump) //ถ้ายังไม่มี cooldownJump
                if (ck.isOnFloor)// เช็คว่ายืนอยู่บนพื้นรึเปล่า
                    anim.SetBool(onGround, true); // ถ้าใช่ให้ set parameter onGround ให้เป็น true (หากเล่น animation Falling อยู่ จะกลับมาเป็น idle)
                else if (!ck.isOnFloor)
                    anim.SetBool(onGround, false);// ถ้าใม่ใช่ set parameter onGround ให้เป็น false (เป็นท่า Falling กำลังตก)

            if (CnInputManager.GetButtonDown("Attack")) { // เช็คว่ากดปุ่ม Attack รึเปล่า
                attack(); // ทำการ Attack
            }
            stateInfo = anim.GetCurrentAnimatorStateInfo(0); // ดึง Animation ที่เล่นอยู่ ณ เวลาปัจจุบัน
            float vertical = CnInputManager.GetAxis("Vertical"); // Added New
            if(vertical < -0.5f) {                
                if (CnInputManager.GetButtonDown("Jump") && platformActive) {
                    platformActive = false;
                    DisablePlatformStand();
                }
            } else {
                if (!platformActive) {
                    EnablePlatformStand();
                    platformActive = true;
                }
            }

            float move = CnInputManager.GetAxis("Horizontal"); //ดึงค่า input จาก JoyStick ทิศ horizontal แนวนอน            
            if (CnInputManager.GetButtonDown("Jump") && !stateInfo.IsName("Jump") && ck.isOnFloor && platformActive) 
                // ถ้ากดกระโดด และ ไม่ได้เล่นอนิเมชั่น กระโดดอยู่ และ ยืนอยู่บนพื้น 
                jump(); // ให้เล่นกระโดด
            else if (move > 0.2f) { // ถ้า move มีค่ามากกว่า 0 (ลาก Joy Stick ไปทางขวา)
                if ((stateInfo.IsName("attack_farmer") || stateInfo.IsName("attack_boxer")
                    || stateInfo.IsName("attack_fisher")) && !ck.isOnFloor) { // ถ้าเล่นอนิเมชั่นโจมตีอยู่ และไม่ได้ยืนอยู่บนพื้น
                    if (turnLeft) {// เช็คว่าหันซ้ายรึเปล่า
                        this.transform.localScale = reDirection(); //หันหน้าไปทิศตรงข้าม (หันขวา)
                        turnLeft = false;
                    }
                    walk(speed); // เดินด้วยความเร็ว = speed ( ทางขวา )
                } else if(!stateInfo.IsName("attack_farmer") && !stateInfo.IsName("attack_boxer")
                    && !stateInfo.IsName("attack_fisher")) { // ถ้าเกิดไม่ได้เล่นอนิเมชั่นโจมตีอยู่
                    if (turnLeft) {
                        this.transform.localScale = reDirection();
                        turnLeft = false;
                    }
                    walk(speed);// เดินด้วยความเร็ว = speed ( ทางขวา )
                }
            } else if (move < -0.2f) { // ถ้า move มีค่าน้อยกว่า
                if ((stateInfo.IsName("attack_farmer") || stateInfo.IsName("attack_boxer")
                    || stateInfo.IsName("attack_fisher")) && !ck.isOnFloor) { // ถ้าเล่นอนิเมชั่นโจมตีอยู่ และไม่ได้ยืนอยู่บนพื้น
                    if (!turnLeft) {// ถ้าไม่ได้หันซ้าย (หันขวาอยู่)
                        this.transform.localScale = reDirection();// หันหน้าไปทิศตรงข้าม (หันซ้าย)
                        turnLeft = true;
                    }
                    walk(-speed);// เดินด้วยความเร็ว = -speed ( ทางซ้าย )
                } else if(!stateInfo.IsName("attack_farmer") && !stateInfo.IsName("attack_boxer")
                    && !stateInfo.IsName("attack_fisher")) {
                    if (!turnLeft) {
                        this.transform.localScale = reDirection();
                        turnLeft = true;
                    }
                    walk(-speed);// เดินด้วยความเร็ว = -speed ( ทางซ้าย )
                }
            } else if (move == 0) { // ถ้าเกิดยืนนิ่งๆไม่ได้เดิน
                anim.SetBool(walkHash, false); // เซ็ท walkHash (parameter ใน animator) ให้เป็น false
                if (!stateInfo.IsName("jump") && !stateInfo.IsName("fall")// ถ้าไม่ได้กระโดด หรือกำลังร่วง
                    && !stateInfo.IsName("hurt") && !stateInfo.IsName("die")// ไม่ได้ Hurt หรือ die
                    && !stateInfo.IsName("walk") && !stateInfo.IsName("attack")
                    && !stateInfo.IsName("attack_farmer") && !stateInfo.IsName("attack_boxer")
                    && !stateInfo.IsName("attack_fisher")) {// ไม่ได้ เดิน หรือ attack
                    anim.Play("idle");// เล่น animation ที่ชื่อ idle
                }
            }
            // Other button setting
            if (Input.GetKeyDown(KeyCode.Escape))// หากกดปุ่ม ESC (ใน android ปุ่ม back)
                Application.Quit();            // ให้ปิด Application
        }
    }

    Vector3 reDirection() {
        return new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }

    public void attack() {
        anim.SetTrigger(attackHash); 
    }

    public void walk(int speedW) {
        this.transform.Translate(new Vector2(speedW, 0) * Time.deltaTime);
        //this.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedW, 0), ForceMode2D.Force);
        if (!stateInfo.IsName("jump") && !stateInfo.IsName("fall"))
            anim.SetTrigger(walkHash);
        //weaponAnim.SetTrigger(Animator.StringToHash("Walk"));
    }

    public void jump() {
        cooldownJump = true;
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpPower), ForceMode2D.Force);
        anim.SetBool(onGround, false);
        anim.SetTrigger(jumpHash);
        Invoke("endCooldownJump", 0.1f);
        //weaponAnim.SetTrigger(Animator.StringToHash("Jump"));
    }

    public void JumpDownFromPlatform() {
        DisablePlatformStand();
        Invoke("EnablePlatformStand",100);
    }

    private void EnablePlatformStand() {
        platform_Stand.SetActive(true);
    }

    private void DisablePlatformStand() {
        platform_Stand.SetActive(false);
    }

    public void endCooldownJump() {
        cooldownJump = false;
    }    

    public void delayAttack() { //Delay Attack is after player get attacking discontrol and wait
        controlable = false;
        
        bool isAlive = gameObject.GetComponent<PlayerStatus>().isAlive;
        if (isAlive == true) {
            Invoke("endDelayAttack", 0.5f);
        }
    }
    public void endDelayAttack() {
        controlable = true; 
    }

    public void setControlable(bool s) {
        controlable = s;
    }

}
