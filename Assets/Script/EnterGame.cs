using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour {
    private AsyncOperation asyncLoader = null;
    private void Start() {
        //EnterWorld("Bangkok");
        //PlayerStatus.Instance.gameObject.GetComponent<Rigidbody2D>().simulated = false;
    }
    public void EnterWorld(string sceneName) {
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        StartCoroutine(LoadLevel(sceneName));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private IEnumerator LoadLevel(string Level) {
        asyncLoader = SceneManager.LoadSceneAsync(Level);
        yield return asyncLoader;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //Finished loading scene
        SceneManager.sceneLoaded -= OnSceneLoaded;
        LoadingScreen.Instance.CloseLoading();
        PlayerStatus.Instance.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        PlayerStatus.Instance.onStatusChangeHandler += () => {
            //if status change then send to server
            DGTRemote.GetInstance().RequestSendPlayerStatusData(PlayerStatus.Instance);
        };
    }
}
