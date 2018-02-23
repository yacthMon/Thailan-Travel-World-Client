using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour {
    // For create Quest data in Game to convert as Quest object
    [System.Serializable]
    public class ConversationData {
        [TextArea]
        public string[] dialogues;
        public AudioClip[] voices;
        public bool isFadeAfterDone;
    }
    [System.Serializable]
    public class QuestConditionData {
        [Header("Type 0 : Report back to NPC")]
        [Header("Type 1 : Find NPC")]
        [Header("Type 2 : Kill monster")]
        [Header("Type 3 : Kill Boss")]
        [Header("Type 4 : Collect Item")]
        [Space(10)]
        public int conditionType;
        public int targetID;
        public int amount;
        public string Note;
    }

    public int questID;
    public string questTitle;
    public string questDetail;
    public string questPlaceDetail;
    [Header("Enter only Quest ID that require for this quest")]
    public int[] questRequireID;
    public QuestConditionData questConditionData;
    public int npcOwnerID;
    [SerializeField]
    public ConversationData[] conversations;
    [Header("Quest continue (เควสต่อเนื่อง)")]
    public QuestData questContinue;
    [Header("Reward after success")]
    public int EXP;

    public Quest CreateQuest() {
        // Create empty quest for make QuestRequire
        List<Quest> questRequire;
        if (questRequireID.Length > 0) {
            questRequire = new List<Quest>();
            foreach (int questID in questRequireID) {
                questRequire.Add(new Quest(questID));
            }
        } else { // no quest require
            questRequire = null;
        }
        // Create QuestCondition from questConditionData
        QuestCondition qcon = new QuestCondition(questConditionData.conditionType, questConditionData.targetID, questConditionData.amount);        
        // Create Conversations[] 
        List<Conversation> cons = new List<Conversation>();
        foreach(ConversationData conData in conversations) {
            Conversation con = conData.voices.Length >0 ? // have voice ? by checking length
                new Conversation(conData.dialogues , conData.voices):
                new Conversation(conData.dialogues);
            if (conData.isFadeAfterDone) {
                con.fade = true;
            }
            cons.Add(con);
        }
        // Create Quest
        Quest quest;
        if (qcon.type == 1) {
            quest = new Quest(questID , questTitle , questDetail , questPlaceDetail, questRequire == null ? null : questRequire.ToArray() , qcon , npcOwnerID , cons.ToArray(),qcon.getTarget());
        } else {
            quest = new Quest(questID , questTitle , questDetail , questPlaceDetail, questRequire == null ? null : questRequire.ToArray() , qcon , npcOwnerID , cons.ToArray());
        }
        // if have questContinue, add to quest
        if (!QuestData.ReferenceEquals(questContinue , null)) {
            quest.questContinue = questContinue.CreateQuest();
        }
        // if have EXP Reward add to quest reward
        if(EXP > 0) {
            quest.rewardEXP = EXP;
        }
        return quest;
    }
}
