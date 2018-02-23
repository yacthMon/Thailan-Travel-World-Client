using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : MonoBehaviour {
    // Delegate
    public delegate void onNPCFinishedTalk();
    public onNPCFinishedTalk onNPCFinishedTalkHandler = null;
    // Delegate
    public int NPCID;
    public string NPCName;
    public string[] dialogue; // ข้อความ intro ปกติ
    public AudioClip[] dialogueVoice; // เสียง dialogue
    public string[] questAvailableDialogue; // ข้อความ intro เวลามีเควส ขอความช่วยเหลือ
    public AudioClip[] questAvailableDialogueVoice;//เสียง intro เวลามีเควสขอความช่วยเหลือ
    public int[] questListId;
    public List<Quest> questList = null; // List ของ เควสทั้งหมดที่ NPC นี้เป็นเจ้าของ
    public int[] questRequire = new int[1] { 0}; // Quest ที่จำเป็นต้องทำสำเร็จ ถึงจะเห็น NPC ตัวนี้
    public int[] questNotVisible = null; // Quest ที่ทำอยู่แล้วจะไม่เห็น NPC ตัวนี้
    public int lastQuestToVisible = 0;// Quest สุดท้ายที่ถ้าทำเสร็จแล้วจะไม่เห็น NPC ตัวนี้อีก
    //public Conversation[] conversation; //* prepare to delete
    QuestContainer qc; // Quest Container ของ Player ใว้ เช็ค Quest Process , Quest Success
    Conversation cv; // Conversation ของ NPC คอยเปลี่ยนแปลงตามเควส ตามสถานการณ์
    public string questStatus; // สถานะ Quest ของ NPC นี้ เช่น มีเควส, เควสกำลังทำอยู่, เควสเสร็จแล้ว, ผู้รับเควส
    public bool talking; // สถานะการพูดคุย ว่าพูดคุยอยู่รึเปล่า
    // UI Setting
    public GameObject questAvailableUI;
    public GameObject questProcessUI;
    public GameObject questDoneUI;
    public bool isAcceptQuest;
    DialogueSystem uiDialogue;
    // UI Setting
    GameObject player;
    AudioSource sfx;
    PlayerStatus ps;

    void Start() {
        uiDialogue = DialogueSystem.Instance;
        if (DialogueSystem.ReferenceEquals(uiDialogue , null)) {
            Debug.Log("UIDialogue is null");
        }
        player = PlayerStatus.Instance.gameObject;
        qc = player.GetComponent<QuestContainer>();        
        sfx = player.GetComponent<AudioSource>();
        ps = player.GetComponent<PlayerStatus>();
        LoadQuest();
        //refreshUI(); 
        /*if ((questAvailableDialogue.Length!=questAvailableDialogueVoice.Length) || (dialogue.Length != dialogueVoice.Length)) {
            Debug.LogWarning("NPC : " + this.name + " has dialogue and voice not equal size");
            Debug.Break();            
        }*/
    }
/*
    private void Update() {
        if (uiDialogue.active && !uiDialogue.isDecide && talking & ps.talking) { 
            // uiDialogue แสดง UI Dialogue อยู่ และไม่ได้โชว์หน้า Decide และกำลัง talking และ player ก็ talking เช่นกัน
            if (Input.GetKeyDown(KeyCode.Space)) {// ถ้าคลิกเมาส์ซ้าย (Android จิ้ม)
                ContinueConversation();
            }
        }
    }
  */  
    public void ContinueConversation() {
        if (!Conversation.ReferenceEquals(cv , null)) {// ถ้า cv ไม่เป็น null
            if (cv.next()) { // Conversation หมดรึยังถ้ายัง 
                uiDialogue.message = cv.getDialogue(); //เอาข้อความใน Conversation มาใส่ dialogue
                sfx.clip = cv.getClip(); // ดึง voice จาก Conversation มาใส่เตรียมใว้
                sfx.Play(); // สั่งให้เล่น voice
                if (questStatus == "Receiver") { // ถ้า สถานะเควสปัจจุบันเป็น Receiver
                    if (qc.getQuestFromProcessByNPCReceiver(NPCID).questID == 4 && cv.step == 1) {
                        // ถ้า Quest ID = 4 และ conversation มี step เป็น 1
                        // GameObject.Find("System").GetComponent<settingDemo>().AfraidGiant();//ทำให้ Giant เล่น Animation กลัวตรงๆ
                    }
                }
            } else { // Conversation หมดแล้ว
                cv.resetStep(); // reset step ของ cv ใหม่
                if (questStatus == "Available") { // ถ้าเป็นการรับเควส เมื่อ Conversation จบแล้วให้ acceptQuest
                    isAcceptQuest = false;
                    qc.acceptQuest(filterQuest()[0]);
                }
                Quest q = null;
                Quest qContinue;
                switch (questStatus) { // เช็คสถานะเควสปัจจุบัน                         
                    case "Available":
                        q = filterQuest()[0];
                        qc.acceptQuest(q);//ดึงเควสแรกที่สามารถทำได้ขึ้นมา และ acceptQuest นั้น
                        break;
                    case "Receiver":
                        // ถ้าเป็น Receiver ดึงเควสจากเควสที่กำลังทำอยู่ที่มี NPCID นี้เป็น Receiver
                        q = qc.getQuestFromProcessByNPCReceiver(NPCID);
                        // สั่งให้เควส q success
                        qc.successQuest(q);
                        // เอาเควสต่อเนื่องจากเควส q มาเก็บใว้ที่ qContinue
                        qContinue = q.questContinue;
                        if (!Quest.ReferenceEquals(q.questContinue , null)) {
                            qc.acceptQuest(qContinue);//ถ้าเควสต่อเนื่องไม่เป็น null ให้รับเควสนั้นต่อเลย
                        }
                        break;
                    case "Done":
                        q = qc.getQuestFromProcessByNPCID(NPCID); //ดึงเควสจาก เควสที่กำลังทำอยู่ที่รับจาก npc ID นี้
                        qc.successQuest(q);// สั่งให้เควส Success
                        qContinue = q.questContinue; // เอาเควสต่อเนื่องจากเควส q มาเก็บใว้ที่ qContinue
                        if (!Quest.ReferenceEquals(q.questContinue , null)) {
                            qc.acceptQuest(qContinue);//ถ้าเควสต่อเนื่องไม่เป็น null ให้รับเควสนั้นต่อเลย
                        }
                        break;
                    default: endDialogue(); break; //ถ้าไม่เข้า case ไหนเลยให้ endDialogue()
                }
                endDialogue();//สั่ง endDialogue จบการพูดคุย
            }
            refreshQuestAllNPC(); //สั่งให้ NPC ทุกตัว refreshUI
        } else {
            endDialogue();//สั่ง endDialogue จบการพูดคุย
        }
    }

    public void StartTalk() {
        ps = PlayerStatus.Instance;
        player = ps.gameObject;        
        sfx = player.GetComponent<AudioSource>();
        if (!ps.talking && !this.talking) {
            // ถ้า player ไม่ได้ talking อยู่ และ NPC ไม่ได้ talking เช่นกัน และ ระยะห่างไม่เกิน 5
            // Start talking
            qc = QuestContainer.Instance; //ดึง QuestContainer ใหม่
            this.talking = true; // เซ็ทให้ NPC กำลังคุยอยู่
            ps.talking = true;// เซ้ทให้ Player กำลังคุยอยู่
            int intro = Random.Range(0 , dialogue.Length); // สุ่ม index ของ Dialogue
            //หลักกการคือการ เปลี่ยน cv ของ npc ให้ไปชี้ Conversation แต่ล่ะ state ของ Quest 
            //เช่น ตอน Quest in process ก็ไปใช้ conversation in process ของ Quest นั้นๆ
            // Quest จะมี Conversation เป็น Open , In process , Done , Receiver
            switch (questStatus) {
                case "Available":
                    intro = Random.Range(0 , questAvailableDialogue.Length - 1);// สุ่ม intro (index dialogue)
                    uiDialogue.message = questAvailableDialogue[intro]; // change dialogue message
                    uiDialogue.active = true; // active dialogue gui
                    if (questAvailableDialogueVoice.Length > 0) {
                        sfx.clip = questAvailableDialogueVoice[intro]; // เซ็ทซาวของ dialogue นั้นให้กับ SFX
                        sfx.Play(); // สั่งให้ sfx เล่นซาว
                    }
                    uiDialogue.btnAccept.onClick.RemoveAllListeners(); // Clear all Listener of button
                    uiDialogue.setAutoClose(); // addListener onClick = disable Accept button and Decline button
                    uiDialogue.btnAccept.onClick.AddListener(() => { // Event onClick Accept
                        cv = filterQuest()[0].openQuestCon;// ดึง openQuestConversation ของ Quest มาใส่ cv
                        cv.next();//สั่ง cv ให้ next() เพื่อเริ่ม Dialogue แรก
                        uiDialogue.message = cv.getDialogue(); // ดึง ข้อความ Dialogue จาก Conversation มาใส่ message ของ uiDialogue
                        if (cv.getClip()) {
                            sfx.clip = cv.getClip();//ดึง voice ของ Dialogue ปัจจุบันมาใส่ sfx
                            sfx.Play();//สั่งให้ sfx เล่น sound
                        }
                        isAcceptQuest = true; // เซ็ท isAcceptQuest เป็น true เพื่อให้รับเควสเมื่อคุยจบ
                        uiDialogue.OnClickDialogueHandle = ContinueConversation;
                    });// Event onClick Accept
                    uiDialogue.btnDecline.onClick.AddListener(endDialogue);// หากกดปุ่ม Decline ให้ endDilaogue
                    uiDialogue.showDecideWithDelay(1.5f); // สั่งให้ uiDialogue โชว์ Decline หลังจาก 1.5 วินาทีให้หลัง                    
                    foreach (Quest q in filterQuest()) { // ดึงเควสที่ Player สามารถรับได้มาแสดงชื่อ
                        Debug.Log("Quest available : " + q.questTitle);
                    }
                    break;
                case "Process":
                    cv = qc.getQuestFromProcessByNPCID(NPCID).processQuestCon;//ดึง conversation In process ของเควสที่ทำอยู่
                    cv.resetStep(); cv.next();// resetStep ของ conversation และ next เพิ่มเริ่มที่ Dialogue แรก
                    uiDialogue.message = cv.getDialogue();// ดึงข้อความ จาก Dialogue มาใส่ใน message ของ uiDialogue
                    if (cv.getClip()) {
                        sfx.clip = cv.getClip();//ดึง voice ของ Dialogue ปัจจุบันมาใส่ sfx
                        sfx.Play();// สั่งให้ sfx เล่น sound
                    }
                    uiDialogue.active = true; // สั่งให้โชว์ uiDialogue
                    uiDialogue.OnClickDialogueHandle = ContinueConversation;
                    break;
                case "Done":
                    cv = qc.getQuestFromProcessByNPCID(NPCID).doneQuestCon;//ดึง conversation Done ของเควสที่ทำอยู่
                    cv.resetStep(); cv.next();// resetStep ของ conversation และ next เพิ่มเริ่มที่ Dialogue แรก
                    uiDialogue.message = cv.getDialogue();// ดึงข้อความ จาก Dialogue มาใส่ใน message ของ uiDialogue
                    if (cv.getClip()) {
                        sfx.clip = cv.getClip();//ดึง voice ของ Dialogue ปัจจุบันมาใส่ sfx
                        sfx.Play();// สั่งให้ sfx เล่น sound
                    }
                    uiDialogue.active = true;    // สั่งให้โชว์ uiDialogue
                    uiDialogue.OnClickDialogueHandle = ContinueConversation;
                    break;
                case "Receiver":
                    cv = qc.getQuestFromProcessByNPCReceiver(NPCID).receiveQuestCon;//ดึง conversation Receiver ของเควสที่ทำอยู่
                    cv.resetStep(); cv.next();// resetStep ของ conversation และ next เพิ่มเริ่มที่ Dialogue แรก
                    uiDialogue.message = cv.getDialogue();// ดึงข้อความ จาก Dialogue มาใส่ใน message ของ uiDialogue
                    if (cv.getClip()) {
                        sfx.clip = cv.getClip();//ดึง voice ของ Dialogue ปัจจุบันมาใส่ sfx
                        sfx.Play();// สั่งให้ sfx เล่น sound
                    }
                    uiDialogue.active = true;// สั่งให้โชว์ uiDialogue
                    uiDialogue.OnClickDialogueHandle = ContinueConversation;
                    break;
                default:
                    uiDialogue.message = dialogue.Length > 0 ? dialogue[intro] : ""; //เซ็ท message ใน uiDialogue ให้เป็น dialogue ตำแหน่งที่ intro 
                    sfx.clip = dialogueVoice.Length > 0 ? dialogueVoice[intro] : null; //เซ็ท sound ใน sfx ให้เป็น sound ของ dialogueVoice ตำแหน่งที่ intro
                    sfx.Play();// สั่งเล่น Sound                    
                    uiDialogue.active = true; // โชว์ uiDialogue
                    uiDialogue.OnClickDialogueHandle = ContinueConversation;
                    break;                    
            }            
            refreshQuestAllNPC();
        }
    }

    void endDialogue() {// end conversation 
        uiDialogue.OnClickDialogueHandle = null;
        uiDialogue.active = false;
        sfx.Stop();
        if (onNPCFinishedTalkHandler != null) {
            Debug.Log("onNPCFinishedTalkHandler is not null");
            if (qc.checkQuestSuccess(lastQuestToVisible)) {
                //ถ้า lastQuestToVisible ถูกส่งแล้ว event Finished Talk จะทำงานหลังจาก endDiaglogue 2 วิ
                //เนื่องจาก ส่วนใหญ่ที่ หรือ 100% ที่เข้าส่วนนี้คือ ส่งเควส lastQuestToVisible ซึ่ง NPC จะต้องหายไป
                //โดยจะมีการ fade screen เป็นสีดำ (Conversation ในส่วนของ fadeIn)
                Invoke("callOnNPCFinishedTalkHandler" , 2f);
                Invoke("endTalking" , 2f);
            } else {
                onNPCFinishedTalkHandler();
                endTalking();
            }
        }
        cv = null;
        
    }
    void endTalking() {
        this.talking = false;
        ps.talking = false;
    }
    void callOnNPCFinishedTalkHandler() {
        onNPCFinishedTalkHandler();
    }
    // Show sign for quest status
    public void refreshStatus() {
        if (qc.checkQuestProcessByNPCReceiver(NPCID)) {
            questDoneUI.SetActive(true);
            questStatus = "Receiver";
            return;
        }
        if (checkContainProcessQuest()) {
            // Player Have quest from this NPC in processList            
            if (checkQuestProceesDone()) {
                if (checkQuestSendToThisNPC()) {
                    // Player Have quest in process list which Done and ready to success for this NPC
                    questDoneUI.SetActive(true);
                    questStatus = "Done";
                    gameObject.GetComponent<Animator>().SetBool(Animator.StringToHash("worry") , false);
                } else { // Quest from this npc success but this npc not a receiver
                    questProcessUI.SetActive(true);
                    questStatus = "Process";
                }
            } else {
                // Player Have quest in process list but not done just in process
                questProcessUI.SetActive(true);
                questStatus = "Process";
            }
        } else {
            // Don't have quest from this NPC in processList but NPC have quest available for Player
            Quest[] questAvailable = filterQuest();
            if (questAvailable.Length > 0) {
                questAvailableUI.SetActive(true);
                gameObject.GetComponent<Animator>().SetBool(Animator.StringToHash("worry"),true);
                questStatus = "Available";
            } else {
                // Nothing about quest
                questStatus = "Nothing";
            }
            // questAvailable is quest that Player can accept
        }               
    }
    // --------------------------------------------------------
    public bool checkContainProcessQuest() { 
        return qc.checkQuestProcessByNPC(NPCID); //check if have quest in processsList with from this NPC
    }

    public bool checkQuestProceesDone() {
        Quest q = qc.getQuestFromProcessByNPCID(NPCID); // get Quest in processList with from this NPC
        if (qc.checkQuestStatusDone(q)) { // check if quest done ?
            return true;
        }
        return false;
    }

    public bool checkQuestSendToThisNPC() {
        Quest q = qc.getQuestFromProcessByNPCID(NPCID); // get Quest in processList with from this NPC
        if (qc.checkQuestStatusDone(q)) { // check if quest done ?
            if (q.qcon.type == 1) {
                if (q.npcTargetID == this.NPCID) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (q.npcID == this.NPCID)
                    return true;
            }
        }
        return false;
    }

    public Quest[] filterQuest() { // find Available quest and return
        List<Quest> qResult = new List<Quest>();
        foreach (Quest q in questList) {
            if (!Quest.ReferenceEquals(q , null)) {
                bool check = qc.checkQuestSuccess(q.questRequire);//Check quest require if it success
                bool check2 = qc.checkQuestSuccess(q); //Check quest if it success             
                if (check && !check2) {
                    qResult.Add(q);
                }
            }
        }
        return (Quest[])qResult.ToArray();
    }
    public void resetUI() { // set UI quest status off
        questAvailableUI.SetActive(false);
        questProcessUI.SetActive(false);
        questDoneUI.SetActive(false);
    }

    public void refreshUI() { // refresh UI quest status
        qc = QuestContainer.Instance;        
        resetUI();
        refreshStatus();
    }

    public void refreshQuestAllNPC() {
        GameObject.Find("NPC_System").BroadcastMessage("refreshUI"); // broadcast message to all child of NPC_System
    }

    public void LoadQuest() {
        if (questListId.Length > 0) {
            foreach (int questId in questListId) {
                Quest quest = QuestManager.GetQuestByID(questId);
                if (!Quest.ReferenceEquals(quest , null)) {
                    this.questList.Add(quest);
                }
            }
            refreshUI();
        }
    }

    public bool shouldAvailable() {
        qc = QuestContainer.Instance;
        // true should
        // false shouldn't
        if (questStatus== "Done") {
            return true;
        } // เช็คสถานะ Quest Status ของ NPC ว่าเป็น Done หรือเปล่า ถ้าเป็น Done ควร Visible ให้ส่งเควสได้
        // ในกรณีที่ notVisible ใน เควสที่ตัวเองให้ อย่างเช่น เควสสู้กับ NPC
        if (qc.checkQuestSuccess(lastQuestToVisible)) {
            return false;
        } // Quest สุดท้ายที่ถ้าทำเสร็จแล้วจะไม่เห็น NPC อยู่ใน Success List หรือเปล่า ถ้าใช่ return false 
        // Check is QuestRequire success ?  && Check is QuestNotVisible in quest process ?     
        bool haveNotVisible = false;
        foreach(int qid in questNotVisible) {
            haveNotVisible = qc.checkQuestProcess(qid);// Check if have quest not visible in process ? if have turn to false
            if (haveNotVisible) break;
        }
        bool result = (qc.checkQuestSuccess(questRequire) && !haveNotVisible);
        return result;
    }
}
