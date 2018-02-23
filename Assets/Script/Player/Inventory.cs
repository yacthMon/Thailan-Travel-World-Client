using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField]
    private int money;    
    private static int maxSizeSlot = 24;
    public GameObject slotsContentWindow;
    GameObject[] slotsInventoryUI = new GameObject[maxSizeSlot];
    Item[] slots = new Item[maxSizeSlot];
    public static Inventory Instance;

    private void Start() {
        if (Instance == null) {
            Instance = this;
        }
        if (slotsContentWindow != null)
            RefreshSlotsUI();
    }

    public void initInventory(List<PlayerData.ItemList> il, int money) {
        PlayerData.ItemList[] itemList = il.ToArray();
        if (slotsContentWindow != null)
            RefreshSlotsUI();        
        for (int i = 0; i < itemList.Length; i++) {
            // Item ใน Slots ลิ้งค์ไปที่ Component ที่ ItemManager หยิบมาให้ที่เดียว เวลาลบเลยโดนลบหมด            
            slots[i] = new Item();
            slots[i].GetItemDataByID(itemList[i].getItemId());
            slots[i].SetIndex(i);
            slots[i].SetItemStack(itemList[i].getItemAmount());            
            Item itemAddToUI = slotsInventoryUI[i].AddComponent<Item>() as Item;
            Item.CopyItem(itemAddToUI , slots[i]);
        }
        this.money = money;
        UpdateSlotsWindows();
    }

    private void RefreshSlotsUI() { // get Slots UI into control
        for (int i = 0; i < maxSizeSlot; i++) {
            slotsInventoryUI[i] = slotsContentWindow.transform.GetChild(i).gameObject;
        }
    }
    
    public Item[] GetSellableItems() {
        List<Item> sellableItems = new List<Item>();
        foreach(Item item in slots) {
            if (!Item.ReferenceEquals(item,null)) {                
                if (item.IsSellable()) {
                    sellableItems.Add(item); // Won't work in latest test :(
                }
            }
        }
        return sellableItems.ToArray();
    }


    #region Control

    #region Check
    public bool isInventoryFull() {
        if(getFreeFirstSlot() == -1) {
            return true;
        }
        return false;
    }

    public int getFreeFirstSlot() {
        for (int i = 0; i < maxSizeSlot; i++) {
            if(Item.ReferenceEquals(slots[i], null)) {//!Item.ReferenceEquals(slots[i],null)
                Debug.Log("Free at " + i);
                return i; // return index of free slot
            }
        }
        return -1; // full inventory
    }

    public int getSlotIndexFromItemId(int itemId) {
        for (int i = 0; i < maxSizeSlot; i++) {
            if(!Item.ReferenceEquals(slots[i], null))
            if (slots[i].GetItemId() == itemId) {
                return i; // found
            }
        }
        return -1; // not found 
    }
    #endregion

    public int GetMoney() {
        return this.money;
    }

    #region Get
    public Item getItemFromSlot(int index) {
        return slots[index];
    }

    public Item popItemFromSlot(int index) {
        Item item = getItemFromSlot(index);
        DeleteItemFromInventory(index);
        return item;
    }
    #endregion

    #region Add
    /// <summary>
    /// To collect item in normal case
    /// </summary>
    /// <param name="itemGameObject">Item GameObject</param>
    public void CollectItemToInventory(GameObject itemGameObject) {
        CollectItemToInventory(itemGameObject,false);
    }
    /// <summary>
    /// Collect item with defined mode (addByCode)
    /// </summary>
    /// <param name="itemGameObject">Item GameObject</param>
    /// <param name="addByCode">Is add by code</param>
    public void CollectItemToInventory(GameObject itemGameObject,bool addByCode) {
        Item i = itemGameObject.GetComponent<Item>();
        int indexOfItemInInventory = getSlotIndexFromItemId(i.GetItemId());
        if (i.IsStackable() && indexOfItemInInventory > -1) {
            Debug.Log("Increase stack to item Id : " + i.GetItemId());
            slots[indexOfItemInInventory].IncreaseStack(); // increase stack to item that already in inventory
            DGTRemote.GetInstance().RequestInventoryIncreaseItem(i.GetItemId() , 1);
            if(!addByCode)
                Destroy(itemGameObject);            
            NotificationSystem.Instance.AddNotification("คุณได้รับไอเท็ม " + i.GetItemName() + " เพิ่ม");
            UpdateSlotsWindows();
            return;
        } else {
            int indexFree = getFreeFirstSlot(); //หาช่องว่างใน Inventory
            if (indexFree > -1) {
                i.SetIndex(indexFree);
                slots[indexFree] = i; // Add item to first empty slot
                DGTRemote.GetInstance().RequestInventoryAddItem(i.GetItemId() , 1);
                //Add item to UI Slots for when player click to read detail
                Item itemAdded = slotsInventoryUI[indexFree].AddComponent<Item>();
                Item.CopyItem(itemAdded, i);

                if (!addByCode)
                    Destroy(itemGameObject);
                if (slots[indexFree].IsStackable())
                    slots[indexFree].SetItemStack(1); 
                //notification you got item blah blah as new
                Debug.Log("คุณได้รับไอเท็ม " + slots[indexFree].GetItemName());
                NotificationSystem.Instance.AddNotification("คุณได้รับไอเท็ม " + slots[indexFree].GetItemName());
                UpdateSlotsWindows();
                return;
            }
        }
        // Can't collect item maybe full inventory
    }

    public void CollectItemToInventory(Item item) {        
        int indexOfItemInInventory = getSlotIndexFromItemId(item.GetItemId());
        if (item.IsStackable() && indexOfItemInInventory > -1) {
            Debug.Log("Increase stack to item Id : " + item.GetItemId());
            slots[indexOfItemInInventory].IncreaseStack(); // increase stack to item that already in inventory
            DGTRemote.GetInstance().RequestInventoryIncreaseItem(item.GetItemId() , 1);            
            Debug.Log("คุณได้รับไอเท็ม " + item.GetItemName() + " เพิ่ม");
            NotificationSystem.Instance.AddNotification("คุณได้รับไอเท็ม " + item.GetItemName() + " เพิ่ม");
            UpdateSlotsWindows();
            return;
        } else {
            int indexFree = getFreeFirstSlot(); //หาช่องว่างใน Inventory
            if (indexFree > -1) {
                item.SetIndex(indexFree);
                slots[indexFree] = item; // Add item to first empty slot
                DGTRemote.GetInstance().RequestInventoryAddItem(item.GetItemId() , 1);
                //Add item to UI Slots for when player click to read detail
                Item itemAdded = slotsInventoryUI[indexFree].AddComponent<Item>();
                Item.CopyItem(itemAdded , item);                
                if (slots[indexFree].IsStackable())
                    slots[indexFree].SetItemStack(1);
                //notification you got item blah blah as new
                Debug.Log("คุณได้รับไอเท็ม " + slots[indexFree].GetItemName());
                NotificationSystem.Instance.AddNotification("คุณได้รับไอเท็ม " + slots[indexFree].GetItemName());
                UpdateSlotsWindows();
                return;
            }
        }
        // Can't collect item maybe full inventory
    }

    public void IncreasetMoney(int amount) {
        this.money += amount;
        UpdateMoneyToServer();
    }

    public void DecreaseMoney(int amount) {
        this.money -= amount;
        UpdateMoneyToServer();
    }

    #endregion

    #region Delete
    public void DeleteItemFromInventory(int index) {
        DGTRemote.GetInstance().RequestInventoryRemoveItem(slots[index].GetItemId());
        slots[index] = null;
        UpdateSlotsWindows();
    }

    public void DecreaseItemFromInventory(int index) {
        DecreaseItemFromInventory(index , 1);
    }
    //Overload
    public void DecreaseItemFromInventory(int index,int amount) {        
        slots[index].DecreaseStack(amount);
        if (slots[index].GetItemStack() <= 0) {
            DeleteItemFromInventory(index);
        } else {
            DGTRemote.GetInstance().RequestInventoryDecreaseItem(slots[index].GetItemId(), amount);
            UpdateSlotsWindows();
        }
    }
    
    #endregion

    public void UpdateSlotsWindows() {
        for (int i = 0; i < maxSizeSlot; i++) {
            GameObject slotImg = slotsInventoryUI[i].transform.GetChild(0).gameObject;
            GameObject slotAmount = slotsInventoryUI[i].transform.GetChild(1).gameObject; 
            if (!Item.ReferenceEquals(slots[i],null)) {
                slotImg.SetActive(true);
                slotImg.GetComponent<Image>().sprite = slots[i].GetImageItem();
                //amount
                if (slots[i].IsStackable()) {
                    slotAmount.SetActive(true);
                    slotAmount.GetComponent<Text>().text = slots[i].GetItemStack()+"";
                }
            } else {
                slotImg.SetActive(false);
                slotImg.GetComponent<Image>().sprite = null;
                slotAmount.SetActive(false);
                slotAmount.GetComponent<Text>().text = "";
                Destroy(slotsInventoryUI[i].GetComponent<Item>());
            }
            
            
            
        }
    }

    #endregion

    public void UseItem(int index) {
        Debug.Log("Use item in index : " + index);
        if (!Item.ReferenceEquals(slots[index] , null)) {
            Item oldEquipment = null;
            if (slots[index].IsEquipable()) {                
                Item.ItemEquipment equipmentData = slots[index].GetItemEquipment();
                PlayerStatus ps = PlayerStatus.Instance;
                if (equipmentData.head) {
                    oldEquipment = ItemManager.GetItemEquipmentBodyByEquipmentValue(ps.head,ps.gender,ps.job);
                    ps.head = equipmentData.equipmentValue;
                    DGTRemote.GetInstance().RequestChangeEquipment(1, ps.head);
                }else if (equipmentData.body) {
                    oldEquipment = ItemManager.GetItemEquipmentBodyByEquipmentValue(ps.body , ps.gender , ps.job);
                    ps.body = equipmentData.equipmentValue;
                    DGTRemote.GetInstance().RequestChangeEquipment(2 , ps.body);
                } else if (equipmentData.weapon) {
                    oldEquipment = ItemManager.GetItemEquipmentBodyByEquipmentValue(ps.weapon , ps.gender , ps.job);                    
                    ps.weapon = equipmentData.equipmentValue;
                    DGTRemote.GetInstance().RequestChangeEquipment(3 , ps.weapon);
                }                
            } else {
                Item.ItemEffect effect = slots[index].GetItemEffect();
                NotificationSystem.Instance.AddNotification("คุณได้ใช้ไอเท็ม " + slots[index].GetItemName());
                if (effect.GetEffectType() == (int)Item.ItemEffect.EffectId.POSITIVE) {
                    PlayerStatus.Instance.AddHP(effect.GetEffectValue());
                }                
            }
            // remove item from inventory
            if (slots[index].IsStackable()) {
                DecreaseItemFromInventory(index);
            } else {
                DeleteItemFromInventory(index);
            }
            if(oldEquipment)
                CollectItemToInventory(oldEquipment);
        }
    }

    public void UpdateMoneyToServer() {
        DGTRemote.GetInstance().RequestUpdateMoney(this.money);
    }

    public void GetOnlineItemToInventory(int itemID) {
        Debug.Log("Get item id : " + itemID);
        Item item = ItemManager.GetItemComponent(itemID);
        if (item) {            
            CollectItemToInventory(item);
        } else {
            Debug.Log("Not found online item");
        }
    }


    #region Collider
    private void OnTriggerStay2D(Collider2D col) {        
        if (col.CompareTag("ItemDrop")) {
            //CollectItemToInventory(col.gameObject);
            DGTRemote.GetInstance().RequestGetOnlineItem(col.GetComponent<Item>().GetOnlineID(), col.GetComponent<Item>().GetItemId());
        }
    }

    #endregion
}
