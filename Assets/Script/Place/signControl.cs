using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class signControl : MonoBehaviour {
    public int placeID;
    public string title;
    [TextArea]
    public string detail;
    [Header("Collection")]
    public Sprite imgCollection;
    public string city;
    [Header("GPS Checkin")]
    public int itemID;
    public bool equipment;
    public string equipmentValue;
    public bool isLocationEnable=false;
    public GPSSystem.Point[] points;
    [Header("Camera Show Information")]
    public Vector2 cameraPosition;
    public float cameraSize;
    private bool animateCamera,toView;
    private Camera currentCamera;
    private float timeLerp;
    public float tempCameraSize;
    private GameObject gameplayUI,informationUI,titlePlaceText,detailPlaceText;
	// Use this for initialization
	void Start () {
        GameObject playerSystem = PlayerStatus.Instance.transform.root.gameObject;
        gameplayUI = playerSystem.transform.GetChild(1).GetChild(1).gameObject;
        informationUI = playerSystem.transform.GetChild(1).GetChild(3).gameObject;
        titlePlaceText = informationUI.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        detailPlaceText = informationUI.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).gameObject;
    }
    
    public void DoCheckIn() {
        if (!PlayerStatus.Instance.CheckExistCheckin(placeID)) {
            if (isLocationEnable) {
                GPSSystem gps = GPSSystem.Instance;
                if (!gps.dataReady) {
                //if (false) {
                    Popup.Instance.showPopup("Checkin" , "GPS ยังไม่พร้อมใช้งาน \r\nกรุณาลองใหม่อีกครั้งภายหลัง");
                    gps.startGPSService();
                } else {
                    if (GPSSystem.IsInPolygon(points, gps.getCurrentLocationPoint())) {
                    PlayerStatus ps = PlayerStatus.Instance;
                    //if (true) {
                        // Get Special Item                    
                        //Debug.Log("You got item");
                        PlaceData pd = new PlaceData(placeID , title , detail , city , imgCollection);
                        pd.SetTimeNow();
                        ps.AddPlaceToList(pd);
                        DGTRemote.GetInstance().RequestSendCheckin(placeID , pd.time);
                        if (itemID != 0) {
                            //Debug.Log("Give item by Checkin");
                            // OLD
                            /*GameObject reward = ItemManager.GetItemGameObject(itemID);
                            Popup.Instance.showPopup("Checkin" , "Checkin ที่ " + title +
                                "สำเร็จ. \r\nคุณได้รับไอเท็ม " + reward.GetComponent<Item>().getItemName() );
                            Inventory.Instance.CollectItemToInventory(reward, true);*/
                            Item reward = ItemManager.GetItemComponent(itemID);
                            Popup.Instance.showPopup("Checkin" , "Checkin ที่ " + title +
                            "สำเร็จ. \r\nคุณได้รับไอเท็ม " + reward.GetItemName());
                            Inventory.Instance.CollectItemToInventory(reward);
                        } else if(equipment){
                            Item equipmentReward = ItemManager.GetItemEquipmentBodyByEquipmentValue(equipmentValue,ps.gender,ps.job);
                            Popup.Instance.showPopup("Checkin" , "Checkin ที่ " + title +
                            "สำเร็จ. \r\nคุณได้รับไอเท็ม " + equipmentReward.GetItemName());
                            Inventory.Instance.CollectItemToInventory(equipmentReward);
                        }
                    } else {
                        Popup.Instance.showPopup("Checkin" , "Checkin ล้มเหลว \r\nคุณไม่ได้อยู่ในสถานที่ดังกล่าว");
                        // You not stay in this place :(
                        //Debug.Log("You not in this place :(");                    
                    }
                }
            } else {
                Popup.Instance.showPopup("Checkin " + title , "ยังไม่สามารถ Checkin สถานที่นี้ได้ \r\n ขออภัยในความไม่สะดวก");
            }
        } else {
            Popup.Instance.showPopup("Checkin " + title , "คุณเคย Checkin สถานที่นี้ไปแล้ว");
        }
    }

    public void ShowInformation() {
        // Set information data
        titlePlaceText.GetComponent<Text>().text = title;
        detailPlaceText.GetComponent<Text>().text = detail;
        gameplayUI.SetActive(false);
        informationUI.SetActive(true);
        // Add ShowPlayer to back button
        informationUI.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ShowPlayer);
        // Add Checkin to checkin button
        informationUI.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(DoCheckIn);

        currentCamera = Camera.main;
        currentCamera.GetComponent<follow>().positionTarget = (Vector3)cameraPosition;
        currentCamera.GetComponent<follow>().positionDefined = true;
        tempCameraSize = 6;// original camera have size 6
        timeLerp = 0;
        toView = true;
        animateCamera = true;
        
    }

    public void ShowPlayer() {
        // move scroll back to top
        informationUI.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        gameplayUI.SetActive(true);
        informationUI.SetActive(false);
        // Add ShowPlayer to back button
        informationUI.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        // Add Checkin to checkin button
        informationUI.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

        currentCamera.GetComponent<follow>().positionDefined = false;
        timeLerp = 0;
        toView = false;
        animateCamera = true;
    }
    
    void Update() {
        if (animateCamera) {
            if (toView) {
                currentCamera.orthographicSize = Mathf.Lerp(currentCamera.orthographicSize , cameraSize , timeLerp);
                if (currentCamera.orthographicSize == cameraSize) {
                    animateCamera = false;
                }                
            } else {
                currentCamera.orthographicSize = Mathf.Lerp(currentCamera.orthographicSize , tempCameraSize , timeLerp);
                if (currentCamera.orthographicSize == tempCameraSize) {
                    animateCamera = false;
                }
            }
            timeLerp += 0.1f * Time.deltaTime;
        }
    }

    public PlaceData GetPlaceData() {
        return new PlaceData(placeID,title , detail , city , imgCollection);
    }
}
