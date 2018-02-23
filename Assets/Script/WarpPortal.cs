using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WarpPortal : MonoBehaviour {
    public string sceneName;
    public Vector3 positionOnNextScene;
    GameObject player;
    FadeScreen fs;
    //ตอนวาป (เริ่มวาป) ก็ให้ clear online player แล้วก็ให้ส่ง request change map ไป
    public void doWarp(GameObject player) {
        if(!this.player)
            this.player = player;
        if(!fs)
            fs = FadeScreen.Instance;
        fs.onFadeOutCompleteHandler += warpPlayerToTargetScene; //move player after screen tunr to black
        //fs.onFadeInCompleteHandler += clearEventHandler; //clear after done
        fs.fadeOut(3);

    }
    private void warpPlayerToTargetScene() {
        SceneManager.LoadScene(sceneName , LoadSceneMode.Single);
        player.GetComponent<PlayerStatus>().currentMap = sceneName;
        player.transform.position = positionOnNextScene;
        OnlinePlayerController.Instance.ClearOnlinePlayer();
        OnlineMonsterController.Instance.ClearOnlineMonster();
        OnlineItemControl.Instance.ClearOnlineItem();
        DGTRemote.GetInstance().RequestPlayerChangeMap(sceneName, positionOnNextScene);
        NotificationSystem.Instance.ClearNotification();
        fs.onFadeOutCompleteHandler -= warpPlayerToTargetScene;
    }

    private void clearEventHandler() {
        fs.onFadeOutCompleteHandler -= warpPlayerToTargetScene;
        fs.onFadeInCompleteHandler -= clearEventHandler;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            doWarp(collision.gameObject);            
        }
    }

}
