using UnityEngine;
using System.Collections;

public class RandomScale : MonoBehaviour
{
	public float sizeMin;
	public float sizeMax;
	
	// Use this for initialization
	void Awake ()
	{
		float size = Random.Range(sizeMin, sizeMax);
		transform.localScale = Vector3.one * size;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
