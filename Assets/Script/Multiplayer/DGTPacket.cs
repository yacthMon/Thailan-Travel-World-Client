using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

class DGTPacket : PacketManager
{
	public class Config
	{
		public string host;
		public int port;

		public Config (string h, int p)
		{
			host = h;
			port = p;
		}
	};
    public enum AnimationId {
        IDLE = 1,
        WALK = 2,
        HURT = 3,
        ATTACK = 4,
        JUMP = 5,
        FALL = 6,
        DIE = 7,
        DIE_LOOP = 8
    }
    private AnimatorStateInfo stateInfo;
    private enum PacketId
	{
        // 1xxxx For Client to Server
        CS_CONNECTION = 10001,
        CS_PING = 10002,
        /* 11xxx for Account Access */
        CS_REGISTER = 11010,
        CS_AUTHENTICATION  = 11011,
        CS_UPDATE_ACCOUNTDATA = 11012,
        CS_CHECK_CHARACTER_NAME = 11013,
        CS_CREATE_CHARACTER = 11014,
        CS_AUTHENTICATION_WITH_FACEBOOK = 11015,
        CS_REGISTER_FACEBOOK_DATA = 11016,
        /* 12xxx for Online Realtime*/
        CS_REQUEST_ENTER_WORLD = 12020,
        CS_SEND_PLAYER_MOVING = 12021,
        CS_EXIT_WORLD = 12022,
        CS_PLAYER_CHANGE_MAP = 12023,
        CS_SEND_PLAYER_STATUS = 12024,
        CS_INVENTORY_ADD = 12025,
        CS_INVENTORY_INCREASE = 12026,
        CS_INVENTORY_DECREASE = 12027,
        CS_INVENTORY_REMOVE = 12028,
        CS_INVENTORY_UPDATE_MONEY = 12029,
        // Monster Part
        CS_SEND_MONSTER_HURT = 12201,
        // Quest Part
        CS_SEND_QUEST_ACCEPT= 12300,
        CS_SEND_QUEST_UPDATE= 12301,
        CS_SEND_QUEST_SUCCESS= 12302,
        // Checkin Part
        CS_SEND_CHECKIN = 12400,
        // Item Part
        CS_REQUEST_GET_ITEM = 12410,
        CS_CHANGE_EQUIPMENT = 12450,
        CS_CHAT = 12101,
        CS_NOTIFICATION = 12102,

        // 2xxxx For Server to Client
        SC_ERROR =20000,
        SC_CONNECTION =20001,
        SC_PING_SUCCESS = 20002,
        /* 21xxx for Account Access */
        SC_REGISTER_SUCCESS = 21010,
        SC_REGISTER_FAILED = 21011,
        SC_AUTHENTICATION_GRANT = 21012,
        SC_AUTHENTICATION_DENIED = 21013,
        SC_ACCOUN_DATA = 21014,
        SC_CHARACTER_NAME_AVAILABLE= 21015,
        SC_CHARACTER_NAME_ALREADY_USED = 21016,
        SC_CHARACTER_CREATE_SUCCESS = 21017,
        SC_CHARACTER_CREATE_FAILED = 21018,
        SC_FACEBOOK_REQUEST_REGISTER= 21019,
        /* 22xxx for Online Realtime*/
        SC_ONLINE_REALTIME_CONTROL = 22000,
        SC_MULTIPLAYER_PLAYERS_IN_WORLD = 22020,
        SC_MULTIPLAYER_ENTER_WORLD_GRANT = 22021,
        SC_MULTIPLAYER_ENTER_WORLD_DENIED = 22022,
        SC_ONLINE_PLAYER_CONNECT =22023,
        SC_ONLINE_PLAYER_CONTROL = 22024,
        SC_ONLINE_PLAYER_DISCONNECT = 22025,
        // Monster part
        SC_ONLINE_MONSTER_IN_WORLD = 22200,
        SC_ONLINE_MONSTER_SPAWN = 22201,
        SC_ONLINE_MONSTER_CONTROL = 22202,
        SC_ONLINE_MONSTER_ELIMINATE = 22203,
        SC_ONLINE_MONSTER_REWARD = 22204,
        // Item part
        SC_ONLINE_ITEM_IN_WORLD = 22300,
        SC_GET_ITEM_GRANT= 22301,
        SC_ONLINE_ITEM_REMOVE= 22302,
        SC_CHAT = 22026,
        SC_NOTIFICATION = 22027

	}

	private DGTRemote _remote;

	public DGTPacket (DGTRemote remote) : base()
	{
		_remote = remote;

		PacketMapper ();
	}

	protected override void OnConnected ()
	{
		_remote.OnConnected ();
	}

	protected override void OnDisconnected ()
	{
		_remote.OnDisconnected ();
	}

	protected override void OnFailed ()
	{
		_remote.OnFailed ();
	}


#region PacketMapper
	private void PacketMapper ()
	{
        _Mapper[(int)PacketId.SC_ERROR] = RecvError ;
        _Mapper[(int)PacketId.SC_CONNECTION] = RecvConnection;
        _Mapper[(int)PacketId.SC_PING_SUCCESS] = RecvPing_Success;
        _Mapper[(int)PacketId.SC_REGISTER_SUCCESS] = RecvRegister_Success;
        _Mapper[(int)PacketId.SC_REGISTER_FAILED] = RecvRegister_Failed;
        _Mapper[(int)PacketId.SC_AUTHENTICATION_GRANT] = RecvAuthentication_Grant;
        _Mapper[(int)PacketId.SC_AUTHENTICATION_DENIED] = RecvAuthentication_Denied;
        _Mapper[(int)PacketId.SC_ACCOUN_DATA] = RecvAccountData;
        _Mapper[(int)PacketId.SC_CHARACTER_NAME_AVAILABLE] = RecvCharacterNameAvailable;
        _Mapper[(int)PacketId.SC_CHARACTER_NAME_ALREADY_USED] = RecvCharacterNameAlreadyUsed;
        _Mapper[(int)PacketId.SC_CHARACTER_CREATE_SUCCESS] = RecvCharacterCreateSuccess;
        _Mapper[(int)PacketId.SC_CHARACTER_CREATE_FAILED] = RecvCharacterCreateFailed;
        _Mapper[(int)PacketId.SC_ONLINE_PLAYER_CONNECT] = RecvMultiplayerConnect;
        _Mapper[(int)PacketId.SC_FACEBOOK_REQUEST_REGISTER] = RecvFacebookRequestRegister;
        // Multiplayer
        _Mapper[(int)PacketId.SC_ONLINE_REALTIME_CONTROL] = RecvOnlineRealtimeControl;
        _Mapper[(int)PacketId.SC_MULTIPLAYER_PLAYERS_IN_WORLD] = RecvMultiplayerPlayerInSameMap;
        _Mapper[(int)PacketId.SC_MULTIPLAYER_ENTER_WORLD_GRANT] = RecvMultiplayerEnterWorldGrant;
        _Mapper[(int)PacketId.SC_MULTIPLAYER_ENTER_WORLD_DENIED] = RecvMultiplayerEnterWorldDenied;
        _Mapper[(int)PacketId.SC_ONLINE_PLAYER_CONTROL] = RecvMultiplayerControl;
        _Mapper[(int)PacketId.SC_ONLINE_PLAYER_DISCONNECT] = RecvMultiplayerDisconnect;
        _Mapper[(int)PacketId.SC_ONLINE_MONSTER_IN_WORLD] = RecvOnlineMonsterInWorld;
        _Mapper[(int)PacketId.SC_ONLINE_MONSTER_SPAWN] = RecvOnlineMonsterSpawn;
        //_Mapper[(int)PacketId.SC_ONLINE_MONSTER_CONTROL] = RecvOnlineMonsterControl;
        _Mapper[(int)PacketId.SC_ONLINE_MONSTER_ELIMINATE] = RecvOnlineMonsterEliminate;        
        _Mapper[(int)PacketId.SC_ONLINE_MONSTER_REWARD] = RecvOnlineMonsterReward;
        _Mapper[(int)PacketId.SC_ONLINE_ITEM_IN_WORLD] = RecvOnlineItemInWorld;
        _Mapper[(int)PacketId.SC_GET_ITEM_GRANT] = RecvGetOnlineItemGrant;
        _Mapper[(int)PacketId.SC_ONLINE_ITEM_REMOVE] = RecvRemoveOnlineItem;
        _Mapper[(int)PacketId.SC_CHAT] = RecvChat;
        _Mapper[(int)PacketId.SC_NOTIFICATION] = RecvNotification;	  
	}
#endregion

#region send to server

    public void RequestConnection()
    {

    }

    public void RequestPing()
    {
        PacketWriter pw = BeginSend((int)PacketId.CS_PING);        
        EndSend();
    }

    public void RequestRegister(string username,string password,string email,string gender) {
        PacketWriter pw = BeginSend((int)PacketId.CS_REGISTER);
        pw.WriteString(username);
        pw.WriteString(password);
        pw.WriteString(email);
        pw.WriteString(gender);
        EndSend();
    }

    public void RequestAuthentication(string username, string password)
    {
		PacketWriter pw = BeginSend((int)PacketId.CS_AUTHENTICATION);
		pw.WriteString(username);
		pw.WriteString(password);
		EndSend();
    }

    public void RequestAuthenticationWithFacebook(string fbid , string token) {
        PacketWriter pw = BeginSend((int)PacketId.CS_AUTHENTICATION_WITH_FACEBOOK);
        pw.WriteString(fbid);
        pw.WriteString(token);
        EndSend();
    }
    public void RequestUpdatePlayerData()
    {
		PacketWriter pw = BeginSend((int)PacketId.CS_UPDATE_ACCOUNTDATA);

		EndSend();
    }
    
    public void RequestEnterWorld(string characterName) {
        PacketWriter pw = BeginSend((int)PacketId.CS_REQUEST_ENTER_WORLD);
        pw.WriteString(characterName);
        EndSend();
    }

    public void RequestSendPlayerStatus(PlayerStatus ps) {
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_PLAYER_STATUS);
        pw.WriteUInt8(ps.level);
        pw.WriteUInt16(ps.playerEXP);
        pw.WriteUInt16(ps.maxEXP);
        pw.WriteUInt16(ps.playerHP);
        pw.WriteUInt16(ps.maxHP);
        pw.WriteUInt16(ps.playerSP);
        pw.WriteUInt16(ps.maxSP);        
        pw.WriteUInt16(ps.atk);
        pw.WriteUInt16(ps.def);
        EndSend();
    }

    

    public void RequestExitWorld() {
        Debug.Log("Exit world");
        PacketWriter pw = BeginSend((int)PacketId.CS_EXIT_WORLD);
        EndSend();
    }

    public void RequestSendPlayerData(GameObject player){
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_PLAYER_MOVING);
        PlayerStatus ps = player.GetComponent<PlayerStatus>();        
        pw.WriteUInt32(ps.playerID);
        pw.WriteFloat(player.transform.position.x);
        pw.WriteFloat(player.transform.position.y);        
        pw.WriteFloat(player.GetComponent<Rigidbody2D>().velocity.x);
        pw.WriteFloat(player.GetComponent<Rigidbody2D>().velocity.y);
        pw.WriteFloat(player.transform.localScale.x);        
        pw.WriteInt8(TurnAnimationToId(ps.anim));
        EndSend();
    }

    public void RequestPlayerChangeMap(string mapName,Vector2 position) {
        PacketWriter pw = BeginSend((int)PacketId.CS_PLAYER_CHANGE_MAP);
        pw.WriteString(mapName);
        pw.WriteFloat(position.x);
        pw.WriteFloat(position.y);
        EndSend();
    }

    public void RequestCheckName(string name) {
        PacketWriter pw = BeginSend((int)PacketId.CS_CHECK_CHARACTER_NAME);
        pw.WriteString(name);
        EndSend();
    }

    public void RequestCreateCharacter(string name, string gender, string job,string hair) {
        PacketWriter pw = BeginSend((int)PacketId.CS_CREATE_CHARACTER);
        pw.WriteString(name);
        pw.WriteString(gender);
        pw.WriteString(job);
        pw.WriteString(hair);
        EndSend();
    }

    public void RequestRegisterFacebookData(string email, string gender) {
        PacketWriter pw = BeginSend((int)PacketId.CS_REGISTER_FACEBOOK_DATA);
        pw.WriteString(email);
        pw.WriteString(gender);
        EndSend();
    }

    public void RequestInventoryAdd(int itemId, int amount) {
        PacketWriter pw = BeginSend((int)PacketId.CS_INVENTORY_ADD);
        pw.WriteUInt32(itemId);
        pw.WriteUInt16(amount);
        EndSend();
    }

    public void RequestInventoryIncrease(int itemId , int amount) {
        PacketWriter pw = BeginSend((int)PacketId.CS_INVENTORY_INCREASE);
        pw.WriteUInt32(itemId);
        pw.WriteUInt16(amount);
        EndSend();
    }

    public void RequestInventoryDecrease(int itemId , int amount) {
        PacketWriter pw = BeginSend((int)PacketId.CS_INVENTORY_DECREASE);
        pw.WriteUInt32(itemId);
        pw.WriteUInt16(amount);
        EndSend();
    }

    public void RequestInventoryRemove(int itemId ) {
        PacketWriter pw = BeginSend((int)PacketId.CS_INVENTORY_REMOVE);
        pw.WriteUInt32(itemId);        
        EndSend();
    }

    public void RequestInventoryUpdateMoney(int money) {
        PacketWriter pw = BeginSend((int)PacketId.CS_INVENTORY_UPDATE_MONEY);
        pw.WriteUInt32(money);
        EndSend();
    }

    //Monster Part 
    // 12201
    public void RequestSendMonsterHurt(int monsterID, int knockback) {
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_MONSTER_HURT);
        pw.WriteInt32(monsterID);
        pw.WriteInt32(knockback);
        EndSend();
    }
    //Quest Part
    // 12300
    public void RequestSendAcceptQuest(int questID) {
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_QUEST_ACCEPT);
        pw.WriteUInt16(questID);
        EndSend();
    }
    // 12301
    public void RequestSendUpdateQuest(int questID , int currentTotal) {
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_QUEST_UPDATE);
        pw.WriteUInt16(questID);
        pw.WriteUInt16(currentTotal);
        EndSend();
    }
    // 12302
    public void RequestSendSuccesQuest(int questID) {
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_QUEST_SUCCESS);
        pw.WriteUInt16(questID);
        EndSend();
    }
    // 12400
    public void RequestSendCheckin(int placeID, string time) {
        Debug.Log("Send checkin");
        PacketWriter pw = BeginSend((int)PacketId.CS_SEND_CHECKIN);
        pw.WriteUInt8(placeID);
        pw.WriteString(time);
        EndSend();
    }
    // 12410
    public void RequestGetItem(int itemOnlineID, int itemID) {
        PacketWriter pw = BeginSend((int)PacketId.CS_REQUEST_GET_ITEM);
        pw.WriteUInt32(itemOnlineID);
        pw.WriteUInt32(itemID);
        EndSend();
    }
    // 12450
    public void RequestChangeEquipment(int part, string equipmentValue) {
        PacketWriter pw = BeginSend((int)PacketId.CS_CHANGE_EQUIPMENT);
        pw.WriteInt8(part);
        pw.WriteString(equipmentValue);
        EndSend();
    }

    public void RequestChat()
    {
        

    }

    public void Notification()
    {

    }
    
#endregion

#region receive from server
    // 20000
    private void RecvError(int packet_id,PacketReader pr)
    {
        DGTRemote.GetInstance().Logout();
        Popup.Instance.showPopup("Disconnected from server" , pr.ReadString());
        /*Popup.Instance.btnOK.onClick.AddListener(() => {
            DGTRemote.GetInstance().Logout();
        });*/
    }
    // 20001
    private void RecvConnection (int packet_id,PacketReader pr)
    {
        Debug.Log("RecvConnection");
    }
    // 20002
    private void RecvPing_Success(int packet_id,PacketReader pr)
    {
        int ping = pr.ReadUInt8();
        Debug.Log("Ping : " + ping + "ms");

    }
    // 21010
    private void RecvRegister_Success(int packet_id,PacketReader pr) {
        string username = pr.ReadString();
        DGTRemote.GetInstance().ReceiveRegisterSuccess(username);
    }
    // 21011
    private void RecvRegister_Failed(int packet_id,PacketReader pr) {
        int errCode = pr.ReadInt8();
        string reason = pr.ReadString();
        DGTRemote.GetInstance().ReceiveRegisterFailed(reason);
    }
    // 21012
    private void RecvAuthentication_Grant(int packet_id,PacketReader pr)
    {
        Debug.Log("RecvAuthentication_Grant");
        DGTRemote.GetInstance().ReceiveAuthenticationGrant();

        /*int uid = pr.ReadUInt32();
        int color = pr.ReadUInt8();
        int highestLevel = pr.ReadUInt16();
        int highestCheckpoint= pr.ReadUInt16();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Get Data From Server-  UID : " + uid + "  Color :" + color);*/
        /* for work when start gameplay
        DGTRemote.GetInstance().RequestEnterWorld(player.transform.position, dinoColor);
        DGTRemote.GetInstance().StartSendingData();*/

    }
    // 21013
    private void RecvAuthentication_Denied(int packet_id,PacketReader pr)
    {
        int errCode = pr.ReadInt8();
        string reason = pr.ReadString();
        DGTRemote.GetInstance().ReceiveAuthenticationDeinied(reason);
    }
    // 21014
    private void RecvAccountData(int packet_id,PacketReader pr) {           
        int accountId = pr.ReadInt32();
        Debug.Log("Recieve Account Data id : " + accountId);
        int characterAmount = pr.ReadInt8();
        PlayerData.Character[] characters = null;
        if (characterAmount > 0) {
            characters = new PlayerData.Character[characterAmount];
            for (int i = 0; i < characterAmount; i++) { // for each character                
                string name = pr.ReadString();
                characters[i] = new PlayerData.Character(name);
                // Status
                string gender = pr.ReadString();
                string job = pr.ReadString();
                int level = pr.ReadInt8();
                int exp = pr.ReadInt32();
                int hp = pr.ReadInt32();
                int sp = pr.ReadInt32();
                int maxEXP = pr.ReadInt32();
                int maxHp = pr.ReadInt32();
                int maxSp = pr.ReadInt32();
                int atk = pr.ReadInt32();
                int def = pr.ReadInt32();
                characters[i].SetStatus(gender,job, level, exp, hp, sp, maxEXP,maxHp, maxSp, atk, def);
                // Equipment
                string headEquipment = pr.ReadString();
                string bodyEquipment = pr.ReadString();
                string weaponEquipment = pr.ReadString();
                characters[i].SetEquipment(headEquipment, bodyEquipment, weaponEquipment);
                // Location
                string currentMap = pr.ReadString();
                float positionX = pr.ReadFloat();
                float positionY = pr.ReadFloat();                
                characters[i].SetLocation(currentMap, positionX, positionY);                
                // Inventory
                int money = pr.ReadInt32();
                characters[i].SetMoney(money);
                int itemAmount = pr.ReadInt8();
                /*Debug.Log("Character name : " + name);
                Debug.Log("Job : " + job + ", Lvl." + level + " EXP." + exp);
                Debug.Log("HP : " + hp + "/" + maxHp + ",  SP : " + sp +"/"+maxSp );
                Debug.Log("Attack : " + atk + " , Defense : " + def);
                Debug.Log("[Equipment] Head : " + headEquipment + ", Body : "+bodyEquipment+", Weapon : "+weaponEquipment);
                Debug.Log("[Location] Map : " + currentMap + " X : " + positionX + " Y : " + positionY);
                Debug.Log("Inventory");
                Debug.Log("Gold : " + gold);
                Debug.Log("Item [" + itemAmount + "]");*/
                /* Receive Item*/
                if (itemAmount > 0) {
                    for(int j = 0; j < itemAmount; j++) {
                        int itemId = pr.ReadInt32();
                        int amount = pr.ReadInt16();
                        //Debug.Log("Item id : " + itemId + ", " + amount + "ea"); 
                        characters[i].ListItemToInventoryList(itemId , amount);                        
                    }                    
                }else {
                    //Debug.Log("No item in inventory");
                }
                // QUEST
                int questSuccessAmount = pr.ReadUInt16();
                Debug.Log("Quest Success : " + questSuccessAmount);
                if (questSuccessAmount > 0) {
                    for (int j = 0; j < questSuccessAmount; j++) {                        
                        characters[i].AddQuestSuccessList(pr.ReadUInt16());
                    }
                }
                int questProcessAmount = pr.ReadUInt16();                
                if (questProcessAmount > 0) {
                    for (int j = 0; j < questProcessAmount; j++) {
                        characters[i].AddQuestProcessList( pr.ReadUInt16() , pr.ReadUInt16());
                    }
                }
                // Checkin
                int checkinAmount = pr.ReadUInt8();
                if(checkinAmount > 0) {
                    for(int j=0; j < checkinAmount; j++) {
                        characters[i].AddCheckinList(pr.ReadUInt8(),pr.ReadString());
                    }
                }
            }
            // Done character
            DGTRemote.GetInstance().ReceiveAccountData(accountId, characters);
        }else {
            DGTRemote.GetInstance().ReceiveAccountData(accountId, null);            
        }
    }
    // 21015
    private void RecvCharacterNameAvailable(int packet_id,PacketReader pr) {        
        DGTRemote.GetInstance().ReceiveCharacterNameAvailable();        
    }
    // 21016
    private void RecvCharacterNameAlreadyUsed(int packet_id, PacketReader pr) {        
        DGTRemote.GetInstance().ReceiveCharacterNameAlreadyUsed();
    }
    // 21017
    private void RecvCharacterCreateSuccess(int packet_id, PacketReader pr) {
        string name = pr.ReadString();
        PlayerData.Character character = new PlayerData.Character(name);
        // Status
        string gender = pr.ReadString();
        string job = pr.ReadString();
        int level = pr.ReadInt8();
        int exp = pr.ReadInt32();
        int hp = pr.ReadInt32();
        int sp = pr.ReadInt32();
        int maxEXP = pr.ReadInt32();
        int maxHp = pr.ReadInt32();
        int maxSp = pr.ReadInt32();
        int atk = pr.ReadInt32();
        int def = pr.ReadInt32();
        character.SetStatus(gender, job, level, exp, hp, sp, maxEXP, maxHp, maxSp, atk, def);
        // Equipment
        string headEquipment = pr.ReadString();
        string bodyEquipment = pr.ReadString();
        string weaponEquipment = pr.ReadString();
        character.SetEquipment(headEquipment, bodyEquipment, weaponEquipment);
        // Location
        string currentMap = pr.ReadString();        
        float positionX = pr.ReadFloat();
        float positionY = pr.ReadFloat();        
        character.SetLocation(currentMap, positionX, positionY);
        // Inventory
        int money = pr.ReadInt32();
        character.SetMoney(money);        
        /*Debug.Log("Character name : " + name);
        Debug.Log("Job : " + job + ", Lvl." + level + " EXP." + exp);
        Debug.Log("HP : " + hp + "/" + maxHp + ",  SP : " + sp +"/"+maxSp );
        Debug.Log("Attack : " + atk + " , Defense : " + def);
        Debug.Log("[Equipment] Head : " + headEquipment + ", Body : "+bodyEquipment+", Weapon : "+weaponEquipment);
        Debug.Log("[Location] Map : " + currentMap + " X : " + positionX + " Y : " + positionY);
        Debug.Log("Inventory");
        Debug.Log("Gold : " + gold);
        Debug.Log("Item [" + itemAmount + "]");*/
        DGTRemote.GetInstance().ReceiveCharacterCreated(character);
    }
    // 21018
    private void RecvCharacterCreateFailed(int packet_id, PacketReader pr) {
        DGTRemote.GetInstance().ReceiveCharacterCreateFailed();
    }
    // 21019
    private void RecvFacebookRequestRegister(int packet_id , PacketReader pr) {
        DGTRemote.GetInstance().ReceiveFacebookRequestLogin();
    }    
    // 22021
    private void RecvMultiplayerEnterWorldGrant(int packet_id , PacketReader pr) {
        Debug.Log("Access World Grant !");
        DGTRemote.GetInstance().ReceiveMultiplayerEnterWorldGrant();
    }
    // 22022
    private void RecvMultiplayerEnterWorldDenied(int packet_id , PacketReader pr) {
        Debug.Log("Access World Denied :(");
        DGTRemote.GetInstance().ReceiveMultiplayerEnterWorldDenied();
    }
    
    private void RecvMultiplayerPlayerInSameMap(int packet_id,PacketReader pr) {
        int dataAmount = pr.ReadInt16();
        for(int i = 0; i < dataAmount; i++) {
            RecvMultiplayerConnect(packet_id, pr);
        }
    }
    // 22023
    private void RecvMultiplayerConnect(int packet_id, PacketReader pr){
        int uid = pr.ReadUInt32();
        string name = pr.ReadString();
        Vector2 position = new Vector2(pr.ReadFloat(), pr.ReadFloat());
        string gender = pr.ReadString();
        string job = pr.ReadString();
        int HP = pr.ReadUInt32();
        int SP = pr.ReadUInt32();        
        int level = pr.ReadUInt32();
        string equipmentHead = pr.ReadString();
        string equipmentBody = pr.ReadString();
        string equipmentWeapon = pr.ReadString();
        DGTRemote.GetInstance().ReceiveOnlinePlayerConnect(uid,name,position, gender,job , HP ,SP,level,equipmentHead,equipmentBody,equipmentWeapon);
    }
    // 22024
    private void RecvMultiplayerControl(int packet_id,PacketReader pr)
    {
        Debug.Log("Receive Online Player Control");
        int dataNumber = pr.ReadInt16();
        for(int i = 0; i < dataNumber; i++) {
            DGTRemote.GetInstance().ReceiveOnlinePlayerData(pr.ReadUInt32(),
                new Vector2(pr.ReadFloat(), pr.ReadFloat()),//read Position x,y
                new Vector2(pr.ReadFloat(), pr.ReadFloat()),//read Velocity x,y
                pr.ReadFloat(), pr.ReadUInt8());//read scaleX
        }   
    }
    // 22025
    private void RecvMultiplayerDisconnect(int packet_id, PacketReader pr) {
        DGTRemote.GetInstance().ReceiveOnlinePlayerDisconnect(pr.ReadUInt32());
    }
    // --------- Monster
    // 22200
    private void RecvOnlineMonsterInWorld(int packet_id,PacketReader pr) {
        int dataAmount = pr.ReadInt8();
        for (int i = 0; i < dataAmount; i++) {
            RecvOnlineMonsterSpawn(packet_id , pr);
        }
    }
    // 22201
    private void RecvOnlineMonsterSpawn(int packet_id , PacketReader pr) {
        int ID = pr.ReadUInt32();
        int monsterID = pr.ReadUInt32();
        string monsterName = pr.ReadString();
        int level = pr.ReadUInt8();
        int HP = pr.ReadUInt32();
        int maxHP = pr.ReadUInt32();
        int ATK = pr.ReadUInt16();
        int DEF = pr.ReadUInt16();
        int movementSpeed = pr.ReadUInt8();        
        //Debug.Log("[" + monsterID + "] " + monsterName + " lv." + level + " HP:" + HP + " Speed:" + movementSpeed );
        Vector2 position = new Vector2(pr.ReadFloat() , pr.ReadFloat());
        Vector2 targetPosition = new Vector2(pr.ReadFloat() , pr.ReadFloat());        
        OnlineMonsterController.Instance.AddNewMonster(ID , monsterID ,monsterName,level,HP, maxHP , ATK,DEF,movementSpeed, position, targetPosition);
    }
    // 22202
    private void RecvOnlineMonsterControl(int packet_id , PacketReader pr) {
        Debug.Log("Receive Online Monster Control");
        int dataAmount = pr.ReadInt8();
        for (int i = 0; i < dataAmount; i++) {
            int ID = pr.ReadUInt32();
            int HP = pr.ReadUInt32();
            Vector2 position = new Vector2(pr.ReadFloat() , pr.ReadFloat());
            DGTRemote.GetInstance().ReceiveOnlineMonsterControl(ID,HP,position);
        }
    }    
    // 22203
    private void RecvOnlineMonsterEliminate(int packet_id , PacketReader pr) {
        int id = pr.ReadUInt32();
        int dataLength = pr.ReadUInt8();
        List<Item> itemDrop = new List<Item>();
        for (int i = 0; i < dataLength; i++) {
            Item item = ItemManager.GetItemComponent(pr.ReadUInt32());
            item.SetOnlineID(pr.ReadUInt32());
            itemDrop.Add(item);
        }
        DGTMainController.Instance.EliiminateMonster(id,itemDrop.ToArray());
    }
    // 22204
    private void RecvOnlineMonsterReward(int packet_id,PacketReader pr) {
        int monsterID = pr.ReadUInt32();
        int expReceive = pr.ReadUInt32();
        bool isKiller = pr.ReadInt8() == 0 ? false : true;
        //Debug.Log("Receive reward from killing monster [" + monsterID + "] EXP : " + expReceive +" killer : " + isKiller);
        DGTRemote.GetInstance().ReceiveOnlineMonsterReward(expReceive , isKiller, monsterID );
    }
    // 22000
    private void RecvOnlineRealtimeControl(int packet_id, PacketReader pr) {
        // Player
        int dataAmount = pr.ReadInt16();
        for (int i = 0; i < dataAmount; i++) {
            DGTRemote.GetInstance().ReceiveOnlinePlayerData(pr.ReadUInt32() ,
                new Vector2(pr.ReadFloat() , pr.ReadFloat()) ,//read Position x,y
                new Vector2(pr.ReadFloat() , pr.ReadFloat()) ,//read Velocity x,y
                pr.ReadFloat() , pr.ReadUInt8());//read scaleX
        }
        // Monster
        dataAmount = pr.ReadInt8();
        for (int i = 0; i < dataAmount; i++) {
            int ID = pr.ReadUInt32();
            int HP = pr.ReadUInt32();
            Vector2 position = new Vector2(pr.ReadFloat() , pr.ReadFloat());
            DGTRemote.GetInstance().ReceiveOnlineMonsterControl(ID , HP , position);
        }
        // Monster Hurt
        dataAmount = pr.ReadInt8();
        for (int i = 0; i < dataAmount; i++) {
            int ID = pr.ReadUInt32();
            int damage = pr.ReadUInt32();
            int hpLeft = pr.ReadUInt32();
            int knockbackDirection = pr.ReadInt8();
            DGTRemote.GetInstance().ReceiveOnlineMonsterHurt(ID , damage , hpLeft , knockbackDirection);
        }
        // Change Equipment
        dataAmount = pr.ReadInt8();
        for(int i=0; i< dataAmount; i++) {
            int UID = pr.ReadUInt32();
            int part = pr.ReadInt8();
            string value = pr.ReadString();
            DGTRemote.GetInstance().ReceiveOnlinePlayerChangeEquipment(UID , part , value);
        }
    }

    // 22300
    private void RecvOnlineItemInWorld(int packet_id, PacketReader pr) {
        int dataAmount = pr.ReadUInt8();
        for(int i =0;i<dataAmount; i++) {
            int onlineID = pr.ReadUInt32();
            int itemID = pr.ReadUInt32();
            Vector2 position = new Vector2(pr.ReadFloat() , pr.ReadFloat());
            OnlineItemControl.Instance.AddItemToWorld(onlineID,itemID,position);
        }
    }
    // 22301
    private void RecvGetOnlineItemGrant(int packet_id , PacketReader pr) {
        int itemID = pr.ReadUInt32();
        Inventory.Instance.GetOnlineItemToInventory(itemID);
    }
    // 22302
    private void RecvRemoveOnlineItem(int packet_id, PacketReader pr) {
        int onlineItemID = pr.ReadUInt32();        
        OnlineItemControl.Instance.RemoveItem(onlineItemID);
    }


    private void RecvChat(int packet_id,PacketReader pr)
    {
        Debug.Log("Chat");
    }

    private void RecvNotification(int packet_id,PacketReader pr)
    {
        Debug.Log("Notification");
    }
    #endregion
    
    private int TurnAnimationToId(Animator anim) {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("idle")) {
            return (int)AnimationId.IDLE;
        } else if (stateInfo.IsName("walk")) {
            return (int)AnimationId.WALK;
        } else if (stateInfo.IsName("jump")) {
            return (int)AnimationId.JUMP;
        } else if (stateInfo.IsName("fall")) {
            return (int)AnimationId.FALL;
        } else if (stateInfo.IsName("attack_farmer")|| stateInfo.IsName("attack_boxer") || stateInfo.IsName("attack_fisher")) {
            return (int)AnimationId.ATTACK;
        } else if (stateInfo.IsName("hurt")) {
            return (int)AnimationId.HURT;
        } else if (stateInfo.IsName("die")) {
            return (int)AnimationId.DIE;
        } else if (stateInfo.IsName("dieLoop")) {
            return (int)AnimationId.DIE_LOOP;
        }        
        return 1;// default animation 
    }
}
