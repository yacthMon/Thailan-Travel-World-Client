using UnityEngine;

public class respawnSystem : MonoBehaviour { //Abandon
    public delegate void onPlayerRespawn();
    public onPlayerRespawn onPlayerRespawnHandler = null;
    public GameObject currentPlayer;
    public GameObject playerPrefab;

    public void respawn() {
        if (onPlayerRespawnHandler != null)
            onPlayerRespawnHandler();        
        GameObject player = Instantiate(playerPrefab, this.transform.position, Quaternion.identity);
        GameObject.Find("Main Camera").GetComponent<follow>().target = player.transform;
        PlayerStatus ps = player.GetComponent<PlayerStatus>();
        PlayerStatus ops = currentPlayer.GetComponent<PlayerStatus>();
        ps.hpSlider = ops.hpSlider;
        ps.spSlider = ops.spSlider;
        ps.expSlider = ops.expSlider;
        ps.hpText = ops.hpText;
        ps.spText = ops.spText;
        ps.expText = ops.expText;
        ps.levelText = ops.levelText;
        ps.uiRespawn = ops.uiRespawn;
        QuestContainer oqc = currentPlayer.GetComponent<QuestContainer>();        
        PlayerStatus.copyStatus(ps, ops);
        ps.recoverStatus();
        GameObject.Find("Camera_Avatar").GetComponent<follow>().target = player.transform.GetChild(0).transform;
        Destroy(currentPlayer);
        currentPlayer = player;
        player.GetComponent<QuestContainer>().copyQuestContainer(oqc); // copy quest container from old character
        
    }
}
