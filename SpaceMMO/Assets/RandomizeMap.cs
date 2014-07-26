using UnityEngine;
using System.Collections;

public class RandomizeMap : MonoBehaviour
{
	public int asteroidsRemaining = 100;
	public GameObject asteroid;
	public int gridDetail = 10;
	float mapSizeX;
	float mapSizeY;
	float mapSizeZ;
	GameObject go;

	void Awake ()
	{
		mapSizeX = GameObject.Find("Map").GetComponent<BoxCollider>().size.x;
		mapSizeY = GameObject.Find("Map").GetComponent<BoxCollider>().size.y;
		mapSizeZ = GameObject.Find("Map").GetComponent<BoxCollider>().size.z;
		while (asteroidsRemaining > 0)
			for (float x = -mapSizeX / 2; x < mapSizeX / 2; x += gridDetail)
				for (float y = -mapSizeY / 2; y < mapSizeY / 2; y += gridDetail)
					for (float z = -mapSizeZ / 2; z < mapSizeZ / 2; z += gridDetail)
						if (Random.Range(0, mapSizeX * mapSizeY * mapSizeZ / asteroidsRemaining / gridDetail) < 1)
					{
						//Debug.Log(mapSizeX * mapSizeY * mapSizeZ / asteroidsRemaining / gridDetail);
						go = (GameObject) GameObject.Instantiate(asteroid, new Vector3(x, y, z), Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)));
						asteroidsRemaining --;
					}
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
