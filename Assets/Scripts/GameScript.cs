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
		NetworkPlayer np = Network.player;
		int loc = 1;
		if(Network.peerType == NetworkPeerType.Server) loc = 2;
		CreatePlayer(np, true, loc);
		networkView.RPC("CreatePlayer", RPCMode.Others, np, false, loc);
		
//		foreach(NetworkPlayer np in Network.connections){
//			if(np == Network.player){
//				if(Network.peerType == NetworkPeerType.Server) loc = 2;
//				else loc = 1;
//				CreatePlayer(np, true, loc);
//				networkView.RPC("CreatePlayer", RPCMode.Others, np, false, loc);
//			} else{
//				if(Network.peerType == NetworkPeerType.Server) loc = 1;
//				else loc = 2;
//				CreatePlayer(np, false, loc);
//				networkView.RPC("CreatePlayer", RPCMode.Others, np, true, loc);
//			}
//		}
		
	}
	
	[RPC]
	void CreatePlayer(NetworkPlayer np, bool isMe, int loc){
		player = MMG.playerList[np];
		MMG.playerList[np].avatar = Instantiate(playerPrefab, GameObject.Find("SpawnPoint"+loc).transform.position, new Quaternion(0f,0f,0f,0f)) as GameObject;
		if(isMe){
			MMG.playerList[np].avatar.GetComponent<CarpetControl>().isMyPlayer = true;
			MMG.playerList[np].avatar.GetComponentInChildren<Camera>().enabled = true;
		} else MMG.playerList[np].avatar.GetComponentInChildren<Camera>().enabled = false;
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
