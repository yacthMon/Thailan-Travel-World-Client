using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayerController : MonoBehaviour {
    [Header("Setting")]
    public int distance;
    [Space(10)]
    [Header("GameObject")]
    [SerializeField]
    private GameObject onlinePlayers;
    public GameObject onlinePlayerPrefab;
    private Dictionary<int, GameObject> onlinePlayerList;
    public static OnlinePlayerController Instance;
    private int customSort = 0;

	void Start () {        
        if (!Instance) {
            Instance = this;
        }        
        onlinePlayerList = new Dictionary<int, GameObject>();
        onlinePlayers = this.gameObject;//new GameObject("Online Players");
        //onlinePlayers.transform.SetParent(this.transform);
	}

    public bool IsPlayerExist(int uid) {
        return onlinePlayerList.ContainsKey(uid);
    }

    public GameObject GetOnlinePlayer(int uid) {
        GameObject onlinePlayer = null;
        onlinePlayerList.TryGetValue(uid, out onlinePlayer);
        return onlinePlayer;
    }

    public void NewOnlinePlayer(int uid, string playerName, Vector2 position, string gender,string job , int HP , int SP ,  int level , string equipmentHead , string equipmentBody , string equipmentWeapon) {
        if(!onlinePlayerList.ContainsKey(uid))
        {
            GameObject newPlayer = Instantiate(onlinePlayerPrefab, position, Quaternion.identity);
            PlayerStatus ps = newPlayer.GetComponent<PlayerStatus>();            
            ps.playerID = uid;
            ps.playerName = playerName;
            ps.level = level;
            ps.gender = gender;
            ps.job = job;
            ps.playerHP = HP;
            ps.playerSP = SP;
            ps.head = equipmentHead;
            ps.body = equipmentBody;
            ps.SetToOnlinePlayer();
            // set sorting order
            ps.updateLayer(customSort++);            
            // set name for characterUI
            newPlayer.GetComponent<CharacterUI>().targetName = playerName;
            newPlayer.transform.SetParent(onlinePlayers.transform);
            onlinePlayerList.Add(uid, newPlayer);
        }
    }

    public void RemoveOnlinePlayer(int uid) { 
        if (onlinePlayerList.ContainsKey(uid)) {
            Destroy(GetOnlinePlayer(uid));
            onlinePlayerList.Remove(uid);
        }
    }

    public void ClearOnlinePlayer() {
        foreach (var item in onlinePlayerList) {
            Destroy(item.Value);
        }
        customSort = 0;
        onlinePlayerList.Clear();
    }

    public void UpdateOnlinePlayer(int uid,Vector2 position,Vector2 velocity,float scalex, int animationId) {        
        if (IsPlayerExist(uid)){ //check if player exist from uid            
            GameObject onlinePlayer = GetOnlinePlayer(uid);
                if(scalex != onlinePlayer.transform.localScale.x)
                    onlinePlayer.transform.localScale = new Vector3(scalex, transform.localScale.y, transform.localScale.z);
            onlinePlayer.GetComponent<OnlinePlayer>().MoveTo(position,velocity);            
            onlinePlayer.GetComponent<OnlinePlayer>().SetAnimationState(animationId);
        }      
    }

    public void UpdateOnlinePlayerChangeEquipment(int uid, int part, string value) {
        GameObject onlinePlayer = GetOnlinePlayer(uid);
        if (onlinePlayer) {
            switch (part) {
                case 1: onlinePlayer.GetComponent<PlayerStatus>().head = value; break;
                case 2: onlinePlayer.GetComponent<PlayerStatus>().body = value; break;
                case 3: onlinePlayer.GetComponent<PlayerStatus>().weapon = value; break;
            }
        }
    }

}
