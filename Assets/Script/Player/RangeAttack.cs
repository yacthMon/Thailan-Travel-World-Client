using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour {

    public bool attackable;
	void Start () {
		
	}
	
    public void Attack() {
        float direction = (PlayerStatus.Instance.gameObject.transform.localScale.x);//reverse
        Vector2 target = this.transform.position;
        target.x += (direction*-1) * 8; // (reverse first) 8 is range
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Effect/Fisher/Prefab/bullet"));
        bullet.transform.position = this.transform.position;
        bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * direction ,
            bullet.transform.localScale.y ,
            bullet.transform.localScale.z);        
        
        bullet.GetComponent<Bullet>().StartMoving(target , 15, attackable);
        
    }
}
