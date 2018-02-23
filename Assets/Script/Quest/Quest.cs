using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest : ScriptableObject {
    // Delegate   
    
    // Delegate
    //private int questID;
    public int questID {get; set;}
    public string questTitle { get; set; }
    public string questDetail { get; set; }
    public string questPlaceDetail { get; set; }
    public Quest[] questRequire { get; set; }
    public Quest questContinue { get; set; }
    public int rewardEXP;
    public bool isQuestSuccess { get; set; }
    public int npcID { get; set; }
    public int npcTargetID { get; set; }
    public QuestCondition qcon { get; set; }  
    public Conversation openQuestCon { get; set; }
    public Conversation processQuestCon { get; set; }
    public Conversation doneQuestCon { get; set; }
    public Conversation receiveQuestCon { get; set; }

    public Quest() {        
        // create Quest with zero setting
        this.questID = 0;
        this.questTitle = "Unknown";
        this.questDetail = "Unknown";
        this.questPlaceDetail = "Unknow";
        this.questRequire = null;
        this.qcon = null;
        this.isQuestSuccess = false;
        this.questContinue = null;
        this.npcID = 0;
    }

    public Quest(int id) {
        // create Quest with id for empty quest require
        this.questID = id;
    }

    public Quest(int id , string title , string detail , string placeDetail , Quest[] questRequire , QuestCondition qc , int npcid , Conversation[] con , int npctarget) {
        // create Quest with target NPC (send quest to target NPC after done)
        this.questID = id;
        this.questTitle = title;
        this.questDetail = detail;
        this.questPlaceDetail = placeDetail;
        this.questRequire = questRequire;
        this.qcon = qc;
        this.isQuestSuccess = false;
        this.npcID = npcid;
        this.npcTargetID = npctarget;
        if (con != null) {
            if (con.Length > 0) {
                this.openQuestCon = con[0];
                this.processQuestCon = con[1];
                this.doneQuestCon = con[2];
                if (con.Length == 4)
                    this.receiveQuestCon = con[3];
            } else {
                this.openQuestCon = con[0];
                this.processQuestCon = con[1];
                this.doneQuestCon = con[2];
            }
        }
        this.questContinue = null;        
    }

    public Quest(int id, string title, string detail,string placeDetail, Quest[] questRequire,QuestCondition qc, int npcid, Conversation[] con) {
        // create Quest with normal setting
        this.questID = id;
        this.questTitle = title;
        this.questDetail = detail;
        this.questPlaceDetail = placeDetail;
        this.questRequire = questRequire;
        this.qcon = qc;
        this.isQuestSuccess = false;
        this.npcID = npcid;
        if (con != null) {
            if (con.Length > 0) {
                this.openQuestCon = con[0];
                this.processQuestCon = con[1];
                this.doneQuestCon = con[2];
                if (con.Length == 4)
                    this.receiveQuestCon = con[3];
            } else {
                this.openQuestCon = con[0];
                this.processQuestCon = con[1];
                this.doneQuestCon = con[2];
            }
        }
        this.questContinue = null;
    }
    
    public void startQuest() {
        if (!QuestCondition.ReferenceEquals(qcon, null)) {
            qcon.startTracking(questID,questTitle);
            if(qcon.type == 3) {
                GameObject system = GameObject.Find("System");
                system.GetComponent<DialogueSystem>().active = false; // close Dialogue
                NPCSystemController npcSystem = GameObject.Find("NPC_System").GetComponent<NPCSystemController>();
                GameObject npc = npcSystem.findNPC(npcID); // npc who will become boss
                npcSystem.setNPCvisible(npc,false); // set that npc not visible
                GameObject Boss = Instantiate(Resources.Load("Prefab/Boss/Boss_" + npc.name),npc.transform.position, Quaternion.identity)
                    as GameObject;
                Physics2D.IgnoreLayerCollision(8, 11, false);// player BlockMonster
                respawnSystem rs = system.GetComponent<respawnSystem>();
                rs.onPlayerRespawnHandler += ()=> {
                    Debug.Log("Destroy"); 
                    Destroy(Boss);
                };
                Boss.GetComponent<monsterAI>().angryAttacker(rs.currentPlayer); // set Boss to angry player
                Boss.GetComponent<MonsterStatus>().onMonsterDestroyHandler += () => {// add Event after boss die
                    npcSystem.setNPCvisible(npc, true); // set that npc visible
                    npc.GetComponent<NPCStatus>().refreshUI(); // refresh UI for that NPC
                    npc.transform.position = Boss.transform.position; // move NPC to that position
                    Physics2D.IgnoreLayerCollision(8, 11, true);// player BlockMonster
                };
            }
        }
    }

    public void RewardToPlayer() {
        PlayerStatus.Instance.getEXP(rewardEXP);
    }

    public string GetRewardString() {
        return rewardEXP > 0 ? "ได้รับค่าประสบการณ์ " + rewardEXP : "";
    }

    
}
