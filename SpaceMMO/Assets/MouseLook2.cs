using UnityEngine;
using System.Collections;

public class MouseLook2 : MonoBehaviour {
	float rotationX = 0f;
	float rotationY = 0f;
	public float sensitivityX = 0.25f;
	public float sensitivityY = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion xQuaternion;
		Quaternion yQuaternion;

		rotationX = Input.GetAxis("Mouse X") * sensitivityX;
		rotationY = Input.GetAxis("Mouse Y") * sensitivityY;

		transform.RotateAround(transform.up, rotationX);
		transform.RotateAround(transform.forward, rotationY);
	}
}
