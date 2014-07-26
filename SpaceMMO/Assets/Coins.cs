using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour
{
	public int amount;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<Player>().gold += amount;
			Destroy(gameObject);
		}
	}
}
