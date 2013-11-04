﻿using UnityEngine;
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
	public int health = 100;
	
	// Use this for initialization
	void Start () {
		moveDir = Vector3.zero;
		enemy = GameObject.Find("OtherPlayer");
		spells = new Spells();
		if(!(Network.player.ToString() == player)){
			GetComponentInChildren<AudioListener>().enabled = false;
			GetComponentInChildren<Camera>().enabled = false;
		}
		
	}
	
	// Update is called once per frame
	void Update(){
		
		
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
			//spells go here
			if(Input.GetButtonUp("Fire1")){
				networkView.RPC("castSpell", RPCMode.AllBuffered, "homing", look.transform.position, look.transform.forward, look.transform.rotation, player);
			
			}
			if(Input.GetButtonUp("Fire2")){
				networkView.RPC("castSpell", RPCMode.AllBuffered, "fireball", look.transform.position, look.transform.forward, look.transform.rotation, player);
			}
			if(Input.GetButtonUp("Fire3")){
				networkView.RPC("castSpell", RPCMode.AllBuffered, "bouncer", look.transform.position, look.transform.forward, look.transform.rotation, player);
			
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
		if(!shot.Equals(player)){
			target = gameObject;
		}
		else target = enemy;
		
		switch(sName){
		case "homing": 
			fb = (GameObject)Instantiate(spells.homing, pos + forw * 3, rot);
			fb.GetComponent<FireballBehavior>().setEnemy(target);
			fb.GetComponent<FireballBehavior>().setControl(target);
			break;
		case "fireball":
			fb = (GameObject)Instantiate(spells.fireball, pos + forw * 3, rot);
			fb.GetComponent<SFireballBehavior>().setEnemy(target);
			break;
		case "bouncer":
			fb = (GameObject)Instantiate(spells.bouncer, pos + forw * 3, rot);
			fb.GetComponent<BounceBehavior>().setEnemy(target);
			break;
		}
	}
	
	
	
	
	
	
}
