using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    [System.Serializable]
    public class ItemEffect{
        public enum EffectId {
            NEGATIVE = 0,
            POSITIVE = 1,
            INCREASE = 2
        }
        [SerializeField]
        public int effectType, valueOfEffect, timeOfEffect;
        [SerializeField]
        public string effectDetail;

        public int GetEffectValue() {
            return this.valueOfEffect;
        }

        public string GetEffectDetail() {
            return this.effectDetail;
        }

        public int GetEffectType() {
            return this.effectType;
        }
    }

    [System.Serializable]
    public class ItemEquipment {
        public string equipmentValue,gender,job;        
        public bool head, body, weapon;
    }
    [SerializeField]
    private int itemOnlineID;
    [SerializeField]
    private int itemId;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int itemType;
    [SerializeField]
    private Sprite itemImage;
    [SerializeField]
    private string itemDetail;
    [SerializeField]
    private bool useableItem, equipable;
    [SerializeField]
    private bool stackableItem;
    [SerializeField]
    private int stack;
    [SerializeField]
    private bool sellableItem;
    [SerializeField]
    private int price;
    private int index; // easier manage inventory
    
    [SerializeField]
    private ItemEffect effect;
    [SerializeField]
    private ItemEquipment equipmentData;
    private void Start() {
        if (useableItem) {
            //this.effect = this.GetComponent<ItemEffect>(); 
        }
    }

    public void SetItemEffect(ItemEffect effect) {
        this.effect = effect;
    }

    public void SetOnlineID(int onlineID) {
        this.itemOnlineID = onlineID;
    }

    public ItemEffect GetItemEffect() {
        return this.effect;
    }

    public ItemEquipment GetItemEquipment() {
        return this.equipmentData;
    }

    public void GetItemDataByID(int id) {
        Item itemData = ItemManager.GetItemComponent(id);
        if (itemData) {
            CopyItem(this , itemData);
        }
    }

    public void IncreaseStack() {
        IncreaseStack(1);
    }

    public void IncreaseStack(int number) {
        stack += number;
    }

    public void DecreaseStack() {
        DecreaseStack(1);
    }

    public void DecreaseStack(int number) {
        stack -= number;
    }

    public int GetItemId() {
        return this.itemId;
    }

    public string GetItemName() {
        return this.itemName;
    }

    public int GetItemType() {
        return this.itemType;
    }

    public int GetItemStack() {
        return this.stack;
    }
    
    public bool IsStackable() {
        return this.stackableItem;
    }

    public bool IsUseable() {
        return this.useableItem;
    }

    public bool IsEquipable() {
        return this.equipable;
    }

    public string GetItemDetail() {
        return this.itemDetail;
    }

    public Sprite GetImageItem() {
        return this.itemImage;
    }

    public void SetItemStack(int stack) {
        this.stack = stack;
    }

    public bool IsSellable() {
        return this.sellableItem;
    }

    public int GetSellPrice() {
        return this.sellableItem ? this.price : 0;
    }

    public void SetIndex(int i) {
        this.index = i;
    }

    public int GetOnlineID() {
        return this.itemOnlineID;
    }

    public int GetIndex() {
        return this.index;
    }
    /// <summary>
    /// Copy Item from B to A
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static void CopyItem(Item a, Item b) {
        a.itemId = b.itemId;
        a.itemName = b.itemName;
        a.itemType = b.itemType;
        a.itemImage = b.itemImage;
        a.itemDetail = b.itemDetail;
        a.useableItem = b.useableItem;
        a.equipable = b.equipable;
        a.stack = b.stack;
        a.stackableItem = b.stackableItem;
        a.sellableItem = b.sellableItem;
        a.price = b.price;
        a.index = b.index;
        a.effect = b.effect;
        a.equipmentData = b.equipmentData;
    }

    public override string ToString() {
        return "["+this.itemId +"] " + this.itemName +" Sellable : " + this.sellableItem + " Index ["+this.index+"]";
    }

}
