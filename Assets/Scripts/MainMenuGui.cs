using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuGui : MonoBehaviour {

	public string gameName = "Player Name";
	string typeName = "RealityFlux0.1";
	private HostData[] hostList;
	private bool isRefreshingHostList = false;
	
	public static MainMenuGui MMG;
	
	bool hosting = false;
	bool joined = false;
	bool loaded = false;
	
	public class PlayerInfo
	{
		public NetworkPlayer networkPlayer;
		public string name;
		public GameObject avatar;
	}

	public Dictionary<NetworkPlayer,PlayerInfo> playerList = new Dictionary<NetworkPlayer,PlayerInfo>();
	
	void Awake(){
		DontDestroyOnLoad(this);
		MMG = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update()
    {
        if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            isRefreshingHostList = false;
            hostList = MasterServer.PollHostList();
        }
    }

    private void RefreshHostList()
    {
        if (!isRefreshingHostList)
        {
            isRefreshingHostList = true;
            MasterServer.RequestHostList(typeName);
        }
    }
	
	void OnGUI() {
		
		if(!loaded){
		
		GUILayout.BeginVertical("box");
		GUILayout.Label("Reality Flux");
		
		if(GUILayout.Button("test")){
			Application.LoadLevel("firstScene");
			loaded = true;
		}
		
		if(!hosting && !joined){
			gameName = GUILayout.TextField(gameName);
			PlayerPrefs.SetString("playerName", gameName);
			if(GUILayout.Button("Host Game")){
				PlayerPrefs.Save();
				StartServer();
				
			}
			if(GUILayout.Button("Search for games")){
				RefreshHostList();
			}
			if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(200, 25 + (35 * i), 200, 30), hostList[i].gameName)){
						PlayerPrefs.Save();
						JoinServer(hostList[i]);
						joined = true;
					}
					
                }
            }
		}
		if(hosting){
			
			if(GUILayout.Button("Stop Hosting Server")){
				StopServer();
			}
			if(Network.connections.Length > 0){
				GUILayout.Label("Player Found!");
				if(GUILayout.Button("Start Game")){
					//assign players 1 and 2
					
					//Start Game	
					networkView.RPC( "LoadLevel", RPCMode.AllBuffered, "firstScene");
					
				}
			}
			else GUILayout.Label("Waiting for second player");
		}
		if(joined){
			GUILayout.Label("Waiting for host to start game");
			if(GUILayout.Button("Disconnect")){
				Network.Disconnect(100);
				joined = false;
			}
		}
		
		
		
		GUILayout.EndVertical();
		}
	}
	
	private void StartServer()
    {
		hosting = true;
        Network.InitializeServer(10, 25001, !Network.HavePublicAddress());
		MasterServer.dedicatedServer = true;
        MasterServer.RegisterHost(typeName, gameName + "'s game");
    }
	
	private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }
	
	private void StopServer(){
		while(Network.connections.Length > 0){
			Network.CloseConnection(Network.connections[0],true);
		}
		Network.Disconnect(100);
		MasterServer.UnregisterHost();
	}
	
	[RPC]
	void EditPlayer(NetworkPlayer networkPlayer, string pname){
		PlayerInfo temp = new PlayerInfo();
		temp.networkPlayer = networkPlayer;
		temp.name = name;
		temp.avatar = null;
		playerList[networkPlayer] = temp;
	}
	
	
	void OnServerInitialized(){
		networkView.RPC("EditPlayer", RPCMode.AllBuffered, Network.player, PlayerPrefs.GetString("playerName")); 
	}
	
	void OnConnectedToServer(){
		networkView.RPC("EditPlayer", RPCMode.AllBuffered, Network.player, PlayerPrefs.GetString("playerName"));
	}
	
	[RPC]
	void LoadLevel(string level){
		Application.LoadLevel(level);
		loaded = true;
	}
	
}
