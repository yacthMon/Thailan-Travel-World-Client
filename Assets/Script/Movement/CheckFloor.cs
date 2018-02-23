using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFloor : MonoBehaviour {
    public bool isOnFloor;    
    private void OnTriggerStay2D(Collider2D col) {
        if (col.CompareTag("Floor"))
            isOnFloor = true;
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("Floor"))
            isOnFloor = false;

    }
}
