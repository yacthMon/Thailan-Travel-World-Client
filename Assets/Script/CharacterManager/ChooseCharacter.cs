using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChooseCharacter : MonoBehaviour {
    PlayerData.Character selectCharacter;
    PlayerData.Character[] characters;
    public GameObject characterPreview;
    public GameObject characterName;
    public GameObject characterDetail;
    public GameObject characterMoney;
    public GameObject playerSystem;
    public LoadingScreen loading;
    public GameObject uiChoose;
	void Start () {
        characters = PlayerData.Instance.getCharacter();
        if (characters != null && characters.Length > 0) {
            // This account have character :D
            Debug.Log("Have character");
            previewCharacter(characters[0]);
            //previewCharacter(selectCharacter);
            //Detail
            uiChoose.transform.GetChild(1).gameObject.SetActive(true);
            //Confirm button
            uiChoose.transform.GetChild(2).gameObject.SetActive(true);
            //Create button
            uiChoose.transform.GetChild(3).gameObject.SetActive(false);
            //Delete button
            //uiChoose.transform.GetChild(4).gameObject.SetActive(true);
        } else {
            // This account have no character :(
            // must send to Create Character
            Debug.Log("Don't have character");
            //Detail
            uiChoose.transform.GetChild(1).gameObject.SetActive(false);
            //Confirm button
            uiChoose.transform.GetChild(2).gameObject.SetActive(false);
            //Create button
            uiChoose.transform.GetChild(3).gameObject.SetActive(true);
            //Delete button
            uiChoose.transform.GetChild(4).gameObject.SetActive(false);
        }
	}    

    public void previewCharacter(PlayerData.Character prCharacter) {        
        // UI Detail
        characterName.GetComponent<Text>().text = prCharacter.GetCharacterName();
        characterMoney.GetComponent<Text>().text = prCharacter.GetMoney()+"";
        string detail = ToThaiJob(prCharacter.GetJob()) + "\r\n" +
            prCharacter.GetLevel() + "\r\n" +
            prCharacter.GetHP() +"/"+prCharacter.GetMaxHP() + "\r\n" +
            prCharacter.GetSP() + "/" + prCharacter.GetMaxSP() + "\r\n" +
            prCharacter.GetEXP() + "\r\n" +
            prCharacter.GetATK() + "\r\n" +
            prCharacter.GetDEF() + "\r\n";
        characterDetail.GetComponent<Text>().text = detail;       
        // Active preview character
        transform.GetChild(2).gameObject.SetActive(true);
        PlayerAnimation pa = transform.GetChild(2).GetComponent<PlayerAnimation>();
        pa.genderData = prCharacter.GetGender();
        pa.jobData = prCharacter.GetJob();
        pa.hairData = prCharacter.GetHeadEquipment();
        pa.clothData = prCharacter.GetBodyEquipment();
        pa.shoeData = pa.clothData;
        selectCharacter = prCharacter;
        //Detail
        uiChoose.transform.GetChild(1).gameObject.SetActive(true);
        //Confirm button
        uiChoose.transform.GetChild(2).gameObject.SetActive(true);
        //Create button
        uiChoose.transform.GetChild(3).gameObject.SetActive(false);
        //Delete button
        //uiChoose.transform.GetChild(4).gameObject.SetActive(true);

    }
    public void chooseCharacter() {        
        GameObject playerGameObject = playerSystem.transform.GetChild(0).gameObject;
        PlayerStatus ps = playerGameObject.GetComponent<PlayerStatus>();
        ps.gender = selectCharacter.GetGender();
        ps.job = selectCharacter.GetJob();
        ps.playerName = selectCharacter.GetCharacterName();
        ps.playerID = PlayerData.Instance.GetAccountId();
        ps.level = selectCharacter.GetLevel();
        ps.playerHP = selectCharacter.GetHP();
        ps.maxHP = selectCharacter.GetMaxHP();
        ps.playerSP = selectCharacter.GetSP();
        ps.maxSP = selectCharacter.GetMaxSP();
        ps.playerEXP = selectCharacter.GetEXP();
        ps.maxEXP = selectCharacter.getMaxEXP();
        ps.atk = selectCharacter.GetATK();
        ps.def = selectCharacter.GetDEF();
        // Equipment
        ps.head = selectCharacter.GetHeadEquipment();
        ps.body = selectCharacter.GetBodyEquipment();
        // Equipment
        ps.updateUI();        
        playerSystem.SetActive(true);
        // set Location
        ps.transform.position = new Vector2(selectCharacter.GetPositionX() , selectCharacter.GetPositionY());        
        ps.currentMap = selectCharacter.GetCurrentMap();
        // (when scene finished load screen manager will auto set simulated to true)
        //////////// Inventory Part //////////
        Inventory inventory = playerSystem.transform.GetChild(0).GetChild(0).GetComponent<Inventory>();
        inventory.initInventory(selectCharacter.GetInventoryList(), selectCharacter.GetMoney());
        //////////// Inventory Part //////////
        //////////// Checkin Part ////////////
        foreach(KeyValuePair<int,string> checkinData in selectCharacter.GetCheckins()) {
            PlaceData pd = PlaceManager.GetPlaceDataFromPlaceID(checkinData.Key);
            pd.SetTime(checkinData.Value);
            ps.AddPlaceToList(pd);
        }
        //////////// Checkin Part ////////////
        // Loading Screen      
        loading.onLoadingScreenShowedHandler += enterOnlineWorld;
        loading.ShowLoading();
    }

    public void enterOnlineWorld(){
        Debug.Log("Enter Online World");
        LoadingScreen.Instance.onLoadingScreenShowedHandler -= enterOnlineWorld;        
        string map = selectCharacter.GetCurrentMap();
        // Active PlayerGameObject
        playerSystem.transform.GetChild(0).gameObject.SetActive(true);
        DGTMainController.Instance.EnterOnlineWorld(selectCharacter.GetCharacterName());
    }
    
    public void enterWorld() {
        // Set target for main camera to follow    
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = playerSystem.transform.GetChild(0).transform.position;
        mainCamera.GetComponent<follow>().target = playerSystem.transform.GetChild(0);
        mainCamera.GetComponent<follow>().enabled = true;        
        // Set Gameplay UI to show up [child 2 of UI gameobject]
        loading.transform.parent.GetChild(1).gameObject.SetActive(true);
        //////////// Quest Part //////////////    
        // Quest Success
        QuestContainer qcont = playerSystem.transform.GetChild(0).GetComponent<QuestContainer>();
        int[] questSuccessId = selectCharacter.GetQuestSuccess();
        foreach (int questID in questSuccessId) {
            Quest quest = QuestManager.GetQuestByID(questID);
            qcont.AddSuccessQuest(quest);
        }
        // Quest Process
        Dictionary<int , int> questProcess = selectCharacter.GetQuestProcess();
        foreach (KeyValuePair<int , int> data in questProcess) {
            Quest quest = QuestManager.GetQuestByID(data.Key);
            qcont.AddProcessQuest(quest , data.Value);
        }
        //////////// Quest Part //////////////             
        // Enter world will auto fade loading screen off when finished load
        GameObject.Find("EnterGame").GetComponent<EnterGame>().EnterWorld(selectCharacter.GetCurrentMap());
    }

    private string ToThaiJob(string job) {
        string result = "";
        switch (job) {
            case "farmer": result = "ชาวนาไทย";break;
            case "fisher": result = "ชาวประมง";break;
            case "boxer": result = "นักมวยไทย"; break;
        }
        return result;
    }
}
