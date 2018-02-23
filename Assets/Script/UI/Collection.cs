using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class Collection : MonoBehaviour {    
    private const int placeShowAmount = 4;
    public GameObject placePrefab;
    public GameObject pageNumberPrefab;
    [SerializeField]
    private RectTransform placeList;
    [SerializeField]
    private RectTransform pageNumberList;
    public Text cityTitle;
    public Image placeShowImage;
    public Text placeTitle;
    public Text checkInTime;
    public Text placeDescription;    
    List<PlaceData> currentPlaceList;
    int placeAmount;
    int page;
	public void LoadPlaceList(string city) {
        cityTitle.text = city;
        currentPlaceList = PlayerStatus.Instance.GetPlaceList(city);        
        placeAmount = currentPlaceList.Count;
        ClearPageNumberList();
        CreatePageNumber();
        LoadPage(1);
    }

    public void LoadPage(int page) {
        if (page < 1) {
            page = 1;
        }
        ClearPlaceList();
        //lastest place for this page (index)        
        int latestPlace = (page * placeShowAmount);
        if (latestPlace <= placeAmount) {            
            for (int i = latestPlace - placeShowAmount; i < latestPlace; i++) {
                CreatePlaceContent(i);
            }
        } else if ((latestPlace - placeAmount) < 4) {
            //old for (int i = latestPlace - placeShowAmount; i < latestPlace - (latestPlace - placeAmount); i++) {
            for (int i = latestPlace - placeShowAmount; i < placeAmount; i++) {
                CreatePlaceContent(i);
            }
        } else {
            
        }
    }
    
    private void CreatePlaceContent(int i) {
        GameObject place = Instantiate(placePrefab);
        PlaceData data = place.AddComponent<PlaceData>();
        data.Clone(currentPlaceList[i]);
        place.transform.GetChild(1).GetComponent<Image>().sprite = data.placeImage;
        place.transform.GetChild(2).GetComponent<Text>().text = data.placeTitle;
        // add on click to place
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { ShowPlaceDataDetail(data); });
        place.GetComponent<EventTrigger>().triggers.Add(entry);
        // set Parent to place
        place.transform.SetParent(placeList,false);
        
    }

    public void ShowPlaceDataDetail(PlaceData data) {
        placeShowImage.sprite = data.placeImage;
        placeTitle.text = data.placeTitle;
        checkInTime.text = data.time.ToString();
        placeDescription.text = data.placeDescription;
    }

    private void CreatePageNumber() {
        int maxNumber = (placeAmount / 4) + ((placeAmount % 4)>0?1:0);        
        for (int i = 1; i <= maxNumber; i++) {            
            GameObject pageNumber = Instantiate(pageNumberPrefab);
            pageNumber.GetComponent<Text>().text = i+"";
            // add on click to pageNumber
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => {
                LoadPage(int.Parse( pageNumber.GetComponent<Text>().text));
                foreach (Transform number in pageNumberList.transform) {
                    number.GetComponent<Text>().color = Color.black;
                }
                pageNumber.GetComponent<Text>().color = Color.white;
            });
            if (i == 1) {
                pageNumber.GetComponent<Text>().color = Color.white;
            }
            pageNumber.GetComponent<EventTrigger>().triggers.Add(entry);
            pageNumber.transform.SetParent(pageNumberList,false);            
        }
    }

    private void ClearPlaceList() {
        foreach (Transform place in placeList.transform) {
            Destroy(place.gameObject);
        }        
    }
    private void ClearPageNumberList() {
        foreach (Transform number in pageNumberList.transform) {
            Destroy(number.gameObject);
        }
    }
}
