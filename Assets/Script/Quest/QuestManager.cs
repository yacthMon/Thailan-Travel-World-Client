using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
   public static Quest GetQuestByID(int id) {
        GameObject questPrefab = Resources.Load("Prefab/Quest/Quest_" + id) as GameObject;
        if (questPrefab) {
            return questPrefab.GetComponent<QuestData>().CreateQuest();
        }
        return null;
    } 
}
