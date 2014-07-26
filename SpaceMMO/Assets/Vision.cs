using UnityEngine;
using System.Collections;

public class Vision : MonoBehaviour
{

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
		if (other.gameObject.name == "Player" || other.gameObject.tag == "Player")
		{
			transform.parent.gameObject.GetComponent<Enemy>().enabled = true;
			transform.parent.Find("CallForHelp").GetComponent<CallForHelp>().enabled = true;
		}
	}
}
