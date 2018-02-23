using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ActionTool : ScriptableObject {
    
    public static void KnockBack(GameObject source, GameObject target, Vector2 force) {
        Vector2 targetPosition = target.transform.position;
        Vector2 sourcePosition = source.transform.position;
        if (targetPosition.x > sourcePosition.x) {
            // โดนชนจากด้านขวาของ Player
            KnockBack(source, new Vector2(-force.x, force.y));
        } else if (targetPosition.x < sourcePosition.x) {
            // โดนชนจากด้านซ้ายของ Player
            KnockBack(source , new Vector2(force.x, force.y));
        }
    }
    
    public static void KnockBack(GameObject target , Vector2 force) {
        target.GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x , force.y) , ForceMode2D.Force);
    }


}
