using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public bool attacked=false; // ใช้สำหรับเช็คว่าการโจมตีนี้ โจมตีโดนรึยัง
    public bool onlineOtherPlayer;

    private void Start() {
        onlineOtherPlayer = transform.GetComponentInParent<PlayerStatus>().IsOnlineOtherPlayer();    
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (!onlineOtherPlayer) {
            if (col.CompareTag("Monster") && !attacked) {
                if (col.GetComponent<MonsterStatus>().isAlive) {
                    col.SendMessage("getHurt" , this.transform.parent.transform.parent.gameObject);
                    attacked = true;
                    Invoke("ReAttacked" , 0.7f);
                }
            }
        }
    }   

    public void ReAttacked() {
        attacked = false;
    }
}
