using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {
	
	public static GameScript GS;
	
	public GameObject playerPrefab;
	public MainMenuGui MMG;
	
	
	public MainMenuGui.PlayerInfo player;
	
	// Use this for initialization
	void Awake () {
		GS = this;
		MMG = MainMenuGui.MMG;
		playerPrefab = (GameObject) Resources.Load("Character");
		
		
	}
	
	void Start(){
		
		int loc = 1;
		if(Network.peerType == NetworkPeerType.Server) loc = 2;
		
		CreatePlayer(Network.player, true, loc);
		NetworkPlayer np = Network.player;
		networkView.RPC("CreatePlayer", RPCMode.Others, np, false, loc);
		
	}
	
	[RPC]
	void CreatePlayer(NetworkPlayer networkPlayer, bool isMe, int loc){
		player = MMG.playerList[networkPlayer];
		player.avatar = Instantiate(playerPrefab, GameObject.Find("SpawnPoint"+loc).transform.position, new Quaternion(0f,0f,0f,0f)) as GameObject;
		if(isMe){
			player.avatar.GetComponent<CarpetControl>().isMyPlayer = true;
			player.avatar.GetComponentInChildren<Camera>().enabled = true;
		} else player.avatar.GetComponentInChildren<Camera>().enabled = false;
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
