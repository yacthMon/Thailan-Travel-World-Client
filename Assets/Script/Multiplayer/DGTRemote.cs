using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DGTRemote : MonoBehaviour
{
	public DGTMainController mainController;
    
	private enum State
	{
		DISCONNECTED = 0,
		DISCONNECTING,
		CONNECTED,
		CONNECTING,
	};

	private State _State;
	private DGTPacket _Packet;

	////////////////////////////////////////////////////////////////////////////////
	// Singleton Design Pattern.
	////////////////////////////////////////////////////////////////////////////////

	private static GameObject gameObjectState;
	private static DGTRemote g_instance;

	public static DGTRemote GetInstance(){
		if(g_instance == null){                                 //Call instace
            gameObjectState = new GameObject("DGTRemote");
            g_instance = gameObjectState.AddComponent<DGTRemote>();
            DontDestroyOnLoad(gameObjectState);
		}
        return g_instance;
	}
	public static void resetGameState(){
		Destroy(gameObjectState);
		g_instance = null;
	}
	public static DGTRemote Instance { get { return GetInstance(); } }

	////////////////////////////////////////////////////////////////////////////////
	public void Connect(string host, int port){
        if (_State != State.DISCONNECTED) return;

		_State = State.CONNECTING;
		_Packet.Connect(host, port);
	}

	public void Disconnect(){        
		if (_State != State.CONNECTED) return;
		_State = State.DISCONNECTED;
        _Packet.Disconnect();
	}

	public void OnConnected(){
		UnityEngine.Debug.Log (" Connected : _State "+ _State);
		_State = State.CONNECTED;
	}

	public void OnDisconnected(){
		if(_State != State.DISCONNECTED)
		{
            Popup.Instance.showPopup("Networking" , "หลุดการเชื่อมต่อกับเซิฟเวอร์");
            Popup.Instance.btnOK.onClick.AddListener(()=> { Application.Quit(); });
        }
		_State = State.DISCONNECTED;

	}

	public void OnFailed(){
		if(_State != State.DISCONNECTED)
		{
            Popup.Instance.showPopup("Networking" , "ไม่สามารถเชื่อมต่อกับเซิฟเวอร์ได้");
        }
		_State = State.DISCONNECTED;
	}

	public bool Connected(){
		return _Packet.Connected && _State == State.CONNECTED;
	}

	public bool ConnectFailed(){
        return _Packet.Failed;
	}

	public void ProcessEvents(){
		_Packet.ProcessEvents();
	}

	void Awake(){
		_Packet = new DGTPacket(this);
		_State = State.DISCONNECTED;
		//test();
	}
	////////////////////////////////////////////////////////////////////////////////
	public void RequestAuthentication(string username, string password){
        //_Packet.RequestAuthentication("duid@"+deviceUID,pName);
        _Packet.RequestAuthentication(username, password);
	}

    public void RequestRegister(string username,string password,string email,string gender) {
        _Packet.RequestRegister(username, password, email, gender);
    }
    public void RequestRegisterFacebookData(string email, string gender) {
        _Packet.RequestRegisterFacebookData(email,gender);
    }
    public void RequestAuthenticationFacebook(string fbid, string token) {
        _Packet.RequestAuthenticationWithFacebook(fbid , token);
    }
    
	public void RequestUpdatePlayerData() {
		_Packet.RequestUpdatePlayerData();
	}

    public void RequestSendData(GameObject player) {
        _Packet.RequestSendPlayerData(player);
    }

    public void RequestSendPlayerStatusData(PlayerStatus ps) {
        _Packet.RequestSendPlayerStatus(ps);
    }

    public void RequestEnterWorld(string characterName) {
        _Packet.RequestEnterWorld(characterName); //
    }

    public void RequestPlayerChangeMap(string mapName,Vector2 position) {
        _Packet.RequestPlayerChangeMap(mapName, position);
    }

    #region Inventory Request
    public void RequestInventoryAddItem(int itemId, int amount) {
        _Packet.RequestInventoryAdd(itemId , amount);
    }

    public void RequestInventoryIncreaseItem(int itemId , int amount) {
        _Packet.RequestInventoryIncrease(itemId,amount);
    }

    public void RequestInventoryDecreaseItem(int itemId , int amount) {
        _Packet.RequestInventoryDecrease(itemId , amount);
    }

    public void RequestInventoryRemoveItem(int itemId) {
        _Packet.RequestInventoryRemove(itemId);
    }

    public void RequestUpdateMoney(int money) {
        _Packet.RequestInventoryUpdateMoney(money);
    }

    #endregion

    #region Monster Request
    public void RequestSendingMonsterHurt(int monsterID, int knockback) {
        _Packet.RequestSendMonsterHurt(monsterID, knockback);
    }
    #endregion

    #region Quest
    public void RequestSendAcceptQuest(int questID) {
        _Packet.RequestSendAcceptQuest(questID);
    }

    public void RequestSendUpdateQuest(int questID, int currentTotal) {
        _Packet.RequestSendUpdateQuest(questID , currentTotal);
    }

    public void RequestSendSuccessQuest(int questID) {
        _Packet.RequestSendSuccesQuest(questID);
    }
    #endregion

    #region Checkin
    public void RequestSendCheckin(int placeID, string time) {
        _Packet.RequestSendCheckin(placeID , time);
    }
    #endregion

    public void RequestGetOnlineItem(int itemOnlineID, int itemID) {
        _Packet.RequestGetItem(itemOnlineID, itemID);
    }

    public void RequestChangeEquipment(int part , string equipmentValue) {
        _Packet.RequestChangeEquipment(part , equipmentValue);
    }

    public void RequestExitWorld() {
        _Packet.RequestExitWorld();
    }

    public void checkCharacterName(string name) {
        _Packet.RequestCheckName(name);
    }

    public void TryPing() {
        _Packet.RequestPing();

    }

    //receive access grant
    public void ReceiveAuthenticationGrant() {
        //mainController.EnterChooseCharacter();
        // Yeah login success, next wait for account data
    }
    
    public void ReceiveAuthenticationDeinied(string reason) {
        Popup.Instance.showPopup("การเข้าสู่ระบบล้มเหลว",reason);
        GameObject.Find("Login button").GetComponent<Button>().interactable = true;
    }

    public void ReceiveAccountData(int pid, PlayerData.Character[] characters) {
        PlayerData pd = mainController.gameObject.AddComponent<PlayerData>();
        pd.SetAccountId(pid);
        if(characters != null)
            pd.SetCharacters(characters);
        mainController.EnterChooseCharacter();
    }

    public void ReceiveRegisterSuccess(string username) {
        Popup.Instance.showPopup("การสมัครบัญชี" , "บัญชีของคุณถูกสร้างขึ้นเรียบร้อย");
        GameObject.Find("inputUsername").GetComponent<InputField>().text = username;
        GameObject.Find("Authentication").GetComponent<Animator>().Play("showLogin");
        GameObject.Find("btnRegister").GetComponent<Button>().interactable = true;
    }

    public void ReceiveRegisterFailed(string reason) {
        Popup.Instance.showPopup("การสมัครบัญชีล้มเหลว",reason);
        GameObject.Find("btnRegister").GetComponent<Button>().interactable = true;
    }
    // 21015
    public void ReceiveCharacterNameAvailable() {
        CreateCharacter cc = GameObject.Find("Create Character").GetComponent<CreateCharacter>();
        _Packet.RequestCreateCharacter(cc.playerName.text, cc.gender, cc.job, cc.hair);

    }
    // 21016
    public void ReceiveCharacterNameAlreadyUsed() {
        Popup.Instance.showPopup("การสร้างตัวละคร" , "มีตัวละครชื่อนี้อยู่แล้ว กรุณาเปลี่ยนชื่อใหม่");
        GameObject.Find("Create Character").GetComponent<CreateCharacter>().btnCreate.interactable = true;
    }
    // 21017
    public void ReceiveCharacterCreated(PlayerData.Character character) {
        PlayerData pd = mainController.gameObject.GetComponent<PlayerData>();
        pd.AddCharacter(character);
        //notification > Create success
        Popup.Instance.showPopup("การสร้างตัวละคร" , "สร้างตัวละครสำเร็จ !");
        // Move camera to Choose Character
        GameObject sam = GameObject.Find("Screen Action Manager");
        sam.GetComponent<Animator>().Play("goChooseCharacter");
        // Set preview Character
        sam.transform.GetChild(0).GetComponent<ChooseCharacter>().previewCharacter(character);

    }
    // 21018
    public void ReceiveCharacterCreateFailed() {
        GameObject.Find("Create Character").GetComponent<CreateCharacter>().btnCreate.interactable = true;
        Popup.Instance.showPopup("การสร้างตัวละคร" , "การสร้างตัวละครล้มเหลว");
        UnityEngine.Debug.Log("Character create failed");
    }
    // 21019
    public void ReceiveFacebookRequestLogin() {
        Authentication.Instance.RegisterFacebookData();
    }

    // 22021
    public void ReceiveMultiplayerEnterWorldGrant() {
        GameObject.Find("Choose Character").GetComponent<ChooseCharacter>().enterWorld();
    }
    // 22022
    public void ReceiveMultiplayerEnterWorldDenied() {
        Popup.Instance.showPopup("การเข้าสู่เกม" , "ไม่สามารถเข้าสู่ Online World ได้");
    }
    // 22200

    // 22202
    public void ReceiveOnlineMonsterControl(int ID, int HP, Vector2 targetPosition) {        
        mainController.UpdateOnlineMonsterData(ID , HP , targetPosition);
    }

    public void ReceiveOnlineMonsterHurt(int ID, int damage, int hpLeft, int knockback) {
        mainController.UpdateOnlineMonsterHurt(ID , damage , hpLeft , knockback);
    }

    public void ReceiveOnlineMonsterReward(int exp, bool isKiller, int monsterID) {
        PlayerStatus.Instance.getEXP(exp);
        if (isKiller) {
            PlayerStatus.Instance.killedMonster(monsterID);
        }
    }

    public void StartSendingData(){
        mainController.StartSendMoving();
	}

    public void RecvNotification(string msg){

    }

    public void ReceiveOnlinePlayerConnect(int uid, string name, Vector2 position,string gender, string job , int HP,int SP ,int level, string equipmentHead, string equipmentBody, string equipmentWeapon) {
        mainController.NewConnectOnlinePlayer(uid , name , position ,gender, job , HP , SP , level , equipmentHead , equipmentBody , equipmentWeapon);
    }

    public void ReceiveOnlinePlayerData(int uid, Vector2 position, Vector2 velocity, float scalex,int animationId) {        
        mainController.UpdateOnlinePlayerData(uid,position,velocity,scalex, animationId);
    }

    public void ReceiveOnlinePlayerDisconnect(int uid) {
        mainController.RemoveDisconnectPlayer(uid);
    }

    public void ReceiveOnlinePlayerChangeEquipment(int UID ,int part ,string value) {
        OnlinePlayerController.Instance.UpdateOnlinePlayerChangeEquipment(UID , part , value);
    }

    public void Logout() {
        Disconnect();
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>()) {
            if (go.name != "RemoteProxy")
                Destroy(go);
        }
        SceneManager.LoadScene("FirstScene" , LoadSceneMode.Single);
        mainController.StartConnect();
    }

}
