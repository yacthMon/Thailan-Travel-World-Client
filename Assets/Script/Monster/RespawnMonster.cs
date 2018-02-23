using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMonster : MonoBehaviour {
    public GameObject monster;
    public int maxAmount;
    public int currentAmount;
    public int time=5;
    public Transform p1;
    public Transform p2;
    private void Start() {
        InvokeRepeating("respawnCheck", 0, time);
    }
    private void Update() {

    }
    public void respawnCheck() {
        if(currentAmount < maxAmount) {
            while(currentAmount < maxAmount) {
                spawnMonster();
            }
        }
    }
    public void spawnMonster() {        
        Vector3 position = new Vector3(Random.Range(p1.position.x,p2.position.x), 
            Random.Range(p1.position.y,p2.position.y),10);
        GameObject m = Instantiate(monster, position, Quaternion.identity);
        m.GetComponent<MonsterStatus>().onMonsterDieHandler += monsterDie;
        currentAmount++;
    }
    public void monsterDie() {
        this.currentAmount--;
    }

}
