using UnityEngine;
using System.Collections;

public class CarpetControl : MonoBehaviour {
	
	public CharacterController playC;
	public float speed;
	public GameObject enemyPlaceholder;
	
	private Vector3 moveDir;

	public GameObject proj;
	public GameObject proj2;
	public GameObject proj3;
	public Camera look;
	
	public float sensitivityX;
	public float sensitivityY;
	
	float rotationY = 0f;
	
	public float minimumY = -45f;
	public float maximumY = 45f;
	
	// Use this for initialization
	void Start () {
		moveDir = Vector3.zero;
		enemyPlaceholder = GameObject.Find("PlaceholderEnemy");
	}
	
	// Update is called once per frame
	void Update () {
		//spells go here
		if(Input.GetButtonUp("Fire1")){
			
			GameObject fb = (GameObject)Instantiate(proj, look.transform.position + look.transform.forward * 3, look.transform.rotation);
			fb.GetComponent<FireballBehavior>().setEnemy(enemyPlaceholder);
			
		}
		if(Input.GetButtonUp("Fire2")){
			
			GameObject fb = (GameObject)Instantiate(proj2, look.transform.position + look.transform.forward * 3, look.transform.rotation);
			
			
		}
		if(Input.GetButtonUp("Fire3")){
			
			GameObject fb = (GameObject)Instantiate(proj3, look.transform.position + look.transform.forward * 3, look.transform.rotation);
			fb.GetComponent<BounceBehavior>().setEnemy(enemyPlaceholder);
			
		}
		
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
		
		playC.Move(moveDir * Time.deltaTime * speed);
	}
}
