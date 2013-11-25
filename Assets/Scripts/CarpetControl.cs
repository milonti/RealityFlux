using UnityEngine;
using System.Collections;

public class CarpetControl : MonoBehaviour {
	
	public CharacterController playC;
	public float speed;
	public GameObject enemy;
	GameObject fb;
	GameObject shield;
	public float wallCount;
	public Vector3 moveDir;

	Spells spells;
	
	public Camera look;
	
	public float sensitivityX;
	public float sensitivityY;
	
	float rotationY = 0f;
	
	public float minimumY = -45f;
	public float maximumY = 45f;
	
	public bool isMyPlayer = false;
	public string player=null;
	public int health = 90;
	
	public AudioClip fireballSound;
	public AudioClip homingSound;
	public AudioClip capsuleSound;
	public AudioClip shieldSound;
	
	public bool winner;
	public bool loser;
	public bool gameOver;
	public float overTimer;
	public string status;
	
	// Use this for initialization
	void Start () {
		status = "";
		winner = false;
		loser = false;
		gameOver = false;
		
		moveDir = Vector3.zero;
		WizardGUIScript.setMana(50);
		WizardGUIScript.setHealth(100);
		enemy = GameObject.Find("OtherPlayer");
		spells = new Spells();
		
		wallCount=10;
		overTimer = 0;
	}
	
	// Update is called once per frame
	void Update(){
		
		if (!gameOver) {
			WizardGUIScript.addMana(1f*Time.deltaTime);
			if(enemy==null)enemy = GameObject.Find("OtherPlayer");
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
				
				
				if(moveDir.magnitude > 0.001){
					playC.Move(moveDir * Time.deltaTime * speed);
					networkView.RPC ("movePlayer", RPCMode.OthersBuffered, transform.position, player, transform.rotation);
				}	
				else WizardGUIScript.addMana(2f*Time.deltaTime);
				//spells go here
				 if (Input.GetAxis("Mouse ScrollWheel") < 0) {
					WizardGUIScript.spellsUp();
				}
				if (Input.GetAxis("Mouse ScrollWheel") > 0) WizardGUIScript.spellsDown();
				
				switch(WizardGUIScript.getSpellSet())
				{
				case 0:
				if(Input.GetButtonUp("Fire1")&&WizardGUIScript.getMana()>2){
					WizardGUIScript.addMana(-2);
					networkView.RPC("castSpell", RPCMode.AllBuffered, "homing", look.transform.position, look.transform.forward, look.transform.rotation, player);
				}
				if(Input.GetButtonDown("Fire2") && WizardGUIScript.getMana() > 2){
					WizardGUIScript.addMana(-3);
					networkView.RPC("castSpell", RPCMode.AllBuffered, "boulder", look.transform.position, look.transform.forward, look.transform.rotation, player);
				}
				break;
				case 1:
				if(Input.GetButtonUp("Fire1")&&WizardGUIScript.getMana()>1){
					WizardGUIScript.addMana(-1);
					networkView.RPC("castSpell", RPCMode.AllBuffered, "fireball", look.transform.position, look.transform.forward, look.transform.rotation, player);
				}
				if(Input.GetButtonDown("Fire2") && WizardGUIScript.getMana() > 2){
					WizardGUIScript.addMana(-3);
					networkView.RPC("castSpell", RPCMode.AllBuffered, "shield", look.transform.position, look.transform.forward, look.transform.rotation, player);
				}
				if(Input.GetButton("Fire2") && WizardGUIScript.getMana() > 2){
					networkView.RPC("setPosRot", RPCMode.AllBuffered, look.transform.position + look.transform.forward * 8, look.transform.rotation, player);
				}
				break;
				case 2:
					if(Input.GetButtonUp("Fire1") && WizardGUIScript.getMana()>2){
					WizardGUIScript.addMana(-2);
					networkView.RPC("castSpell", RPCMode.AllBuffered, "bouncer", look.transform.position, look.transform.forward, look.transform.rotation, player);
				}	
				if(Input.GetButtonDown("Fire2") && WizardGUIScript.getMana()>2 && wallCount>5){
					WizardGUIScript.addMana(0);
					networkView.RPC("castSpell", RPCMode.AllBuffered, "wall", look.transform.position, look.transform.forward, look.transform.rotation, player);
					wallCount=0;
				}
				break;
				}
				wallCount+=Time.deltaTime;
				if(Input.GetButtonUp("Fire2")) networkView.RPC("endShield", RPCMode.All, player);
				
			}
			if(winner || loser) gameOver = true;
		}
		
		
	}
	
	void OnGUI(){
		if(gameOver){
			status = "";
			if(winner) status = "VICTORY";
			else if(loser) status = "DEFEAT";
			
			GUI.Box ( new Rect( Screen.width/2-40,Screen.height/2-20,80,40), status,"button");
			if(overTimer >= 2 && Screen.lockCursor) Screen.lockCursor = false;
			else overTimer += Time.deltaTime;
		}
	}
	
	
	[RPC]
	void movePlayer(Vector3 dir, string info, Quaternion rot)
	{	
		if(!info.Equals(player)) {
			//GameObject g = GameObject.Find("OtherPlayer");
			
			enemy.transform.position = dir;
			enemy.transform.rotation = rot;
		}
		
	}
	
	[RPC]
	void castSpell(string sName, Vector3 pos, Vector3 forw, Quaternion rot, string shot){
		
		GameObject target=null;
		GameObject soundSrc = null;
		GameObject controller = null;
		
		
		if(!shot.Equals(player)){
			target = gameObject;
			soundSrc = enemy;
			controller = enemy;
		}
		else{
			target = enemy;
			soundSrc = gameObject;
			controller = gameObject;
		}
		
		soundSrc = target;
		
		switch(sName){
		case "homing": 
			fb = (GameObject)Instantiate(spells.homing, pos + forw * 5, rot);
			//fb = (GameObject)Instantiate(spells.homing, pos + forw * 5, rot);
			fb.GetComponent<FireballBehavior>().setEnemy(target);
			fb.GetComponent<FireballBehavior>().setControl(gameObject);
			soundSrc.audio.PlayOneShot(homingSound);
			break;
		case "fireball":
			fb = (GameObject)Instantiate(spells.fireball, pos + forw * 5, rot);
			fb.GetComponent<SFireballBehavior>().setEnemy(target);
			fb.GetComponent<SFireballBehavior>().setControl(gameObject);
			soundSrc.audio.PlayOneShot(fireballSound);
			break;
		case "bouncer":
			fb = (GameObject)Instantiate(spells.bouncer, pos + forw * 8, rot);
			fb.GetComponent<BounceBehavior>().setEnemy(target);
			fb.GetComponent<BounceBehavior>().setControl(gameObject);
			soundSrc.audio.PlayOneShot(capsuleSound);
			break;
		case "shield":
			soundSrc.audio.PlayOneShot(shieldSound);
			fb = (GameObject)Instantiate(spells.forwardShield, pos + forw * 8, rot);
			fb.GetComponent<ForwardShieldBehavior>().setEnemy(target);
			fb.GetComponent<ForwardShieldBehavior>().setControl(controller);
			fb.GetComponent<ForwardShieldBehavior>().controlID = shot;
			shield = fb;
			break;
		case "wall":
			fb = (GameObject)Instantiate(spells.wall, pos + forw * 8, rot);
			fb.GetComponent<WallBehavior>().setEnemy(target);
			fb.GetComponent<WallBehavior>().setControl(gameObject);
			break;
		case "boulder":
			fb = (GameObject)Instantiate(spells.boulder, pos + forw * 40, rot);
			fb.GetComponent<BoulderBehavior>().setEnemy(target);
			fb.GetComponent<BoulderBehavior>().setControl(gameObject);
			break;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		//Debug.Log("test");
        //Debug.Log(collision.collider.name);
		
		//Debug.Log(collision.gameObject.name);
		//Debug.Log (collision.);
		//int i=1;
        
    }
	
	public void detract(int i){
		WizardGUIScript.addHealth(-i);
	}
	
	[RPC]
	public void setPosRot(Vector3 pos, Quaternion rot, string play){
		Object[] f = FindObjectsOfType(typeof(ForwardShieldBehavior));
		foreach(Object o in f){
			ForwardShieldBehavior d = (ForwardShieldBehavior) o;
			d.setPosRot(pos, rot, play);
		}
	}
	
	[RPC]
	public void endShield(string play){
		Object[] f = FindObjectsOfType(typeof(ForwardShieldBehavior));
		foreach(Object o in f){
			ForwardShieldBehavior d = (ForwardShieldBehavior) o;
			d.die(play);
		}
	}
	
	[RPC] 
	public void loserStatus(string p){
		if(!p.Equals(player)){
			//set that I won
			winner = true;
		}
	}
	
	public void sendLoser(string p){
		networkView.RPC("loserStatus", RPCMode.OthersBuffered, p);
		loser = true;
	}
	
}
