using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    [SerializeField]
    private PlayerStatus ps;
    [SerializeField]
    private string animationState;
    [SerializeField]
    private string frameState;
    [SerializeField]
	private SpriteRenderer front_hair, back_hair, face,body,cloth,shoe,weapon;
    [SerializeField]
    private bool isPreview;
    public GameObject attackPointed;
    public string genderData, jobData,hairData,clothData,shoeData;
    // Body (hair, face, body[fixed]) [Gender]
    private void Start() {
        Animator anim = this.GetComponent<Animator>();
        if (isPreview) {
            anim.SetBool(jobData , true);
        } else {
            anim.SetBool(ps.job , true);
        }
    }

    public void ChangeAnimationState(string state) {
        this.animationState = state;
    }

    public void ChangeFrameState(string state) {
        this.frameState = state;
    }
    
    public void ChangeGenderData(string gender) {
        this.genderData = gender;
    }

    public void ChangeJobData(string job) {
        this.jobData = job;
    }

    public void ChangeHairData(string hair) {
        this.hairData = hair;
    }

    private void LateUpdate() {
        string gender;
        string job;
        string hairName;
        string clothName;
        string shoeName;
        if (isPreview) {
            gender = genderData;
            job = jobData;
            hairName = hairData;
            clothName = clothData;
            shoeName = shoeData;
        } else {
            gender = ps.gender;
            job = ps.job;
            hairName = ps.head;
            clothName = ps.body;
            shoeName = ps.body;
        }        
        
        string weaponName = "1";
        
        if(animationState == "fall") {
            animationState = "jump"; // because use same sprite
        }


        //Update hair,face this process for update sprite to correct gender
        #region Update for Body
        if (animationState == "attack") {
            if (frameState == "Normal" || frameState == "Fight1") {
                //face                
                Sprite newSprite = FindSprite("Character/Body/Face/Normal/" + gender , "face_1");
                if (newSprite)
                    face.sprite = newSprite;

                //front hair
                newSprite = FindSprite("Character/Body/Hair/" + gender +"/"+hairName , "front_" + hairName);
                if (newSprite) {
                    front_hair.sprite = newSprite;
                } else { front_hair.sprite = null; }
                //back hair
                newSprite = FindSprite("Character/Body/Hair/" + gender + "/" + hairName , "back_" + hairName);
                if (newSprite) {
                    back_hair.sprite = newSprite;
                } else { back_hair.sprite = null; }
            } else {
                //face                
                var newSprite = FindSprite("Character/Body/Fight/" + job + "/Face/" + gender , frameState);
                if (newSprite) { 
                    face.sprite = newSprite;
                } else {
                    face.sprite = FindSprite("Character/Body/Face/Normal/" + gender , "face_1");//original idle
                }
                //front hair                
                newSprite = FindSprite("Character/Body/Fight/" + job + "/Hair/" + gender + "/" + hairName , frameState + "_front_" + hairName);
                if (newSprite) {
                    front_hair.sprite = newSprite;
                } else {
                    front_hair.sprite = FindSprite("Character/Body/Hair/" + gender , "front_" + hairName);//original idle
                }
                //back hair
                newSprite = FindSprite("Character/Body/Fight/" + job + "/Hair/" + gender + "/" + hairName , frameState + "_back_" + hairName);
                if (newSprite) {
                    back_hair.sprite = newSprite;
                } else {
                    back_hair.sprite = FindSprite("Character/Body/Hair/" + gender , "back_" + hairName); //original idle
                }
            }
        } else if (animationState == "die" || animationState == "dieLoop") {            
            var newSprite = FindSprite("Character/Body/Die/Hair/" + gender +"/"+ hairName , frameState +"_front_" + hairName);
            if (newSprite) {
                front_hair.sprite = newSprite;
            } else { front_hair.sprite = null; }

            newSprite = FindSprite("Character/Body/Die/Hair/" + gender + "/" + hairName , frameState + "_back_" + hairName);
            if (newSprite) {
                back_hair.sprite = newSprite;
            } else { back_hair.sprite = null; }

        } else { //Not attack,die,dieLoop then it going to be idle,jump,walk,hurt
            //face            
            var newSprite = FindSprite("Character/Body/Face/Normal/" + gender , face.sprite.name);
            if (newSprite)
                face.sprite = newSprite;
            //front hair            
            newSprite = FindSprite("Character/Body/Hair/" + gender , "front_" +hairName);
            if (newSprite) { 
                front_hair.sprite = newSprite;
            } else { front_hair.sprite = null; }
            //back hair
            newSprite = FindSprite("Character/Body/Hair/" + gender , "back_" + hairName);
            if (newSprite) { 
                back_hair.sprite = newSprite;
            } else { back_hair.sprite = null; }            
        }
        #endregion

        //Update Cloth,Shoe,Weapon this process for update sprite to crrect equipment
        #region Update for Equipment
        if (animationState == "attack") {
            //cloth            
            Sprite newSprite = FindSprite("Character/" + job + "/Fight/Cloth/" + clothName + "/" + gender , frameState + "_cloth_" + clothName);
            if (newSprite) {
                cloth.sprite = newSprite;
            } else {
                newSprite = FindSprite("Character/" + job + "/Fight/Cloth" , frameState + "_cloth_" + clothName);
                if (newSprite) {
                    cloth.sprite = newSprite;
                } else {
                    cloth.sprite = FindSprite("Character/" + job + "/Idle/Cloth/" + clothName + "/" + gender , "Idle_cloth_" + clothName); //original idle
                }
            }
            //weapon
            newSprite = FindSprite("Character/" + job + "/Fight/Weapon" , frameState + "_weapon_" + weaponName);
            if (newSprite) {
                weapon.sprite = newSprite;
            } else {
                weapon.sprite = FindSprite("Character/" + job + "/Idle/Weapon" , "Idle_weapon_" + weaponName); //original idle
            }
            //shoe
            newSprite = FindSprite("Character/" + job + "/Fight/Shoe/" + shoeName + "/" + gender , frameState + "_shoe_" + shoeName);
            if (newSprite) {
                shoe.sprite = newSprite;
            } else {
                newSprite = FindSprite("Character/" + job + "/Fight/Shoe" , frameState + "_shoe_" + shoeName);
                if (newSprite) {
                    shoe.sprite = newSprite;
                } else {
                    shoe.sprite = FindSprite("Character/" + job + "/Idle/Shoe/" + shoeName + "/" + gender , "Idle_shoe_" + shoeName); //original idle
                }
            }
        } else if (animationState == "die" || animationState == "dieLoop")  {
            //cloth            
            Sprite newSprite = FindSprite("Character/" + job + "/Die/Cloth/" + clothName + "/" + gender , frameState + "_cloth_" + clothName);
            if (newSprite) {
                cloth.sprite = newSprite;
            } else {
                newSprite = FindSprite("Character/" + job + "/Die/Cloth" , frameState + "_cloth_" + clothName);
                if (newSprite) {
                    cloth.sprite = newSprite;
                }
            }
            //weapon
            newSprite = FindSprite("Character/" + job + "/Die/Weapon" , frameState + "_weapon_" + weaponName);
            if (newSprite) {
                weapon.sprite = newSprite;
            }
            //shoe
            newSprite = FindSprite("Character/" + job + "/Die/Shoe/" + shoeName + "/" + gender , frameState + "_shoe_" + shoeName);
            if (newSprite) {
                shoe.sprite = newSprite;
            } else {
                newSprite = FindSprite("Character/" + job + "/Die/Shoe" , frameState + "_shoe_" + shoeName);
                if (newSprite) {
                    shoe.sprite = newSprite;
                }
            }
        } else {//Not attack,die,dieLoop then it going to be idle,jump,walk
            //cloth            
            Sprite newSprite = FindSprite("Character/" + job + "/" + animationState + "/Cloth/" + clothName+"/"+gender , frameState + "_cloth_" + clothName);            
            if (newSprite) {
                cloth.sprite = newSprite;
            } else {
                newSprite = FindSprite("Character/" + job + "/" + animationState + "/Cloth" , frameState + "_cloth_" + clothName); // original state
                if (newSprite) {
                    cloth.sprite = newSprite;
                } else {
                    cloth.sprite = FindSprite("Character/" + job + "/Idle/cloth/" + clothName + "/" + gender , "Idle_cloth_" + clothName); //original idle
                }
            }
            //weapon
            newSprite = FindSprite("Character/" + job + "/" + animationState + "/weapon" , frameState + "_weapon_" + weaponName);
            if (newSprite) {
                weapon.sprite = newSprite;
            }else {
                weapon.sprite = FindSprite("Character/" + job + "/Idle/weapon" , "Idle_weapon_" + weaponName); //original idle
            }
            //shoe
            newSprite = FindSprite("Character/" + job + "/" + animationState + "/shoe/" + shoeName + "/" + gender , frameState + "_shoe_" + shoeName);
            if (newSprite) {
                shoe.sprite = newSprite;
            } else {
                newSprite = FindSprite("Character/" + job + "/" + animationState + "/shoe" , frameState + "_shoe_" + shoeName);
                if (newSprite) {
                    shoe.sprite = newSprite;
                } else {
                    shoe.sprite = FindSprite("Character/" + job + "/Idle/shoe" , "Idle_shoe_" + shoeName); //original idle
                }
            }
        }
        #endregion
    }

    public void RefreshAttackState() {
        //refresh attack state
    }

    private void DoAttackRange() { 
        attackPointed.GetComponent<RangeAttack>().Attack();
    }

    private void StopControl() {        
        Control.Instance.setControlable(false);
    }

    private void StartControl() {        
        Control.Instance.setControlable(true);

    }
    private Sprite FindSprite(string path, string spriteName) {
        var sprites = Resources.LoadAll<Sprite>(path);
        var newSprite = Array.Find(sprites , item => item.name == spriteName);
        return newSprite;
    }
}
