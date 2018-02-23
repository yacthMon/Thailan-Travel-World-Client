using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTravel : MonoBehaviour {
    [SerializeField]
    private GameObject leftQuickTravel, rightQuickTravel;
    [SerializeField]
    private GameObject leftButton, rightButton;

    private Animator anim;
    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void ActiveButton() {
        leftButton.SetActive(leftQuickTravel?true:false);
        rightButton.SetActive(rightQuickTravel ? true : false);
    }

    public void DisactiveButton() {
        leftButton.SetActive(false);
        rightButton.SetActive(false);
    }

    public void วาปไปทางขวา() {
        PlayerStatus.Instance.gameObject.transform.position = rightQuickTravel.transform.position;
    }
    
    public void วาปไปทางซ้าย() {
        PlayerStatus.Instance.gameObject.transform.position = leftQuickTravel.transform.position;
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
