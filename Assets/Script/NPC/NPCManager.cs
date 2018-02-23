using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {
    public static GameObject GetNPCFromID(int id) {
        GameObject prefabNPC = Resources.Load("Prefab/NPC/" + id) as GameObject;
        if (prefabNPC) {
            return prefabNPC;
        }
        return null;
    }

    public static NPCStatus GetNPCStatusFromID(int id) {
        GameObject prefabNPC = Resources.Load("Prefab/NPC/" + id) as GameObject;
        if (prefabNPC) {
            NPCStatus status = prefabNPC.GetComponent<NPCStatus>();
            //item.SetItemEffect(prefabItem.GetComponent<ItemEffect>());
            return status;
        }
        return null;
    }
	
    public static string GetNPCNameByID(int id) {
        GameObject prefabNPC = Resources.Load("Prefab/NPC/" + id) as GameObject;
        if (prefabNPC) {
            return prefabNPC.GetComponent<NPCStatus>().NPCName;
        }
        return null;
    }
    

    
}
