using UnityEngine;
using System.Collections;

public class Shrapanel : MonoBehaviour
{
	public float sizeMin;
	public float sizeMax;
	public int forceMin;
	public int forceMax;
	public int torqueMin;
	public int torqueMax;
	public int damage;

	// Use this for initialization
	void Start ()
	{
		transform.localScale = new Vector3(Random.Range(sizeMin, sizeMax), Random.Range(sizeMin, sizeMax), Random.Range(sizeMin, sizeMax));
		rigidbody.AddForce((new Vector3(Random.Range(-9999999999, 9999999999), Random.Range(-9999999999, 9999999999), Random.Range(-9999999999, 9999999999)) * 1).normalized * Random.Range(forceMin, forceMax));
		rigidbody.AddTorque((new Vector3(Random.Range(-9999999999, 9999999999), Random.Range(-9999999999, 9999999999), Random.Range(-9999999999, 9999999999)) * 1).normalized * Random.Range(torqueMin, torqueMax));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (rigidbody.velocity.magnitude < 1)
			Destroy(gameObject);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
		Destroy(gameObject);
	}
}
