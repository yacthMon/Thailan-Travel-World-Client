using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnlinePlayer : MonoBehaviour {
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private Vector2 currentVelocity;
    [SerializeField]
    private bool isReach;
    [SerializeField]
    private static float maxSpeed = 5;

    private Animator onlineplayerAnimator;
    private Vector2 oldPosition;
    private int currentAnimation = 1;
    private bool isJumping = false;

    // animator
    int jumpHash = Animator.StringToHash("Jump");
    int walkHash = Animator.StringToHash("Walk");
    int attackHash = Animator.StringToHash("Attack");
    int hurtHash = Animator.StringToHash("Hurt");
    int dieHash = Animator.StringToHash("Die");
    int respawnHash = Animator.StringToHash("Respawn");
    bool isDie = false;
    void Start () {
        /* Animation
        onlineplayerAnimator = gameObject.GetComponent<Animator>();        
        onlineplayerAnimator.Play("Idle");
        onlineplayerAnimator.SetBool("isWalk", false);
        onlineplayerAnimator.SetBool("isJump", false);
        */
        oldPosition = Vector3.zero;
        onlineplayerAnimator = transform.GetChild(0).GetComponent<Animator>();
    }	

	void Update () {
        if (!isReach) {
            this.transform.Translate(currentVelocity * Time.deltaTime);
            //this.transform.Translate(FindVelocityToTarget());
            //this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, 5f*Time.deltaTime);
            Vector2 distance = FindDistance(targetPosition);
            if(distance.x < 0.01f && distance.y < 0.01f) {
                isReach = true;
                //onlineplayerAnimator.SetBool("isWalk", false);
            }
        }
        /* Animation */
        switch (currentAnimation) {
            case (int)DGTPacket.AnimationId.IDLE:
                if (isDie) {
                    isDie = false;
                    //onlineplayerAnimator.SetTrigger(respawnHash);
                    onlineplayerAnimator.SetBool(dieHash , false);                    
                }
                onlineplayerAnimator.SetBool("Walk" , false);
                onlineplayerAnimator.Play("idle");                
                onlineplayerAnimator.SetBool("OnGround" , true);
                break;
            case (int)DGTPacket.AnimationId.WALK:
                onlineplayerAnimator.SetBool("Walk" , true);
                onlineplayerAnimator.SetBool("OnGround" , true);
                break;
            case (int)DGTPacket.AnimationId.JUMP:
                onlineplayerAnimator.SetTrigger(jumpHash);
                onlineplayerAnimator.SetBool("OnGround" , false);
                isJumping = true;
                break;
            case (int)DGTPacket.AnimationId.FALL:
                onlineplayerAnimator.SetBool("OnGround" , true);
                isJumping = false;
                break;
            case (int)DGTPacket.AnimationId.ATTACK:
                onlineplayerAnimator.SetTrigger(attackHash);
                break;
            case (int)DGTPacket.AnimationId.HURT:
                onlineplayerAnimator.SetTrigger(hurtHash);
                break;
            case (int)DGTPacket.AnimationId.DIE:
                onlineplayerAnimator.SetBool(dieHash , true);
                break;
            case (int)DGTPacket.AnimationId.DIE_LOOP:                
                if (!isDie) {
                    onlineplayerAnimator.Play("dieLoop");
                }
                isDie = true;
                break;            
            default:
                onlineplayerAnimator.SetBool("Walk" , false);
                break;
        }
        oldPosition = transform.position;
	}

    public void MoveTo(Vector2 position,Vector2 velocity) {
        if (targetPosition != position) {            
            targetPosition = position; // set target position to go   
        }
        if (isReach) { 
            isReach = false; // enable to move
        }
        //currentVelocity = velocity; // set current velocity
        currentVelocity.x = (targetPosition.x - transform.position.x) * maxSpeed;
        currentVelocity.y = (targetPosition.y - transform.position.y) * maxSpeed;
    }

    public void SetAnimationState(int animationId) {
        this.currentAnimation = animationId;
    }

    private Vector2 FindDistance(Vector2 vector) {
        Vector2 distance = new Vector2(Mathf.Abs(this.transform.position.x - vector.x), Mathf.Abs(this.transform.position.y - vector.y));
        return distance;
    }

}
