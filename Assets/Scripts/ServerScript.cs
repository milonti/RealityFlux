using UnityEngine;
using System.Collections;

public class ServerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(GUILayout.Button("Start Server")){
			//Use NAT punchthrough if no public IP present
			var useNat = !Network.HavePublicAddress();
			Network.InitializeServer(32, 25002, useNat);
			MasterServer.RegisterHost("RealityFlux0.1", "Hosted Game", "comment goes here");
		}
	}
}
