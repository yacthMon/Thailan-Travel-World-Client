using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class NPCActionUI : MonoBehaviour {
    Animator anim;
    NPCStatus npcStatus;
    NPCShop npcShop;
    signControl sign;
    [SerializeField]
    bool signInformation;
    [SerializeField]
    GameObject btnTalk, btnShop, btnSign;
    private void Start() {        
        anim = GetComponent<Animator>();
        if (!signInformation) {
            npcStatus = this.transform.parent.GetComponent<NPCStatus>();
            npcShop = this.transform.parent.GetComponent<NPCShop>();

            if (npcStatus) {
                // add on click to place
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((eventData) => {
                    Debug.Log("Start Talk");
                    npcStatus.StartTalk();
                });
                btnTalk.GetComponent<EventTrigger>().triggers.Add(entry);
            }

            if (npcShop) {
                // add on click to place
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((eventData) => {
                    Debug.Log("Start Shop");
                    npcShop.StartShop();
                });
                btnShop.GetComponent<EventTrigger>().triggers.Add(entry);
            }
        } else {
            sign = this.transform.parent.GetComponent<signControl>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => {
                sign.ShowInformation();
            });
            btnShop.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    public void ActiveButton() {
        if (npcStatus) {
            btnTalk.SetActive(true);
        }
        if (npcShop) {
            btnShop.SetActive(true);
        }
    }

    public void DisactiveButton() {
        btnTalk.SetActive(false);
        btnShop.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.CompareTag("Player"))
            anim.SetBool("Show" , true);
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
            anim.SetBool("Show" , false);
    }

}
