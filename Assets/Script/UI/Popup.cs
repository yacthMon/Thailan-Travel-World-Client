using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {
    public static Popup Instance;
    public GameObject title;
    public GameObject detail;
    public Button btnOK;
    public Animator popupAnimator;
    private void Start(){
        if (!Instance){
            Instance = this;
        }        
    }


    public void showPopup(string title, string detail){
        this.title.GetComponent<Text>().text = title;
        this.detail.GetComponent<Text>().text = detail;
        popupAnimator.Play("fadeIn");
    }
    public void hidePopup(string title , string detail){
        popupAnimator.Play("fadeOut");
    }

    public void ShowLoading() {
        popupAnimator.Play("Loading_In");
    }

    public void HideLoading() {
        popupAnimator.Play("Loading_Out");
    }
}
