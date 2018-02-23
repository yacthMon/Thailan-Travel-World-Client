using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCondition : MonoBehaviour {
    // Delegate    
    public delegate void onStartTrack();
    public delegate void onConditionComplete();    
    public onStartTrack onStartTrackHandler = null;
    public onConditionComplete onConditionCompleteHandler = null;
    // Delegate
    public int type { set; get; }
    // ------ Type 0 Report back to NPC
    // ------ Type 1 Find NPC
    // ------ Type 2 Kill monster   
    // ------ Type 3 Kill Boss
    // ------ Type 4 Collect item
    private int targetID { set; get; }
    private string targetName { set; get; }
    private int total { set; get; }
    private int currentTotal { set; get; }
    private bool isDone { set; get; }
    private int questID { get; set; }
    private string questTitle { set; get; }
    public QuestCondition() {
        type = 0; targetID =0; total = 0;isDone = false;
    }

    public QuestCondition(int type,int target, int total) {
        this.type = type; this.targetID = target; this.total = total;isDone = false;
        if(type == 2) {
            targetName = MonsterManager.GetMonsterNameByID(targetID);
        }
    }

    public int getTotal() {
        return total;
    }

    public int getCurrentTotal() {
        return this.currentTotal;
    }

    public int getTarget() {
        return this.targetID;
    }

    public bool IsDone() {
        return this.isDone;
    }

    public void SetCurrentTotal(int currentTotal) {
        this.currentTotal = currentTotal;
        CheckDone();
    }

    public void startTracking(int questID, string questTitle) {
        this.questID = questID;
        this.questTitle = questTitle;
        if (onStartTrackHandler != null)
            onStartTrackHandler();
        if (type == 0) {
            isDone = true;
            QuestContainer.Instance.refreshQuest();//refresh Quest status
        } else if (type == 1) {
            // find NPC normally is Reciever so we have nothing to do here
        } else if (type == 2) {
            Debug.Log("Start tracking");
            PlayerStatus.Instance.onKillMonsterHandler += TrackingMonster;
        } else if (type == 3) {
            Debug.Log("Start tracking");
            PlayerStatus.Instance.onKillMonsterHandler += TrackingMonster;            
        }
    }
    public void TrackingMonster(int monsterID) {
        if(monsterID == this.targetID) {
            currentTotal += 1;
            if (type != 3) {
                DGTRemote.GetInstance().RequestSendUpdateQuest(questID , currentTotal);
            }
            NotificationSystem.Instance.AddNotification("[เควส] กำจัด " + targetName + " " + currentTotal + "/" + total);
            Debug.Log("Monster kill : " + currentTotal + "/" + total);
            CheckDone();
        }
    }

    public void CheckDone() {
        if (currentTotal == total) {
            isDone = true;
            QuestContainer.Instance.refreshQuest();
            // refresh Quest status after done
            StopTracking();
            if (onConditionCompleteHandler != null)
                onConditionCompleteHandler();
        }
    }

    public void StopTracking() {
        if (type == 2 || type ==3) {
            Debug.Log("Quest done ! stop trakcing");
            PlayerStatus.Instance.onKillMonsterHandler -= TrackingMonster;
        } else if(type == 4) {

        }
    }

    public string ToDetailString() {
        if (type == 2 || type == 3)
            return "กำจัด " + MonsterManager.GetMonsterNameByID(targetID) +" "+ this.currentTotal + " \\ " + this.total;
        else
            return "";
    }
}
