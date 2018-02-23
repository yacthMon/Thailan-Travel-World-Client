using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector2 target;
    public bool hasTarget;
    public int speed;
    public bool attackable;
    public void StartMoving(Vector2 target, int speed, bool attackable) {
        this.target = target;
        this.speed = speed;
        this.attackable = attackable;
        hasTarget = true;
    }
	
	void Update () {
        if (hasTarget) {
            transform.position = Vector2.MoveTowards(transform.position , target , speed * Time.deltaTime);
        }
        if(Mathf.Abs(transform.position.x-target.x) < 0.1f) {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Monster") && attackable) {
            if (col.GetComponent<MonsterStatus>().isAlive) {
                col.SendMessage("getHurt" , PlayerStatus.Instance.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
