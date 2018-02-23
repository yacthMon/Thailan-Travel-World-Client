using System;
using System.Collections;
using UnityEngine;

public class NPCShop : MonoBehaviour {
    [SerializeField]
    private string shopTitle;
    public bool isOpen;
    [System.Serializable]
    public class ItemForSale {
        [SerializeField]
        private int itemID;
        [SerializeField]
        private int price;

        public int GetItemID() {
            return this.itemID;
        }

        public int GetPrice() {
            return this.price;
        }
    }
    [SerializeField]
    private ItemForSale[] itemSaleList;

    public void StartShop() {
        ShopSystem.Instance.OpenShop(this.shopTitle , this.itemSaleList);
    }
    

}
