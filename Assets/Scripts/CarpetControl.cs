using UnityEngine;
using System.Collections;

public class CarpetControl : MonoBehaviour {
	
	public CharacterController playC;
	public float speed;
	public GameObject enemyPlaceholder;
	
	private Vector3 moveDir;

	Spells spells;
	
	public Camera look;
	
	public float sensitivityX;
	public float sensitivityY;
	
	float rotationY = 0f;
	
	public float minimumY = -45f;
	public float maximumY = 45f;
	
	public bool isMyPlayer = false;
	public NetworkPlayer player;
	public int health = 100;
	
	// Use this for initialization
	void Start () {
		moveDir = Vector3.zero;
		enemyPlaceholder = GameObject.Find("PlaceholderEnemy");
		spells = new Spells();
		
	}
	
	// Update is called once per frame
	void Update(){
		if(isMyPlayer){
			//rotation stuff
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
			look.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		
			transform.Rotate(transform.up, sensitivityX * Input.GetAxis("Mouse X"));
		
		
			//movement stuff
			moveDir = Vector3.zero;
		
			moveDir += transform.forward * Input.GetAxis("Vertical");
			moveDir += transform.right * Input.GetAxis("Horizontal");
			moveDir += transform.up * Input.GetAxis("Ascend");
			
			NetworkPlayer np = Network.player;
			
			networkView.RPC ("movePlayer", RPCMode.All, moveDir, np);
			
			//spells go here
			if(Input.GetButtonUp("Fire1")){
				networkView.RPC("castSpell", RPCMode.AllBuffered, "homing", look.transform.position, look.transform.forward, look.transform.rotation);
			
			}
			if(Input.GetButtonUp("Fire2")){
				networkView.RPC("castSpell", RPCMode.AllBuffered, "fireball", look.transform.position, look.transform.forward, look.transform.rotation);
			}
			if(Input.GetButtonUp("Fire3")){
				networkView.RPC("castSpell", RPCMode.AllBuffered, "bouncer", look.transform.position, look.transform.forward, look.transform.rotation);
			
			}
		}
	}
	
	[RPC]
	void movePlayer (Vector3 moveDir, NetworkPlayer np) {
		if(player == np )playC.Move(moveDir * Time.deltaTime * speed);
	}
	
	[RPC]
	void castSpell(string sName, Vector3 pos, Vector3 forw, Quaternion rot){
		GameObject fb = null;
		switch(sName){
		case "homing": 
			fb = (GameObject)Instantiate(spells.homing, pos + forw * 3, rot);
			fb.GetComponent<FireballBehavior>().setEnemy(enemyPlaceholder);
			break;
		case "fireball":
			fb = (GameObject)Instantiate(spells.fireball, pos + forw * 3, rot);
			break;
		case "bouncer":
			fb = (GameObject)Instantiate(spells.bouncer, pos + forw * 3, rot);
			fb.GetComponent<BounceBehavior>().setEnemy(enemyPlaceholder);
			break;
		}
	}
	
	
	
	
}
