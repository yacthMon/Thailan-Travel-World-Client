using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineItemControl : MonoBehaviour {
    public static OnlineItemControl Instance;
    private Dictionary<int , GameObject> onlineItemList;

    private void Start() {
        if (!Instance) {
            Instance = this;
        }
        onlineItemList = new Dictionary<int , GameObject>();
    }

    private bool IsItemExist(int onlineID) {
        return onlineItemList.ContainsKey(onlineID);
    }

    public GameObject GetOnlineItem(int onlineID) {
        GameObject onlineItem = null;
        onlineItemList.TryGetValue(onlineID , out onlineItem);
        return onlineItem;
    }

    public void RemoveItem(int onlineID) {
        if (IsItemExist(onlineID)) {
            Destroy(GetOnlineItem(onlineID));
            onlineItemList.Remove(onlineID);
        }
    }

    public void ClearOnlineItem() {
        foreach (var item in onlineItemList) {
            Destroy(item.Value);
        }
        foreach (Transform item in this.transform) {
            Destroy(item.gameObject);
        }
        onlineItemList.Clear();
    }

    public void AddItemToWorld(int onlineID, int itemID, Vector2 position) {
        GameObject item = ItemManager.GetItemGameObject(itemID);
        if (item) {
            item = Instantiate(item);
            item.transform.SetParent(this.transform);
            item.GetComponent<Item>().SetOnlineID(onlineID);
            item.transform.position = position;
            onlineItemList.Add(onlineID , item);
        }
    }

    public void AddDropItemToList(int onlineID, GameObject itemGO) {
        onlineItemList.Add(onlineID , itemGO);
        itemGO.transform.SetParent(this.transform);        
    }


}
