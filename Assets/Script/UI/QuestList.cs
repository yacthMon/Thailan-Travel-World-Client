using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour {

    public class QuestDataDetail:MonoBehaviour {
        public int questId;
        public string questTitle;
        public string questDetail;
        public string questPlaceDetail;
        public string condition;
        public string reward;
        public int questNPC;
        public bool done;

        public void GetDataDetail(Quest q) {
            this.questId = q.questID;
            this.questTitle = q.questTitle;
            this.questDetail = q.questDetail;
            this.questPlaceDetail = q.questPlaceDetail;
            this.condition = q.qcon.ToDetailString();
            this.reward = q.GetRewardString();
            this.questNPC = q.npcID;
            this.done = q.isQuestSuccess;
        }
    }
    [SerializeField]
    private GameObject questListPrefabs;
    [SerializeField]
    private RectTransform listContent;
    [SerializeField]
    private GameObject bgDetailTitle;
    [SerializeField]
    private Button btnAbandonQuest;
    public Text questDetailTitle;
    public Text questDetailNPC;
    public Text questDetail;
    public Text questDetail2;
    public GameObject completeSign;
    private int state; //0 : In Progress , 1 : Done (State for this UI)
	void Start () {
		
	}

    public void ShowQuestDetail(GameObject q) {
        QuestDataDetail qdd = q.GetComponent<QuestDataDetail>();
        questDetailTitle.text = qdd.questTitle;
        questDetailNPC.text = NPCManager.GetNPCNameByID(qdd.questNPC)+"";
        questDetail.text = qdd.questDetail;
        if (!qdd.done) {
            questDetail2.text = "เงื่อนไข : " + qdd.condition + "\r\n"
                + "แผนที่ : " + qdd.questPlaceDetail + "\r\n"
                + "รางวัล : " + qdd.reward;
        }
        bgDetailTitle.SetActive(true);
        completeSign.SetActive(qdd.done);
        if (state == 0) { 
            btnAbandonQuest.gameObject.SetActive(true);
            btnAbandonQuest.onClick.RemoveAllListeners();
            btnAbandonQuest.onClick.AddListener(() => {            
                if (QuestContainer.Instance.abandonQuest(qdd.questId)) {
                    Popup.Instance.showPopup("ยกเลิกเควส" , "การยกเลิกเควสเสร็จสิ้น");
                    GameObject.Find("NPC_System").BroadcastMessage("refreshUI"); // broadcast message to all child of NPC_System
                } else {
                    Popup.Instance.showPopup("ยกเลิกเควส" , "การยกเลิกเควสล้มเหลว");
                }
                GetProcessList();
            });
        }
    }

    public void GetProcessList() {
        state = 0;
        ClearQuestList();
        QuestContainer qcon = QuestContainer.Instance;
        foreach (Quest q in qcon.processList) {
            AddQuestList(q,false);
        }
    }

    public void GetDoneList() {
        state = 1;
        ClearQuestList();
        QuestContainer qcon = QuestContainer.Instance;
        foreach (Quest q in qcon.successList) {
            AddQuestList(q,true);
        }
    }

    private void AddQuestList(Quest q, bool forceDone) {
        GameObject questList = Instantiate(questListPrefabs);
        QuestDataDetail qdd = questList.AddComponent<QuestDataDetail>();
        qdd.GetDataDetail(q);
        if (forceDone) {
            qdd.done = true;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { ShowQuestDetail(questList); });
        questList.GetComponent<EventTrigger>().triggers.Add(entry);
        questList.transform.GetChild(0).gameObject.SetActive(qdd.done); // set complete mark
        questList.transform.GetChild(1).GetComponent<Text>().text = q.questTitle; // set title 
        questList.transform.SetParent(listContent , false);
    }

    public void ClearQuestList() {
        questDetailTitle.text = "";
        questDetailNPC.text = "";
        questDetail.text = "";
        questDetail2.text = "";
        bgDetailTitle.SetActive(false);
        btnAbandonQuest.gameObject.SetActive(false);
        completeSign.SetActive(false);
        foreach (Transform questList in listContent.transform) {
            Destroy(questList.gameObject);
        }
    }
}
