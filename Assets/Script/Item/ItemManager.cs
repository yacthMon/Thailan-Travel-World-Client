using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : ScriptableObject {
    /// <summary>
    /// เรียก GameObject Item จากเลข ID ของไอเท็ม
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static GameObject GetItemGameObject(int id) {
        GameObject prefabItem = Resources.Load("Item/Prefab/item_" + id) as GameObject;
        if (prefabItem) {
            return prefabItem;
        }
        return null;
    }

    /// <summary>
    /// เรียก Item Script ของ ID ไอเท็มดังกล่าว
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Item GetItemComponent(int id) {
        GameObject prefabItem = Resources.Load("Item/Prefab/item_" + id) as GameObject;
        if (prefabItem) {
            Item item = prefabItem.GetComponent<Item>();
            //item.SetItemEffect(prefabItem.GetComponent<ItemEffect>());
            return item;
        }
        return null;
    }

    /// <summary>
    /// เรียก GameObject Item เปล่าๆ แล้วไปสร้างข้อมูลเอง
    /// </summary>
    /// <returns></returns>
    public static GameObject CreateItemGameObject() {
        GameObject prefabItem = Resources.Load("Item/Prefab/item_0") as GameObject;
        return prefabItem;
    }

    public static Item GetItemEquipmentBodyByEquipmentValue(string value, string gender, string job) {
        Item equipment = null;
        GameObject[] equipmentGO = Resources.LoadAll<GameObject>("Item/Prefab/");
        foreach(GameObject itemGO in equipmentGO) {            
            Item item = itemGO.GetComponent<Item>();
            Debug.Log("Scan item : " + item.GetItemName());
            if (item.IsEquipable()) {                
                Item.ItemEquipment equipmentData = item.GetItemEquipment();
                if (equipmentData.body) {
                    if (equipmentData.gender == "all" || (gender == equipmentData.gender)) {
                        if (equipmentData.job == "all" || (job == equipmentData.job)) {
                            if (equipmentData.equipmentValue == value) {
                                Debug.Log("Found Item : " + item.GetItemName());
                                equipment = item;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return equipment;
    }
}
