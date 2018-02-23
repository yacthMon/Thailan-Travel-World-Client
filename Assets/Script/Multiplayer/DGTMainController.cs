using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DGTMainController : MonoBehaviour
{
    [SerializeField]
    [Range(100,1000)]
    private int sendingTimer;
    public string[] ipaddress;
    public int[] port;
    [SerializeField]
    private bool inWorld;
    [SerializeField]
    public static DGTMainController Instance;
    [SerializeField]
    private bool isLocalhost;
	void OnApplicationQuit()
	{       
        if (this.inWorld) {
            Debug.Log("Exit world");
            ExitOnlineWorld();
        }
        Debug.Log("Disconnect Server");
		DGTRemote.GetInstance().Disconnect();                       //OnApplicationQuit
	}

    void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            if (this.inWorld) {
                // change status to pause and send to online
                //ExitOnlineWorld();
            }
        } else {
            if (!this.inWorld && SceneManager.GetActiveScene().name == "gameplay") {
                // change status to active and send to online
            }
        }
    }

    void Update ()
	{
        DGTRemote.GetInstance().ProcessEvents ();                           //Process Event
	}
    
	void Start ()
	{        
        if (DGTMainController.ReferenceEquals(Instance,null)) {            
            Instance = this;
            Instance.inWorld = false;                     
            StartCoroutine(ConnectToServer(isLocalhost?"192.168.1.48":ipaddress[0],port[0]));
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
        }
	}

    public void StartConnect() {
        StartCoroutine(ConnectToServer(isLocalhost ? "192.168.1.48" : ipaddress[0] , port[0]));
    }

	public IEnumerator ConnectToServer (string ipaddress, int port)
	{
		DGTPacket.Config pc = new DGTPacket.Config (ipaddress, port);       //connect to server IP,port
		DGTRemote.resetGameState ();
		DGTRemote gamestate = DGTRemote.GetInstance ();
		gamestate.Connect (pc.host, pc.port);                                   //IP host, port to serve
		gamestate.ProcessEvents ();                                             //Process Event
		yield return new WaitForSeconds (0.1f);
		for (int i = 0; i < 10; i++) {
			if (gamestate.Connected ()) {
				break;
			}
			if (gamestate.ConnectFailed ()) {
				break;
			}
			gamestate.ProcessEvents ();
			yield return new WaitForSeconds (i * 0.1f);
		}

		if (gamestate.Connected ()) {
            Text status = GameObject.Find("s1_status").GetComponent<Text>();
            status.text = "Online";
            status.color = Color.green;
			gamestate.mainController = this;

		} else {
			yield return new WaitForSeconds (5f);
			Debug.Log ("Cannot connect");
		}
		//StartCoroutine(PingTest());
		yield break;
	}

	public IEnumerator PingTest (){
		int i = 0;
		while (true)
        {
            DGTRemote.GetInstance ().TryPing();                //Lastversion
			i++;
			yield return new WaitForSeconds(3);
		}
	}

    #region Access Data
    public void DoNormalLogin() {
        Authentication.Instance.DoNormalLogin();
    }
    public void DoRegister() {
        Authentication.Instance.DoRegister();
    }


    #endregion

    #region firstScene
    public void EnterChooseCharacter() {
        StartCoroutine(GameObject.Find("Press").GetComponent<ClickGame>().LoadLevel("ChooseCharacter"));
    }
    
    #endregion

    #region Multiplayer Player
    public void EnterOnlineWorld(string characterName) {        
        this.inWorld = true;
        DGTRemote.Instance.RequestEnterWorld(characterName);
        StartSendMoving();
    }
    
    public void ExitOnlineWorld() {
        this.inWorld = false;
        CancelInvoke("SendMoving");
        DGTRemote.Instance.RequestExitWorld();
    }

    public void UpdatePlayerData() {
        DGTRemote.Instance.RequestUpdatePlayerData();
    }

    public void StartSendMoving() {
        Debug.Log("Start sending data repeat Rate : " + sendingTimer +"ms");
        InvokeRepeating("SendMoving", 0, sendingTimer/1000f); // turn million second in to second
    }

    public void NewConnectOnlinePlayer(int uid , string name , Vector2 position , string gender, string job, int HP , int SP , int level , string equipmentHead , string equipmentBody , string equipmentWeapon) {
        Debug.Log("New connection at : " + position);
        OnlinePlayerController.Instance.NewOnlinePlayer(uid, name, position, gender, job, HP,SP,level,equipmentHead,equipmentBody,equipmentWeapon);
    }

    public void RemoveDisconnectPlayer(int uid) {
        OnlinePlayerController.Instance.RemoveOnlinePlayer(uid);
    }

    public void UpdateOnlinePlayerData(int uid, Vector2 position, Vector2 velocity,float scalex, int animationId) {
        OnlinePlayerController.Instance.UpdateOnlinePlayer(uid,position,velocity,scalex, animationId);
    }

    public void UpdateOnlineMonsterData(int ID, int HP, Vector2 Position) {
        OnlineMonsterController.Instance.UpdateOnlineMonster(ID , Position);
    }

    public void UpdateOnlineMonsterHurt(int ID , int damage , int hpLeft , int knockback) {
        OnlineMonsterController.Instance.UpdateOnlineMonsterHurt(ID , damage , hpLeft , knockback);
    }

    public void SendMoving(){
		if(DGTRemote.Instance.Connected()){
            /*  DGTRemote.Instance.RequestSendChat(m_inputText.text);        //Lastversion
              DGTRemote.Instance.RequestSendFloat(1.445f);       */          //Lastversion
                                                                             //Player data (UID,Position (x,y) , Speed (x,y))
            if (PlayerStatus.Instance) {
                DGTRemote.Instance.RequestSendData(PlayerStatus.Instance.gameObject);
            }
		}
	}
    #endregion

    #region Online Monster
    public void RequestSendMonsterHurt(int monsterID, int knockback) {
        DGTRemote.GetInstance().RequestSendingMonsterHurt(monsterID, knockback);
    }    

    public void EliiminateMonster(int id, Item[] itemDrop) {
        OnlineMonsterController.Instance.EliminateMonster(id, itemDrop);
    }
    #endregion
}
