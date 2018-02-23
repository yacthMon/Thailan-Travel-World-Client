using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Map : MonoBehaviour {
    public Sprite currentLocationSprite;
    public RectTransform currentMark;
    public GameObject MarkLocations;

	public void UpdateLocation() {
        DisableAllCityName();
        string currentMap = PlayerStatus.Instance.currentMap;
        foreach(Transform mark in MarkLocations.transform) {
            if(mark.name == currentMap) {
                //Move mark 
                currentMark.position = mark.position;
                currentMark.sizeDelta = mark.GetComponent<RectTransform>().sizeDelta;
                //Enable tootlip (City name)
                if(mark.childCount > 0)
                    mark.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void DisableAllCityName() {
        foreach (Transform mark in MarkLocations.transform) {
            if (mark.childCount > 0) {
                mark.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
