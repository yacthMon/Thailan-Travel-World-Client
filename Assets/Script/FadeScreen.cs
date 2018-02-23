using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour {
    public delegate void onFadeInComplete();
    public delegate void onFadeOutComplete();
    public onFadeInComplete onFadeInCompleteHandler = null;
    public onFadeOutComplete onFadeOutCompleteHandler = null;
    public GameObject fade;
    public float speed;
    public float depth;
    public static FadeScreen Instance;
    float preAlpha = 0;
    bool In;
    bool Out;
    Image fadeimg;
    private void Start() {
        if (!Instance) {
            Instance = this;
        }
        fadeimg = fade.GetComponent<Image>();
    }
    public void fadeOut(float time) {
        Out = true;
        In = false;
        Control.Instance.controlable = false;
        Invoke("fadeIn", time);
    }
    public void fadeIn() {
        Out = false;
        In = true;
        Control.Instance.controlable = true;
    }
    
	private void Update () {
            if (Out &&preAlpha < 1) {
                preAlpha += Time.deltaTime * speed;
                float alpha = Mathf.Lerp(0, 1, preAlpha);
                Color c = fadeimg.color;
                c.a = alpha;
                fadeimg.color = c;
                if (alpha >= 1) {
                    preAlpha = 0;
                    Out = false;
                    if (onFadeOutCompleteHandler != null)
                        onFadeOutCompleteHandler();
                }
            }
            if (In && preAlpha < 1) {
                preAlpha += Time.deltaTime * speed;
                float alpha = Mathf.Lerp(1, 0, preAlpha);
                Color c = fadeimg.color;
                c.a = alpha;
                fadeimg.color = c;
                if (alpha <= 0) {
                    preAlpha = 0;
                    In = false;
                    if (onFadeInCompleteHandler != null)
                        onFadeInCompleteHandler();
                }
            }
    }
}
