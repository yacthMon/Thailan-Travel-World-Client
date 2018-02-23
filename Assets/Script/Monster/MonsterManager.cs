using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    public static string GetMonsterNameByID(int id) {
        GameObject prefabMonster = Resources.Load("Prefab/Monster/" + id) as GameObject;
        if (prefabMonster) {
            return prefabMonster.GetComponent<MonsterStatus>().monsterName;
        }
        return null;
    }
}
