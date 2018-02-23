using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {
    [Tooltip("เลข ID ของไอเท็มที่สามารถ Drop ได้")]
    public int[] itemIdDrops;
    [Tooltip("Rate การตกของ Item")]
    public float[] itemDropRate;
    public Vector2 force = new Vector2(3,4);
    public List<GameObject> itemPool = new List<GameObject>();
    public bool isOnlineMode = true;
    private void Start() {
        /*
        if (!isOnlineMode) { 
            for (int i = 0; i < itemDropRate.Length; i++) {
                if (calculateChanceDrop(itemDropRate[i])) {//if chance success
                    GameObject item = ItemManager.GetItemGameObject(itemIdDrops[i]);
                    if (item) {//ถ้ามี item ที่มี id ดังกล่าว                    
                        itemPool.Add(item);
                    }
                }
            }
        
        /// Add event dropitem to onMonsterDie
        this.GetComponent<MonsterStatus>().onMonsterDieHandler = doDropItem;
        }*/
    }
    
    private bool calculateChanceDrop(float percent) {
        float random = Random.Range(0.00f, 100.00f);        
        if (random <= percent)
            return true;
        else
            return false;
    }

    public void doDropItem() {
        foreach (GameObject item in itemPool) {
            GameObject itemGO = Instantiate(item , this.transform.position , Quaternion.identity);
            Debug.Log("Drop Item ID " + itemGO.GetComponent<Item>().GetOnlineID());
            OnlineItemControl.Instance.AddDropItemToList(itemGO.GetComponent<Item>().GetOnlineID(), itemGO);
            itemGO.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-force.x , force.x) , force.y) , ForceMode2D.Force);
        }
    }

    public void doDropItem(Item[] items) {
        foreach (Item item in items) {
            GameObject itemGO = Instantiate(ItemManager.GetItemGameObject(item.GetItemId()) , this.transform.position , Quaternion.identity);
            Item.CopyItem(itemGO.GetComponent<Item>(),item);
            Debug.Log("Drop Item ID " + itemGO.GetComponent<Item>().GetOnlineID());
            OnlineItemControl.Instance.AddDropItemToList(itemGO.GetComponent<Item>().GetOnlineID() , itemGO);
            itemGO.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-force.x , force.x) , force.y) , ForceMode2D.Force);
        }
    }

    public void AddItemToPool(int[] itemID) {
        for (int i = 0; i < itemID.Length; i++) {
            //Debug.Log("try to get Item : " + itemID[i]);
            GameObject item = ItemManager.GetItemGameObject(itemID[i]);
            if (item) {//ถ้ามี item ที่มี id ดังกล่าว           
                //Debug.Log("Add Item to pool : " +item.name);
                itemPool.Add(item);
            }
        }
    }

    public void AddItemToPool(Item[] items) {
        foreach(Item item in items) {
            GameObject itemGO = ItemManager.GetItemGameObject(item.GetItemId());
            if (itemGO) {
                Item.CopyItem(itemGO.GetComponent<Item>() , item);
                itemPool.Add(itemGO);
            }
        }
    }
}
