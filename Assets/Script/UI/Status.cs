using UnityEngine.UI;
using UnityEngine;
using System;

public class Status : MonoBehaviour {
    public Text characterName;
    public Text alias;
    public Text status;
    [SerializeField]
    private Image front_hair, back_hair, face, body, cloth, shoe, weapon;
    public void refreshStatus() {
        PlayerStatus ps = PlayerStatus.Instance;
        string job = ps.job;
        string jobTH = "unknow";
        switch (job) {
            case "farmer": jobTH = "ชาวนาไทย"; break;
            case "fisher": jobTH = "ชาวประมง"; break;
            case "boxer": jobTH = "นักมวยไทย"; break;
        }
        
        characterName.text = ps.playerName;
        alias.text = "ไม่มีฉายาในขณะนี้";
        status.text = ps.level + "\r\n" + ps.playerEXP + "/" + ps.maxEXP + "\r\n" +
            jobTH + "\r\n" +
            ps.playerHP + "/" + ps.maxHP + "\r\n" +
            ps.playerSP + "/" + ps.maxSP + "\r\n" +
            ps.atk + "\r\n" +
            ps.def;
        // Preview Character
        job = ps.job;
        string gender = ps.gender;
        string hairName = ps.head;
        string clothName = ps.body;
        string weaponName = "1";
        string shoeName = ps.body;
        //face                
        Sprite newSprite = FindSprite("Character/Body/Face/Normal/" + gender , "face_1");
        if (newSprite)
            face.sprite = newSprite;
        //front hair
        newSprite = FindSprite("Character/Body/Hair/" + gender , "front_" + hairName);
        if (newSprite) {
            front_hair.gameObject.SetActive(true);
            front_hair.sprite = newSprite;
        } else { front_hair.sprite = null; front_hair.gameObject.SetActive(false); }
        //back hair
        newSprite = FindSprite("Character/Body/Hair/" + gender , "back_" + hairName);
        if (newSprite) {
            back_hair.gameObject.SetActive(true);
            back_hair.sprite = newSprite;
        } else { back_hair.sprite = null; back_hair.gameObject.SetActive(false); }
        //cloth
        cloth.sprite = FindSprite("Character/" + job + "/Idle/Cloth" , "Idle_cloth_" + clothName);// original idle
        //weapon
        weapon.sprite = FindSprite("Character/" + job + "/Idle/Weapon" , "Idle_weapon_" + weaponName); //original idle
        //shoe
        shoe.sprite = FindSprite("Character/" + job + "/Idle/Shoe" , "Idle_shoe_" + shoeName); //original idle
    }

    private Sprite FindSprite(string path , string spriteName) {
        var sprites = Resources.LoadAll<Sprite>(path);
        var newSprite = Array.Find(sprites , item => item.name == spriteName);
        return newSprite;
    }
}
