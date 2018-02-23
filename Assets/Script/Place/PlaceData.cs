using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceData : MonoBehaviour {
    public int placeID { get; set; }
    public string placeTitle { get; set; }
    public string placeDescription { get; set; }
    public string city { get; set; }
    public string time { get; set; }
    public Sprite placeImage { get; set; }

    public PlaceData() {

    }

    public PlaceData(int id,string title , string description , string city , Sprite img , string time) {
        this.placeTitle = title;
        this.placeDescription = description;
        this.city = city;
        this.placeImage = img;
        this.time = time;
    }

    public PlaceData(int id,string title , string description , string city , Sprite img) {
        placeTitle = title;
        placeDescription = description;
        this.city = city;
        this.placeImage = img;
    }

    public void SetTimeNow() {
        this.time = DateTime.Now.ToString();
    }

    public void SetTime(string time) {
        this.time = time;
    }

    public void SetImage(Sprite image) {
        this.placeImage = image;
    }

    public void Clone(PlaceData pd) {
        this.placeTitle = pd.placeTitle;
        this.placeDescription = pd.placeDescription;
        this.city = pd.city;
        this.time = pd.time;
        this.placeImage = pd.placeImage;
    }

}
