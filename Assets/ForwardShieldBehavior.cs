using UnityEngine;
using System.Collections;

public class ForwardShieldBehavior : MonoBehaviour {
	
	public float speed = 300;
	
	public CharacterController shieldC;
	
	public float life;
	
	public GameObject explosionParticles;
	public GameObject enemy;
	public GameObject control;
	
	public string controlID;
	
	public AudioClip metal_hit_sound;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//life+=Time.deltaTime;
		
		if(Input.GetButtonUp("Fire2")) networkView.RPC("die", RPCMode.All, controlID);
		life += Time.deltaTime;
		if(life >=3) Destroy(gameObject);
	}
	
	public void setEnemy(GameObject e){
		enemy=e;	
	}
	
	public void setControl(GameObject e){
		control=e;	
	}
	
	[RPC]
	public void die(string player)
	{
		if(player.Equals(controlID)) Destroy(gameObject);
	}
	
	
	public void setPosRot(Vector3 pos, Quaternion rot, string player){
		if(controlID.Equals(player)){
			transform.position = pos;
			transform.rotation = rot;
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		audio.PlayOneShot(metal_hit_sound);
	}
	
	public void shittyCollisionDetection(int lose){
		if(Vector3.Distance(shieldC.transform.position,control.transform.position)<3){
			Debug.Log("hit something");
			//control.GetComponent<CarpetControl>().detract(lose);
			Destroy(gameObject);
			
		}
		
	}
	
}
