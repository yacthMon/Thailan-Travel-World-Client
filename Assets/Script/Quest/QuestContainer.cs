using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContainer : MonoBehaviour {
    // Delegate
    public delegate void onQuestAccept();
    public delegate void onQuestStart();
    public delegate void onQuestDone();
    public delegate void onQuestAbandon();
    public onQuestAccept onQuestAcceptHandler = null;
    public onQuestStart onQuestStartHandler = null;
    public onQuestDone onQuestDoneHandler = null;
    public onQuestAbandon onQuestAbandonHandler = null;
    // Delegate
    public List<Quest> processList = new List<Quest>();
    public List<Quest> successList = new List<Quest>();
    // Singleton
    public static QuestContainer Instance;
    private void Start() {
        if (!Instance) {
            Instance = this as QuestContainer;
        }
        // Code สำหรับทดสอบระบบ Quest Container
        /*Quest q = new Quest(1, "Test " + questSuccessId, "Detail ", null, null, 100);
        q.isQuestSuccess = true;
        processList.Add(q);
        
        successList.Add(new Quest(2, "Test " + questSuccessId, "Detail ", null, null, 100));
        */
        /*foreach (Quest qs in successList) {
            Debug.Log(qs.questID);
        }*/
        //Debug.Log("Is q1 done ? : " + checkQuestStatusDone(q));
        //Debug.Log("Is quest process have quest from 201 : " + checkQuestProcessFromNPC(201));     
    }

    public void test() {
        Quest q1 = new Quest(1, "test", "test detail","Test Place Detail", new Quest[0], null, 100,null);
        Debug.Log("Quest Accept q1 : " + acceptQuest(q1));
        Debug.Log("Quest Accept q1 again : " + acceptQuest(q1));
        Debug.Log("Quest Done q1 : " + successQuest(q1));
        Debug.Log("Quest Accept q1 again : " + acceptQuest(q1));
        Quest q2 = new Quest(2, "test2", "test 2 detail", "Test Place Detail", new Quest[1] { q1 }, null, 100,null);
        Debug.Log("Check Quest Success (q1) : " + checkQuestSuccess(new Quest[1] { q1 }));
        Debug.Log("Check Quest Success (q2) : " + checkQuestSuccess(new Quest[1] { q2 }));
        Debug.Log("Abandon quest (q2) : " + abandonQuest(q2));
        Debug.Log("Quest Accept q2 : " + acceptQuest(q2));
        Debug.Log("Abandon quest (q2) : " + abandonQuest(q2));
        Debug.Log("Quest Done (q2) : " + successQuest(q2));
    }
    public bool acceptQuest(Quest q) {
        if (!checkNull(q)) {
            if (processList.Exists(qs => qs.questID == q.questID) ||
                successList.Exists(qs => qs.questID == q.questID)) {
                return false;
            }
            NotificationSystem.Instance.AddNotification("[เควส] รับเควส " + q.questTitle +" มาแล้ว");
            processList.Add(q);
            getQuestFromProcess(q).startQuest();
            if(q.qcon.type!=3)
                DGTRemote.GetInstance().RequestSendAcceptQuest(q.questID);
            return true;
        }
        return false;
    }

    public bool successQuest(Quest q) {
        if(!checkNull(q))
            if (processList.Exists(qs => qs.questID == q.questID) &&
                !successList.Exists(qs => qs.questID == q.questID)) {
                NotificationSystem.Instance.AddNotification("[เควส] ส่ง " + q.questTitle + " เรียบร้อยแล้ว");
                q.RewardToPlayer();
                processList.Remove(q);
                successList.Add(q);
                DGTRemote.GetInstance().RequestSendSuccessQuest(q.questID);
                return true;
            }
        return false;
    }

    public bool AddProcessQuest(Quest q, int currentTotal) {
        if (!checkNull(q)) {            
            q.startQuest();            
            processList.Add(q);
            q.qcon.SetCurrentTotal(currentTotal);
            return true;
        }
        return false;
    }

    public bool AddSuccessQuest(Quest q) {
        if (!checkNull(q)) { 
            successList.Add(q);
            return true;
        }
        return false;
    }

    public bool abandonQuest(Quest q) {
        if(!checkNull(q))
            if (processList.Exists(qs => qs.questID == q.questID)) { 
                processList.Remove(q);
                return true;
            }
        return false;
    }

    public bool abandonQuest(int qid) {
        return abandonQuest(new Quest() { questID = qid });
    }

    public void refreshQuest() {
        foreach(Quest q in processList.ToArray()) {
            if (q.qcon.IsDone()) {
                q.isQuestSuccess = true;
                GameObject npcSystem = GameObject.Find("NPC_System");
                if (npcSystem ) {
                    npcSystem.BroadcastMessage("refreshUI"); //sendMessage all childen
                }
            }
        }
    }    
    //------------------------- Check Quest Process ----------------
    //--------------------------------------------------------------
    public bool checkQuestProcess(Quest[] ql) {
        if (ql != null) { 
            foreach (Quest q in ql) {
                if (checkNull(q)) {
                    return false;
                }
                if (!processList.Exists(qs => qs.questID == q.questID))
                    return false;
            }
        } else return false;
        return true;
    }
    public bool checkQuestProcess(Quest q) {
        if (!checkNull(q)) {
            if (processList.Exists(qs => qs.questID == q.questID))
                return true;
            else
                return false;
        } else return false;
        //return true;
    }
    public bool checkQuestProcess(int[] qlid) {
        if (qlid.Length >0) { //เช็คว่า qlid มีค่าหรือไม่
            foreach (int qid in qlid) { // เอาเลข id ออกมาทีละตัว
                if (qid != 0) {// ถ้า questID ไม่ใช่ 0
                    if (!processList.Exists(qs => qs.questID == qid))
                        return false; // มีบางเควส id ที่รับมา ไม่มีอยู่ใน process lists
                }
            }
        } else return false;
        return true;
    }
    public bool checkQuestProcess(int qid) {
        if (qid == 0) {
            return true;
        } else if (!processList.Exists(qs => qs.questID == qid))
            return false;

        return true;
    }
    //---------------------- Check Quest Success ------------------
    public bool checkQuestSuccess(Quest[] ql) {
        if (ql != null)
            foreach (Quest q in ql) {
                if (checkNull(q)) {
                    return false;
                }
                if (!successList.Exists(qs => qs.questID == q.questID))
                    return false;

            }
        return true;
    }
    public bool checkQuestSuccess(Quest q) {
        if (!checkNull(q))
            if (successList.Exists(qs => qs.questID == q.questID))
                return true;
            else
                return false;
        return true;
    }
    public bool checkQuestSuccess(int[] qlid) {
        if (qlid.Length > 0)//เช็คว่า qlid มีค่าหรือไม่
            foreach (int qid in qlid) {// เอาเลข id ออกมาทีละตัว
                if (qid != 0) {// ถ้า questID ไม่ใช่ 0
                    if (!successList.Exists(qs => qs.questID == qid))
                        return false;// มีบางเควส id ที่รับมา ไม่มีอยู่ใน success list
                }                
            }
        return true;
    }
    public bool checkQuestSuccess(int qid) {
                if (qid == 0) {
                    return false;
                } else if (!successList.Exists(qs => qs.questID == qid))
                    return false;

        return true;
    }
    public bool checkQuestStatusDone(Quest q) {
        if (!checkNull(q)) {
            Quest qCheck = processList.Find(qs => qs.questID == q.questID);
            if (!checkNull(qCheck)) 
                if (qCheck.isQuestSuccess)
                    return true;
        }
        return false;
    }
    public bool checkQuestProcessByNPC(int npcid) {
        if (processList.Exists(qs => qs.npcID == npcid))
                return true;        
        return false;
    }
    public bool checkQuestProcessByNPCReceiver(int npcid) { 
        //check if it have quest for NPC as Receiver ?
        if (processList.Exists(qs => qs.npcTargetID == npcid))
            return true;
        return false;
    }
    public Quest getQuestFromProcess(Quest q) {
        if (!Quest.ReferenceEquals(q , null))
            return processList.Find(qs => qs.questID == q.questID);
        return null;
    }
    public Quest getQuestFromProcessByNPCID(int npcid) {
        Quest q = null;
        q = processList.Find(qs => qs.npcID == npcid);
        return q;
    }
    public Quest getQuestFromProcessByNPCReceiver(int npcid) { 
        // get Quest from Quest process with have npcid as Receiver
        Quest q = null;
        q = processList.Find(qs => qs.npcTargetID == npcid);
        return q;
    }
    public bool checkNull(Quest q) { // check if quest null
        return Quest.ReferenceEquals(q, null);
    } 

    public void copyQuestContainer(QuestContainer qc) {
        Debug.Log("Start Copy Quest");
        Debug.Log("From " + qc.gameObject.name + " to " + this.gameObject.name);
        Debug.Log("Process list Count : " + qc.processList.Count);
        qc.processList.ForEach(q => this.acceptQuest(q));
        qc.successList.ForEach(q => this.successList.Add(q));
        /*foreach(Quest q in qc.processList.ToArray()) {
            Debug.Log("Move " + q.questTitle + " to new Quest Container Process");
            this.processList.Add(q);
        }
        foreach(Quest q in qc.successList.ToArray()) {
            Debug.Log("Move " + q.questTitle + " to new Quest Container Success");
            this.successList.Add(q);
        }*/
        
    }
}
