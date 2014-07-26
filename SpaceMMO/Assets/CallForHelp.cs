using UnityEngine;
using System.Collections;

public class CallForHelp : MonoBehaviour
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
		if (other.gameObject.tag == "Enemy")
		{
			transform.parent.gameObject.GetComponent<Enemy>().enabled = true;
			transform.parent.Find("Vision").GetComponent<Vision>().enabled = false;
		}
	}
}
