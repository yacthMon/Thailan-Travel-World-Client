using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterAI : MonoBehaviour {
    monsterControl mc;
    MonsterStatus ms;
    bool angry = false;
    GameObject target;
    int direct;
	// Use this for initialization
	void Start () {
        mc= this.GetComponent<monsterControl>();
        ms = this.GetComponent<MonsterStatus>();
        ms.onMonsterHurtHandler += angryAttacker;
        if(!ms.isBoss)
        mc.onMonsterHitBlockHandler += cancelAngry;
        this.Invoke("doWalk",3f);
	}
    public void angryAttacker(GameObject tar) {
        if (!GameObject.ReferenceEquals(tar, target)) {
            mc = this.GetComponent<monsterControl>();
            ms = this.GetComponent<MonsterStatus>();
            this.target = tar;
            this.angry = true;
            this.mc.stopWalking();
            this.mc.follow(tar);
            // add Cancel Angry to onPlayerDieHandler
            PlayerStatus.Instance.onPlayerDieHandler += this.cancelAngry;             
            ms.onMonsterDieHandler += () => {
                this.CancelInvoke("doWalk");
                PlayerStatus.Instance.onPlayerDieHandler -= this.cancelAngry;
            };// add Cancel Angry to onMonsterDieHandler 
        }
        this.CancelInvoke("doWalk");
    }
    public void cancelAngry() {
        this.target = null;
        this.angry = false;
        this.mc.stopWalking();
        this.Invoke("doWalk", 3f);
    }
    public void doWalk() {
        mc.move(Random.Range(-10,10));
        this.Invoke("doWalk", Random.Range(4, 10));
    }

}
