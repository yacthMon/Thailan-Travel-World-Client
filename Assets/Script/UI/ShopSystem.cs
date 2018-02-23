using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour {
    public static ShopSystem Instance;
    // Use this for initialization
    [SerializeField]
    private GameObject shopWindow;    
    [SerializeField]
    private Text shopTitle;
    [Header("Detail")]
    [Space(5)]
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Text playerMoney,itemTitle,itemDetail,itemPrice,itemAmount;
    [SerializeField]
    private GameObject buyButtons, sellButtons;
    

    [Header("Item Buy List")]
    [SerializeField]
    private Transform itemGroup;
    [SerializeField]
    private GameObject btnPreviousPage, btnNextPage;
    [SerializeField]
    private GameObject itemLeft, itemRight;
    
    private NPCShop.ItemForSale[] itemBuyList;
    private Item[] itemSellList;
    private const int itemShowAmount = 4;
    private int page=1;
    public bool isBuyMode = true; //true is Buy Mode , false is Sell Mode

    private Sprite none;

    void Start () {
        none = Resources.Load<Sprite>("Item/Image/none"); 
        if (!Instance) {
            Instance = this;
        }
        btnPreviousPage.GetComponent<Button>().onClick.AddListener(PreviousPage);
        btnNextPage.GetComponent<Button>().onClick.AddListener(NextPage);
    }
	
	public void OpenShop(string shopTitle,NPCShop.ItemForSale[] itemBuyList) {
        this.shopWindow.SetActive(true);
        this.shopTitle.text = shopTitle;
        this.itemBuyList = itemBuyList;
        this.itemSellList = Inventory.Instance.GetSellableItems();
        Debug.Log("Length of sell item : " + this.itemSellList.Length);        
        ChangeMode("buy");
        OpenShop();
    }

    //Overload
    private void OpenShop() {
        UpdateMoney();
        ClearItem();
        ClearItemDetail();
        OpenPage(1);
    }

    private void RefreshItemSellList() {
        this.itemSellList = Inventory.Instance.GetSellableItems();
    }

    public void UpdateMoney() {
        playerMoney.text = Inventory.Instance.GetMoney()+"";
    }

    public void ShowItemDetail(Item item, int price) { // direct call for buy
        ShowItemDetail(item , price , 0);
    }
    //Overload
    public void ShowItemDetail(Item item, int price, int amount) { // direct call for sell
        itemTitle.text = item.GetItemName();
        itemDetail.text = item.GetItemDetail();
        itemImage.sprite = item.GetImageItem();
        itemPrice.text = price + " บาท";
        if (amount > 0) {
            itemAmount.text = "จำนวน : " + amount;
        } else {
            itemAmount.text = "";
        }
        // Action buttons Buy and Sell
        ClearActionButton();
        int playerMoney = Inventory.Instance.GetMoney();
        if (isBuyMode) {            
            if(price <= playerMoney) {// buy x1
                Button btn = buyButtons.transform.GetChild(0).GetComponent<Button>();
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    BuyItem(item , price , 1);
                    ShowItemDetail(item , price);
                });
            }
            if (price*2 <= playerMoney) {// buy x2
                Button btn = buyButtons.transform.GetChild(1).GetComponent<Button>();
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    BuyItem(item , price , 2);
                    ShowItemDetail(item , price);
                });
            }
            if (price*5 <= playerMoney) {// buy x5
                Button btn = buyButtons.transform.GetChild(2).GetComponent<Button>();
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    BuyItem(item , price , 5);
                    ShowItemDetail(item , price);
                });
            }
        } else {
            Button btnSell = sellButtons.transform.GetChild(0).GetComponent<Button>();
            Button btnSellAll = sellButtons.transform.GetChild(2).GetComponent<Button>();
            btnSell.interactable = true;
            btnSell.onClick.RemoveAllListeners();
            btnSell.onClick.AddListener(() => {
                SellItem(item , 1);
            });
            btnSellAll.interactable = true;
            btnSellAll.onClick.RemoveAllListeners();
            btnSellAll.onClick.AddListener(() => {
                SellItem(item , 0); // 0 amount for sell All
            });
            if (amount >= 5) { // sell x5
                Button btnSellFifth = sellButtons.transform.GetChild(1).GetComponent<Button>();
                btnSellFifth.interactable = true;
                btnSellFifth.onClick.RemoveAllListeners();
                btnSellFifth.onClick.AddListener(() => {
                    SellItem(item , 5);
                });
            }
        }
    }   

    public void ChangeMode(string mode) {
        page = 1;
        if (mode == "sell") {
            buyButtons.SetActive(false);
            sellButtons.SetActive(true);
            this.isBuyMode = false;
        } else {
            buyButtons.SetActive(true);
            sellButtons.SetActive(false);
            this.isBuyMode = true;
        }
        OpenShop();
    }

    public void OpenPage(int pageNumber) {        
        ClearItem();
        int latestItem = (pageNumber * itemShowAmount);
        if (pageNumber <= 1) {
            pageNumber = 1;
            btnPreviousPage.SetActive(false); //first page shouldn't show previous btn
        } else { //more than page 1 should have previous btn
            btnPreviousPage.SetActive(true);
        }
        if (isBuyMode) { // Buy mode
            if (latestItem <= itemBuyList.Length) {
                // i = first index of item for current page
                for (int i = latestItem - itemShowAmount; i < latestItem; i++) {
                    GenerateItemBuy(i);
                }
                if (itemBuyList.Length - latestItem == 0) { // have no page left
                    btnNextPage.SetActive(false);
                } else {// still have next page
                    btnNextPage.SetActive(true);
                }
            } else if ((latestItem - itemBuyList.Length) < 4) { // for last page and amount mod itemShowAmount not 0
                for (int i = latestItem - itemShowAmount; i < itemBuyList.Length; i++) {
                    GenerateItemBuy(i);
                    btnNextPage.SetActive(false);//last page shouldn't show next btn
                }
            }
        } else { // Sell mode
            if (latestItem <= itemSellList.Length) {
                // i = first index of item for current page
                for (int i = latestItem - itemShowAmount; i < latestItem; i++) {
                    GenerateItemSell(i);
                }
                if (itemSellList.Length - latestItem == 0) { // have no page left
                    btnNextPage.SetActive(false);
                } else {// still have next page
                    btnNextPage.SetActive(true);
                }
            } else if ((latestItem - itemSellList.Length) < 4) { // for last page and amount mod itemShowAmount not 0
                for (int i = latestItem - itemShowAmount; i < itemSellList.Length; i++) {
                    GenerateItemSell(i);
                    btnNextPage.SetActive(false);//last page shouldn't show next btn
                }
            }
        }
        
    }

    private void GenerateItem(int i) { // abandon
        if (isBuyMode) { // if buy mode generate Item for buy
            GenerateItemBuy(i);
        } else { // if sell mode generate Item for sell
            GenerateItemSell(i);
        }
    }

    private void GenerateItemBuy(int itemIndex) {
        GameObject itemBuy = Instantiate((itemIndex+1) % 2 == 1 ? itemLeft: itemRight);
        Item itemData = ItemManager.GetItemComponent(itemBuyList[itemIndex].GetItemID());        
        Text itemName = itemBuy.transform.GetChild(1).GetComponent<Text>();
        Text itemPrice = itemBuy.transform.GetChild(2).GetComponent<Text>();
        Image itemImage = itemBuy.transform.GetChild(3).GetComponent<Image>();
        itemName.text = itemData.GetItemName();
        itemPrice.text = itemBuyList[itemIndex].GetPrice()+".-";
        itemImage.sprite = itemData.GetImageItem();
        // add on click to place
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { ShowItemDetail(itemData , itemBuyList[itemIndex].GetPrice()); });
        itemBuy.GetComponent<EventTrigger>().triggers.Add(entry);
        // set Parent to place
        itemBuy.transform.SetParent(itemGroup , false);
    }

    private void GenerateItemSell(int itemIndex) { //check if length not 0 too  don't forgot
        GameObject itemSell = Instantiate((itemIndex + 1) % 2 == 1 ? itemLeft : itemRight);        
        Text itemName = itemSell.transform.GetChild(1).GetComponent<Text>();
        Text itemPrice = itemSell.transform.GetChild(2).GetComponent<Text>();
        Image itemImage = itemSell.transform.GetChild(3).GetComponent<Image>();
        Text itemAmount = itemSell.transform.GetChild(4).GetComponent<Text>();
        itemName.text = itemSellList[itemIndex].GetItemName();
        itemPrice.text = itemSellList[itemIndex].GetSellPrice() + ".-";
        itemAmount.text = itemSellList[itemIndex].GetItemStack() + "";
        itemImage.sprite = itemSellList[itemIndex].GetImageItem();
        // add on click to place
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { ShowItemDetail(itemSellList[itemIndex] ,
            itemSellList[itemIndex].GetSellPrice(),
            itemSellList[itemIndex].GetItemStack()); });
        itemSell.GetComponent<EventTrigger>().triggers.Add(entry);
        // set Parent to place
        itemSell.transform.SetParent(itemGroup , false);
    }

    private void ClearItem() {
        foreach(Transform item in itemGroup.transform){
            Destroy(item.gameObject);
        }
    }

    private void ClearItemDetail() {
        itemTitle.text = "";
        itemDetail.text = "";
        itemImage.sprite = none;
        itemPrice.text = "";
        itemAmount.text = "";
        ClearActionButton();
    }

    private void ClearActionButton() {
        if (isBuyMode) {
            ClearBuyButton();
        } else {
            ClearSellButton();
        }
    }

    private void ClearBuyButton() {
        foreach (Transform buyButton in buyButtons.transform) {
            buyButton.GetComponent<Button>().interactable = false;
        }            
    }

    private void ClearSellButton() {
        foreach (Transform sellButton in sellButtons.transform) {
            sellButton.GetComponent<Button>().interactable = false;
        }
    }

    public void NextPage() {
        OpenPage(++page);
    }

    public void PreviousPage() {
        OpenPage(--page);
    }

    public void RefreshShop() {
        ClearItemDetail();
        OpenShop();
        OpenPage(page);
    }

    public void BuyItem(Item item,int price,int amount) {
        int cost = price * amount;
        Inventory.Instance.DecreaseMoney(cost);
        for(int i = 0; i < amount; i++) {            
            Inventory.Instance.CollectItemToInventory(item);
        }
        //Debug.Log("คุณซื้อไอเท็ม ไอดี " + item.GetItemId() + " จำนวน " + amount + " เสียเงินจำนวน " + cost);
        RefreshShop();
    }

    public void SellItem(Item item , int amount) {
        int earn = item.GetSellPrice() * (amount!=0?amount: item.GetItemStack());
        Inventory.Instance.IncreasetMoney(earn);
        if(amount > 0) { 
            Inventory.Instance.DecreaseItemFromInventory(item.GetIndex(), amount);
        } else { // amount == 0 mean sell All
            Inventory.Instance.DeleteItemFromInventory(item.GetIndex());
        }
        //Debug.Log("คุณขายไอเท็ม ไอดี " + item.GetItemId() + " จำนวน " + amount + " ได้รับเงินจำนวน " + earn);        
        RefreshItemSellList();
        RefreshShop();
    }
}
