using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    [System.Serializable]
    public class ItemList {
        int itemId;
        int amount;

        public ItemList(int itemId , int amount) {
            this.itemId = itemId;
            this.amount = amount;
        }

        public int getItemId() {
            return this.itemId;
        }

        public int getItemAmount() {
            return this.amount;
        }
    }

    [System.Serializable]
    public class Character {
        
        private string characterName;
        // Status
        private string gender,job;
        private int level, exp, hp, sp, maxEXP, maxHp, maxSp, atk, def;
        // Equipment
        private string headEquipment,bodyEquipment,weaponEquipment;
        // Location
        private string currentMap;
        private float positionX, positionY;
        // Inventory
        private int money;
        private List<ItemList> inventoryList = new List<ItemList>();
        private List<int> questSuccess = new List<int>();        
        private Dictionary<int , int> questProcess = new Dictionary<int , int>();
        private Dictionary<int , string> checkinPlace = new Dictionary<int , string>();
        public Character(string name) {
            this.characterName = name;
        }
        
        public void SetStatus(string gender,string job, int level, int exp, int hp, int sp, int maxEXP, int maxHp, int maxSp, int atk, int def) {
            this.gender = gender;
            this.job = job;
            this.level = level;
            this.exp = exp;
            this.hp = hp;
            this.sp = sp;
            this.maxEXP = maxEXP;
            this.maxHp = maxHp;
            this.maxSp = maxSp;
            this.atk = atk;
            this.def = def;            
        }

#region Getter
        public int GetLevel() {
            return this.level;
        }

        public int GetEXP() {
            return this.exp;
        }

        public int GetHP() {
            return this.hp;
        }

        public int GetSP() {
            return this.sp;
        }

        public int getMaxEXP() {
            return this.maxEXP;
        }

        public int GetMaxHP() {
            return this.maxHp;
        }

        public int GetMaxSP() {
            return this.maxSp;
        }

        public int GetATK() {
            return this.atk;
        }

        public int GetDEF() {
            return this.def;
        }

        public string GetCharacterName() {
            return this.characterName;
        }

        public string GetJob() {
            return this.job;
        }

        public string GetGender() {
            return this.gender;
        }

        public int GetMoney() {
            return this.money;
        }

        public List<ItemList> GetInventoryList() {
            return this.inventoryList;
        }
        
        public string GetHeadEquipment() {
            return this.headEquipment;
        }

        public string GetBodyEquipment() {
            return this.bodyEquipment;
        }

        public string GetWeaponEquipment() {
            return this.weaponEquipment;
        }

        public float GetPositionX() {
            return this.positionX;
        }

        public float GetPositionY() {
            return this.positionY;
        }

        public string GetCurrentMap() {
            return this.currentMap;
        }
        
        public int[] GetQuestSuccess() {
            return this.questSuccess.ToArray();
        }

        public Dictionary<int,int> GetQuestProcess() {
            return this.questProcess;
        }

        public Dictionary<int,string> GetCheckins() {
            return this.checkinPlace;
        }

        #endregion
        public void SetEquipment(string head, string body, string weapon) {
            this.headEquipment = head;
            this.bodyEquipment = body;
            this.weaponEquipment = weapon;
        }

        public void SetLocation(string map, float x, float y) {
            this.currentMap = map;
            this.positionX = x;
            this.positionY = y;
        }

        public void SetCharacterName(string name) {
            this.characterName = name;
        }

        public void SetGender(string gender) {
            this.gender = gender;
        }
        
        public void SetJob(string job) {
            this.job = job;
        }

        public void SetMoney(int money) {
            this.money = money;
        }

        public void ListItemToInventoryList(int itemId, int amount) {
            this.inventoryList.Add(new ItemList(itemId, amount));
        }

        public void AddQuestSuccessList(int questID) {
            this.questSuccess.Add(questID);
        }

        public void AddQuestProcessList(int questID, int currentTotal) {
            this.questProcess.Add(questID , currentTotal);
        }

        public void AddCheckinList(int placeID, string time) {
            this.checkinPlace.Add(placeID , time);
        }
    }
    [SerializeField]
    private int accountId;
    public string fbName;
    public Sprite fbDisplay;
    [SerializeField]

    List<Character> characters = new List<Character>();
    public static PlayerData Instance;

    void Start() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void SetAccountId(int id) {
        this.accountId = id;
    }
    
    public int GetAccountId() {
        return this.accountId;
    }
    public void SetCharacters(Character[] chs) {
        this.characters.AddRange(chs);
    }
    
    public void AddCharacter(Character ch) {
        this.characters.Add(ch);
    }

    public Character[] getCharacter() {
        return this.characters.ToArray();
    }
}
