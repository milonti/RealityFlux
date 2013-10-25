using UnityEngine;
using System.Collections;

public class FirstPerson : MonoBehaviour {
	
	public Camera look;
	
	public float sensitivityX;
	public float sensitivityY;
	
	float rotationY = 0f;
	
	public float minimumY = -45f;
	public float maximumY = 45f;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		look.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		
		transform.Rotate(transform.up, sensitivityX * Input.GetAxis("Mouse X"));
		
		
	}
}
