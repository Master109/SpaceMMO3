using UnityEngine;
using System.Collections;

public class WolfControl : MonoBehaviour {
	public GameObject wolf;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void RunAway () {
		Instantiate(Resources.Load("WolfSmoke"),transform.position,Quaternion.identity);
		wolf.SetActive(false);
		//Destroy(gameObject);
	}
}
