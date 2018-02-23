using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickGame: MonoBehaviour {
    public Slider ProgressBar;
    public Text ProgressText;
    private AsyncOperation asyncLoader = null;

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void onClickButtonGame() {
        StartCoroutine(LoadLevel("ChooseCharacter"));
    }

    //CoRoutine to return async progress, and trigger level load.
    public IEnumerator LoadLevel(string Level) {
        GameObject loginPanel = GameObject.Find("Login");
        if (loginPanel) { loginPanel.SetActive(false); }
        asyncLoader = SceneManager.LoadSceneAsync(Level);
        //asyncLoader.allowSceneActivation = false;
        yield return asyncLoader;        
    }
    
    private void OnGUI() {
        // Check if it's loading;
        if (asyncLoader != null) {
            if (!asyncLoader.isDone) {
                if (!ProgressBar.gameObject.active)
                    ProgressBar.gameObject.SetActive(true);
                ProgressBar.value = asyncLoader.progress;
                ProgressText.text = (asyncLoader.progress * 100).ToString("00") + "%";                
            }
            //GUI.DrawTexture(new Rect(screenSize.x*0.25f, screenSize.y*0.75f, 100, 50), ProgressBarEmpty);
            //GUI.Label(new Rect(screenSize.x * 0.25f, screenSize.y * 0.75f, 100, 50), (async.progress * 100).ToString().Substring(0,1)+"%",guiStyle); 
            //GUI.DrawTexture(new Rect(screenSize.x * 0.25f, screenSize.y * 0.75f, async.progress*100, 50), ProgressBarFull);
            //Debug.Log(async.progress);
        }
    }

    public void EnterLevel() {
            asyncLoader.allowSceneActivation = true;        
    }
}
