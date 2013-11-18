using UnityEngine;
using System.Collections;

public class CarpetControl : MonoBehaviour {
	
	public CharacterController playC;
	public float speed;
	public GameObject enemy;
	
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
	
	
	// Use this for initialization
	void Start () {
		moveDir = Vector3.zero;
		WizardGUIScript.setMana(50);
		WizardGUIScript.setHealth(100);
		enemy = GameObject.Find("OtherPlayer");
		enemy.GetComponentInChildren<Camera>().enabled = false;
		
		//enemy.GetComponentInChildren<Camera>().enabled = false;
		spells = new Spells();
		
	}
	
	// Update is called once per frame
	void Update(){
		
		WizardGUIScript.addMana(1f*Time.deltaTime);
		//WizardGUIScript.addHealth(0.2f*Time.deltaTime);
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
			break;
			case 1:
			if(Input.GetButtonUp("Fire1")&&WizardGUIScript.getMana()>1){
				WizardGUIScript.addMana(-1);
				networkView.RPC("castSpell", RPCMode.AllBuffered, "fireball", look.transform.position, look.transform.forward, look.transform.rotation, player);
			}
			if(Input.GetButtonDown("Fire2")&&WizardGUIScript.getMana()>2){
				WizardGUIScript.addMana(0);
				networkView.RPC("castSpell", RPCMode.AllBuffered, "shield", look.transform.position, look.transform.forward, look.transform.rotation, player);
			}
			break;
			case 2:
				if(Input.GetButtonUp("Fire1")&&WizardGUIScript.getMana()>2){
				WizardGUIScript.addMana(-2);
				networkView.RPC("castSpell", RPCMode.AllBuffered, "bouncer", look.transform.position, look.transform.forward, look.transform.rotation, player);
			}	
			if(Input.GetButtonDown("Fire2")&&WizardGUIScript.getMana()>2){
				WizardGUIScript.addMana(0);
				networkView.RPC("castSpell", RPCMode.AllBuffered, "wall", look.transform.position, look.transform.forward, look.transform.rotation, player);
			}
			break;
			}
			
			
			
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
		GameObject fb = null;
		GameObject target=null;
		GameObject soundSrc = null;
		if(!shot.Equals(player)){
			target = gameObject;
			soundSrc = enemy;
		}
		else{
			target = enemy;
			soundSrc = gameObject;
		}
		
		
		switch(sName){
		case "homing": 
			fb = (GameObject)Instantiate(spells.homing, pos + forw * 5, rot);
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
			fb = (GameObject)Instantiate(spells.forwardShield, pos + forw * 8, rot);
			fb.GetComponent<ForwardShieldBehavior>().setEnemy(target);
			fb.GetComponent<ForwardShieldBehavior>().setControl(look.gameObject);
			break;
		case "wall":
			fb = (GameObject)Instantiate(spells.wall, pos + forw * 8, rot);
			fb.GetComponent<WallBehavior>().setEnemy(target);
			fb.GetComponent<WallBehavior>().setControl(gameObject);
			break;
		}
	}
	void OnCollisionEnter(Collision collision) {
		Debug.Log("test");
        Debug.Log(collision.collider.name);
		
		Debug.Log(collision.gameObject.name);
		//Debug.Log (collision.);
		int i=1;
        
    }
	
	public void detract(int i){
		WizardGUIScript.addHealth(-i);
	}
	
	
	
	
}
