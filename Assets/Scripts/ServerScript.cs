using UnityEngine;
using System.Collections;

public class ServerScript : MonoBehaviour {
	
	bool serverStarted = false;
	string gameName = "Hosted Game";
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(!serverStarted) {
			gameName = GUILayout.TextField(gameName, 25);
			if(GUILayout.Button("Start Server")){
				//Use NAT punchthrough if no public IP present
				var useNat = !Network.HavePublicAddress();
				serverStarted = true;
				Network.InitializeServer(32, 25002, useNat);
				MasterServer.RegisterHost("RealityFlux0.1", gameName, "comment goes here");
					Debug.Log(gameName + " started");
			}
		}
		if(serverStarted) if(GUILayout.Button("Disconnect") && serverStarted){
			
			serverStarted = false;
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
	}
	

}