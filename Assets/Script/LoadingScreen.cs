using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingScreen : MonoBehaviour {
    public delegate void onLoadingScreenClose();
    public delegate void onLoadingScreenClosed();
    public delegate void onLoadingScreenShow();
    public delegate void onLoadingScreenShowed();
    public onLoadingScreenClose onLoadingScreenCloseHandler = null;
    public onLoadingScreenClosed onLoadingScreenClosedHandler = null;
    public onLoadingScreenShow onLoadingScreenShowHandler = null;
    public onLoadingScreenShowed onLoadingScreenShowedHandler = null;
    public static LoadingScreen Instance;
    public GameObject border,loadingCircle,loadingLabel,black;
    public Image fadeImage;
    public float speed=5;
    float preAlpha = 0;
    bool Close;
    bool Show;
    void Start() {
        if (!Instance) {
            Instance = this;
        }
        this.onLoadingScreenShowedHandler += activeComponent;
        this.onLoadingScreenClosedHandler += () => { black.SetActive(false); };
    }

    public void CloseLoading() {
        Show = false;
        Close = true;
        // เอาพวกรูป text loading ออกก่อน
        border.SetActive(false);
        loadingCircle.SetActive(false);
        loadingLabel.SetActive(false);
        if (onLoadingScreenCloseHandler != null)
            onLoadingScreenCloseHandler();
    }
    public void ShowLoading() {
        Show = true;
        Close = false;
        black.SetActive(true);
        if (onLoadingScreenShowHandler != null)
            onLoadingScreenShowHandler();
    }
    private void activeComponent() {        
        border.SetActive(true);
        loadingCircle.SetActive(true);
        loadingLabel.SetActive(true);
    }

    private void Update() {
        if (Show && preAlpha < 1) {
            preAlpha += Time.deltaTime * speed;
            float alpha = Mathf.Lerp(0, 1, preAlpha);
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
            if (alpha >= 1) {
                preAlpha = 0;
                Show = false;
                if (onLoadingScreenShowedHandler != null)
                    onLoadingScreenShowedHandler();
            }
        }
        if (Close && preAlpha < 1) {
            preAlpha += Time.deltaTime * speed;
            float alpha = Mathf.Lerp(1, 0, preAlpha);
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
            if (alpha <= 0) {
                preAlpha = 0;
                Close = false;
                if (onLoadingScreenClosedHandler != null)
                    onLoadingScreenClosedHandler();            
            }
        }
    }
}
