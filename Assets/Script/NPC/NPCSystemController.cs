using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSystemController : MonoBehaviour {

	
	void Start () {
        foreach(Transform npc in transform) {
            npc.gameObject.GetComponent<NPCStatus>().onNPCFinishedTalkHandler += refreshVisibleNPC; 
            //add refreshVisibleNPC to onNPCFinishedTalkHandler on all NPC;
        }
        refreshVisibleNPC();
        this.BroadcastMessage("refreshUI");
    }
	
    public bool setNPCvisible(int npcid, bool visible) {
        GameObject npc = findNPC(npcid);
        if (npc != null) {
            npc.SetActive(visible);
            return true;
        }
        return false;
    }
    public bool setNPCvisible(GameObject npc, bool visible) {
        if (npc != null) {
            npc.SetActive(visible);
            return true;
        }
        return false;
    }
    public GameObject findNPC(int npcID) {
        foreach (Transform child in transform) {
            if (child.GetComponent<NPCStatus>().NPCID == npcID) {                
                return child.gameObject;
            }
        }
        return null;
    }
    public GameObject findNPC(string npcName) {
        return transform.Find(npcName).gameObject;
    }
    public void refreshVisibleNPC() {
        foreach(Transform npc in transform) {
            setNPCvisible(npc.gameObject,npc.gameObject.GetComponent<NPCStatus>().shouldAvailable());
        }
        //this.BroadcastMessage("shouldAvailable");
    }
}
