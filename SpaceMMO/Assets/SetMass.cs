using UnityEngine;
using System.Collections;

public class SetMass : MonoBehaviour 
{
	public float massMultiplier = 1;

	// Use this for initialization
	void Start ()
	{
		rigidbody.mass = transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.z * massMultiplier;
		rigidbody.drag *= rigidbody.mass;
		rigidbody.angularDrag *= rigidbody.mass;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
