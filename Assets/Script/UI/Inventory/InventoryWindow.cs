using UnityEngine.UI;
using UnityEngine;

public class InventoryWindow : MonoBehaviour {
    [SerializeField]
    private GameObject itemDetailWindow;
    [SerializeField]
    private Button btnUseItem;
    [SerializeField]
    private Text lblMoney;
    private Item selectedItem;

    public void OnClickSlot(GameObject slot) {
        Item item = slot.GetComponent<Item>();
        if (!Item.ReferenceEquals(item, null)) {
            GameObject title = itemDetailWindow.transform.GetChild(0).gameObject;
            GameObject image = itemDetailWindow.transform.GetChild(1).gameObject;
            GameObject detail = itemDetailWindow.transform.GetChild(2).GetChild(0).gameObject;
            GameObject effectDetail = itemDetailWindow.transform.GetChild(3).GetChild(0).gameObject;
            title.GetComponent<Text>().text = item.GetItemName(); ;
            image.GetComponent<Image>().sprite = item.GetImageItem();
            detail.GetComponent<Text>().text = item.GetItemDetail();            
            selectedItem = item;
            if (item.IsUseable()) {
                effectDetail.GetComponent<Text>().text = item.GetItemEffect().GetEffectDetail();
                btnUseItem.interactable = true;
            } else {
                effectDetail.GetComponent<Text>().text = "";
                btnUseItem.interactable = false;
            }
            
        }
    }

    public void UpdateMoney() {
        lblMoney.text = Inventory.Instance.GetMoney()+"";
    }

    public void UseItem() {
        if (!Item.ReferenceEquals(selectedItem , null)) {
            Inventory.Instance.UseItem(selectedItem.GetIndex());
        }
    }

    public void DeleteItem() {
        if (!Item.ReferenceEquals(selectedItem , null)) {            
            if (selectedItem.IsStackable()) {
                Inventory.Instance.DecreaseItemFromInventory(selectedItem.GetIndex());
            } else {
                Inventory.Instance.DeleteItemFromInventory(selectedItem.GetIndex());
            }
        }
    }

    public void ClearSelectedItem() {
        selectedItem = null;
    }

}
