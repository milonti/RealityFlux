using UnityEngine;
using System.Collections;

public class CarpetControl : MonoBehaviour {
	
	public CharacterController playC;
	public float speed;
	
	
	private Vector3 moveDir;

	public GameObject proj;
	public Camera cam;
	
	// Use this for initialization
	void Start () {
		moveDir = Vector3.zero;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		
		//movement stuff
		moveDir = Vector3.zero;
		
		moveDir += transform.forward * Input.GetAxis("Vertical");
		moveDir += transform.right * Input.GetAxis("Horizontal");
		
		moveDir += transform.up * Input.GetAxis("Ascend");
		
		playC.Move(moveDir * Time.deltaTime * speed);
	}
}
