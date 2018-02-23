using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineMonsterController : MonoBehaviour {
    public static OnlineMonsterController Instance;
    private Dictionary<int , GameObject> onlineMonsterList;
    private GameObject onlineMonster;
    private bool isMapReady = false;
    private void Start() {
        if (!Instance) {
            Instance = this;
            SceneManager.sceneLoaded += (Scene scene , LoadSceneMode mode) => {
                SetMapReady(true);
            };
        }
        this.onlineMonster = this.gameObject;
        onlineMonsterList = new Dictionary<int , GameObject>();
    }

    private bool IsMonsterExist(int id) {
        return onlineMonsterList.ContainsKey(id);
    }

    private GameObject GetOnlineMonster(int id) {
        GameObject onlineMonster = null;
        onlineMonsterList.TryGetValue(id , out onlineMonster);
        return onlineMonster;
    }

    public void RemoveMonster(int ID) {
        if (onlineMonsterList.ContainsKey(ID)) {
            Destroy(GetOnlineMonster(ID));
            onlineMonsterList.Remove(ID);
        }
    }

    public void SetMapReady(bool state) {
        if (state) {
            isMapReady = true;
            foreach (var monster in onlineMonsterList) {
                monster.Value.GetComponent<Rigidbody2D>().simulated = true;
            }
        } else {
            isMapReady = false;
            foreach (var monster in onlineMonsterList) {
                monster.Value.GetComponent<Rigidbody2D>().simulated = false;
            }
        }
    }

    public void ClearOnlineMonster() {
        foreach (var item in onlineMonsterList) {
            Destroy(item.Value);
        }
        SetMapReady(false);
        foreach(Transform monster in onlineMonster.transform) {
            Destroy(monster.gameObject);
        }
        onlineMonsterList.Clear();
    }

    public void AddNewMonster(int ID , int monsterID , string monsterName, int level, int HP,int maxHP,int ATK,int DEF, int movementSpeed, Vector2 position , Vector2 targetPosition) {
        var monsterPrefab = Resources.Load<GameObject>("Prefab/Monster/" + monsterID);
        if (monsterPrefab) {
            GameObject newMonster = Instantiate(monsterPrefab , position , Quaternion.identity);
            if (!isMapReady) {
                newMonster.GetComponent<Rigidbody2D>().simulated = false;
            }
            MonsterStatus ms = newMonster.GetComponent<MonsterStatus>();
            ms.SetID(ID);
            ms.AddInformation(monsterID , monsterName);            
            ms.AddStatus(level , HP, maxHP, ATK , DEF);
            ms.SetMonsterSpeed(movementSpeed);
            newMonster.GetComponent<OnlineMonster>().MoveTo(targetPosition);
            newMonster.transform.SetParent(this.onlineMonster.transform);

            onlineMonsterList.Add(ID , newMonster);
        } else {
            Debug.Log("Not found monster prefab ID : " + monsterID);
        }
    }

    public void UpdateOnlineMonster(int id, Vector2 targetPosition) {
        if (IsMonsterExist(id)) {
            GameObject monster = GetOnlineMonster(id);
            monster.GetComponent<OnlineMonster>().MoveTo(targetPosition);
        }
    }

    public void UpdateOnlineMonsterHurt(int id , int damage , int hpLeft , int knockback) {
        if (IsMonsterExist(id)) {
            GameObject monster = GetOnlineMonster(id);
            monster.GetComponent<MonsterStatus>().doHurt(damage , hpLeft , knockback);
        }
    }

    public void EliminateMonster(int id, Item[] itemDrop) {
        if (IsMonsterExist(id)) {
            GameObject monster = GetOnlineMonster(id);
            monster.GetComponent<MonsterStatus>().Eliminate();
            DropItem di = monster.GetComponent<DropItem>();            
            di.doDropItem(itemDrop);
        }
    }
}
