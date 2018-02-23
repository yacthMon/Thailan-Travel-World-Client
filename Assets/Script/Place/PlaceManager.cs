using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour {
    public static PlaceData GetPlaceDataFromPlaceID(int placeID) {
        GameObject place = Resources.Load<GameObject>("Sign info/prefab/label_"+placeID);
        if (place) {
            return place.GetComponent<signControl>().GetPlaceData();
        }
        return null;
    }
}
