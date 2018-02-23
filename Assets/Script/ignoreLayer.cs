using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreLayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Physics2D.IgnoreLayerCollision(8, 9,true);
        /* 8 Player
         * 9 Monster
         * 10 OnlinePlayer
         * 11 BlockMonster
         * 12 Item
         * 13 Background
         * 14 Platform
         */
        Physics2D.IgnoreLayerCollision(8, 9, true); // player monster
        Physics2D.IgnoreLayerCollision(8, 8, true); // player player
        Physics2D.IgnoreLayerCollision(8, 10, true);// player onlineyPlayer
        Physics2D.IgnoreLayerCollision(8, 11, true);// player BlockMonster
        Physics2D.IgnoreLayerCollision(8, 12, true);// player Item        
        Physics2D.IgnoreLayerCollision(9, 10, true);// monster onlinePlayer
        Physics2D.IgnoreLayerCollision(9, 9, true);// monster monster        
        Physics2D.IgnoreLayerCollision(9, 12, true);// monster Item
        Physics2D.IgnoreLayerCollision(10, 10, true);//onlinePlayer onlinePlayer
        Physics2D.IgnoreLayerCollision(10, 11, true);// onlinePlayer BlockMonster
        Physics2D.IgnoreLayerCollision(12 , 12 , true);// Item Item
        Physics2D.IgnoreLayerCollision(14, 8, true);// Platform player
        Physics2D.IgnoreLayerCollision(14, 9, true);// Platform monster
        Physics2D.IgnoreLayerCollision(14, 10, true);// Platform onlinePlayer 
        Physics2D.IgnoreLayerCollision(14, 11, true);// Platform BlockMonster 
        Physics2D.IgnoreLayerCollision(14 , 12 , true);// Platform Item 
    }	

}
