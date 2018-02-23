using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineMonster : MonoBehaviour {
    [SerializeField]
    private Vector3 worldPosition;
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private bool isReach;
    private bool turnLeft = true;
    private int speed;
    private Animator anim;
	void Start () {
        speed = this.GetComponent<MonsterStatus>().GetMonsterSpeed();
        this.anim = this.gameObject.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        worldPosition = this.transform.position;
        if (!isReach) {
            if(FindDistance(targetPosition) > 0.1f) {
                anim.SetBool("Walk" , true);
                walk(FindDirect(targetPosition));

            } else { //reach
                isReach = true;
                anim.SetBool("Walk" , false);
            }
        }
	}

    public void MoveTo(Vector2 position) {
        if(targetPosition != position) {
            targetPosition = position;
        }
        if (isReach) {
            isReach = false;
        }

    }
    public void walk(float direct) {
        if (direct >= 0) {
            if (turnLeft) {
                turnLeft = false;
                this.transform.localScale = ReverseDirection();
            }
            this.transform.Translate(new Vector2(speed , 0) * Time.deltaTime);
        } else {
            if (!turnLeft) {
                turnLeft = true;
                this.transform.localScale = ReverseDirection();
            }
            this.transform.Translate(new Vector2(-speed , 0) * Time.deltaTime);
        }
    }

    float FindDistance(Vector2 targetPos) {
        return Mathf.Abs(this.transform.position.x - targetPos.x);
    }
    Vector2 ReverseDirection() {
        return new Vector2(-this.transform.localScale.x , this.transform.localScale.y);
    }
    int FindDirect(Vector2 targetPos) {
        int direction;
        if (transform.position.x > targetPos.x) {
            direction = -1;
        } else {
            direction = 1;
        }        
        return direction;
    }
}
