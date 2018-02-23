using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour {
    public InputField playerName;
    public string gender = "male";
    public string job = "farmer";
    public string hair = "1";
    [SerializeField]
    private PlayerAnimation previewCharacter;
    [SerializeField]
    private string[] hairsMale;
    [SerializeField]
    private string[] hairsFemale;
    private int hairSelectorIndex;
    public Button btnCreate;
    public void checkName() {
        if (string.IsNullOrEmpty(playerName.text)) {
            Popup.Instance.showPopup("การสร้างตัวละคร" , "กรุณาใส่ชื่อตัวละคร");
        } else if (string.IsNullOrEmpty(gender)) {
            Popup.Instance.showPopup("การสร้างตัวละคร" , "กรุณาเลือกเพศตัวละคร");
        } else if (string.IsNullOrEmpty(job)){
            Popup.Instance.showPopup("การสร้างตัวละคร" , "กรุณาเลือกอาชีพตัวละคร");
        }else {
            DGTRemote.GetInstance().checkCharacterName(playerName.text);
            btnCreate.interactable = false;
        }
    }

    public void changeGender(string gender) {
        this.gender = gender;
        previewCharacter.ChangeGenderData(gender);
        switch (gender) {
            case "male":
                hairSelectorIndex = 0;
                previewCharacter.ChangeHairData(hairsMale[hairSelectorIndex]);
                hair = hairsMale[hairSelectorIndex];
                break;                
            case "female":
                hairSelectorIndex = 0;
                previewCharacter.ChangeHairData(hairsFemale[hairSelectorIndex]);
                hair = hairsFemale[hairSelectorIndex];
                break;
        }
    }

    public void changeJob(string job) {
        this.job = job;
        previewCharacter.ChangeJobData(job);
    }

    public void NextHair() {
        switch (gender) {            
            case "male":
                try {
                    previewCharacter.ChangeHairData(hairsMale[++hairSelectorIndex]);
                    hair = hairsMale[hairSelectorIndex];
                } catch (System.IndexOutOfRangeException) {
                    hairSelectorIndex = 0;
                    previewCharacter.ChangeHairData(hairsMale[hairSelectorIndex]);
                    hair = hairsMale[hairSelectorIndex];
                }
                break;
            case "female":
                try {
                    previewCharacter.ChangeHairData(hairsFemale[++hairSelectorIndex]);
                    hair = hairsFemale[hairSelectorIndex];
                } catch (System.IndexOutOfRangeException) {
                    hairSelectorIndex = 0;
                    previewCharacter.ChangeHairData(hairsFemale[hairSelectorIndex]);
                    hair = hairsFemale[hairSelectorIndex];
                }
                break;
        }
    }

    public void PreviousHair() {
        switch (gender) {
            case "male":
                try {
                    previewCharacter.ChangeHairData(hairsMale[--hairSelectorIndex]);
                    hair = hairsMale[hairSelectorIndex];
                } catch (System.IndexOutOfRangeException) {
                    hairSelectorIndex = hair.Length - 1;
                    previewCharacter.ChangeHairData(hairsMale[hairSelectorIndex]);
                    hair = hairsMale[hairSelectorIndex];
                }
                break;
            case "female":
                try { 
                previewCharacter.ChangeHairData(hairsFemale[--hairSelectorIndex]);
                hair = hairsFemale[hairSelectorIndex];
                } catch (System.IndexOutOfRangeException) {
                    hairSelectorIndex = hair.Length - 1;
                    previewCharacter.ChangeHairData(hairsFemale[hairSelectorIndex]);
                    hair = hairsMale[hairSelectorIndex];
                }
                break;
        }
    }
}
