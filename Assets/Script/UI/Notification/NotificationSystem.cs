using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour {

    public static NotificationSystem Instance;
    public ScrollRect scroll;
    private Transform content;
    private GameObject textPrefab;
    private bool scrollToPresent, fadingOut;
	void Start () {
        if (!Instance) {
            Instance = this as NotificationSystem;
        }
        content = transform.GetChild(0);
        scroll = GetComponent<ScrollRect>();
        textPrefab = Resources.Load<GameObject>("UI/Notification/Prefab/Text");
        ClearNotification();
	}

    public void ClearNotification() {
        foreach(Transform noticText in content) {
            Destroy(noticText.gameObject);
        }
    }

    public void AddNotification(string text) {
        GameObject textGameObject = Instantiate(textPrefab);
        textGameObject.GetComponent<Text>().text = text;
        textGameObject.transform.SetParent(content , false);
        StartCoroutine(RefreshContentSize());
    }

    private IEnumerator RefreshContentSize() {
        yield return new WaitForSeconds(0.01f);    
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(360 , 0);//refresh
        scrollToPresent = true;
        yield return new WaitForSeconds(2f);
        fadingOut = true;
    }

    private void Update() {
    /*
        if (Input.GetKeyDown(KeyCode.P)) {
            AddNotification("ทดสอบ notification");
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            ClearNotification();
        }        
    */
        if (scrollToPresent) {
            //Debug.Log(scroll.verticalNormalizedPosition);
            scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition , 0,Time.fixedDeltaTime);
            if(scroll.verticalNormalizedPosition <= 0.002f) {
                scrollToPresent = false;                
            }            
        }        
    }

}
