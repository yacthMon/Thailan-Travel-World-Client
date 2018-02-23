using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour {
    public bool fixMode;
    public Transform target;
    public bool positionDefined;
    public Vector3 positionTarget;

    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;
    // Offset
    public Vector3 offset;
    public bool x = true, y = true, z = true, isCamera=false;
    // Use this for initialization
    private void Start() {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }


    // Update is called once per frame
    private void Update() {
        if (!positionDefined) {
            if (!fixMode) {
                // only update lookahead pos if accelerating or changed direction
                float xMoveDelta = (target.position - m_LastTargetPosition).x;

                bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

                if (updateLookAheadTarget) {
                    m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
                } else {
                    m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos , Vector3.zero , Time.deltaTime * lookAheadReturnSpeed);
                }

                Vector3 aheadTargetPos = (target.position + offset) + m_LookAheadPos + Vector3.forward * m_OffsetZ;
                Vector3 newPos = Vector3.SmoothDamp(transform.position , aheadTargetPos , ref m_CurrentVelocity , damping);

                transform.position = newPos;

                m_LastTargetPosition = target.position;
            } else {
                Vector3 position = new Vector3(x ? target.position.x + offset.x : this.transform.position.x ,
                    y ? target.position.y + offset.y : this.transform.position.y ,
                    z ? target.position.z + offset.z : this.transform.position.z);
                transform.position = position;
            }
        } else {
            if (!fixMode) {
                // only update lookahead pos if accelerating or changed direction
                float xMoveDelta = (positionTarget - m_LastTargetPosition).x;

                bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

                if (updateLookAheadTarget) {
                    m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
                } else {
                    m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos , Vector3.zero , Time.deltaTime * lookAheadReturnSpeed);
                }

                Vector3 aheadTargetPos = (positionTarget) + m_LookAheadPos + Vector3.forward * m_OffsetZ;
                Vector3 newPos = Vector3.SmoothDamp(transform.position , aheadTargetPos , ref m_CurrentVelocity , damping);

                newPos.z = offset.z;
                transform.position = newPos;

                m_LastTargetPosition = positionTarget;
            } else {
                Vector3 position = new Vector3(x ? positionTarget.x: this.transform.position.x ,
                    y ? positionTarget.y: this.transform.position.y ,
                    z ? positionTarget.z + offset.z : this.transform.position.z);
                transform.position = position;
            }
        }
    }    
}