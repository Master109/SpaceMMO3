using UnityEngine;
using System.Collections;

public class RandomRotate : MonoBehaviour
{

	void Awake ()
	{
		transform.rotation = Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
