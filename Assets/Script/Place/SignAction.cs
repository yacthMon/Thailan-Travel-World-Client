using UnityEngine.EventSystems;
using UnityEngine;

public class SignAction : MonoBehaviour {
    Animator anim;
    signControl sign;
    [SerializeField]
    GameObject btnSign;
    private void Start() {
        anim = GetComponent<Animator>();
        sign = this.transform.parent.GetComponent<signControl>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => {
            sign.ShowInformation();
        });
        btnSign.GetComponent<EventTrigger>().triggers.Add(entry);
        
    }

    public void ActiveButton() {
        btnSign.SetActive(true);
    }

    public void DisactiveButton() {
        btnSign.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
            anim.SetBool("Show" , true);
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
            anim.SetBool("Show" , false);
    }

}
