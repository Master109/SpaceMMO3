using UnityEngine;
using System.Collections;
using UnityEditor;

public class SelectObjects : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		SelectionTest ();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	//[MenuItem("KVS/Selection Test")]
	
	void SelectionTest()
	{
		ArrayList objsToSelect = new ArrayList();
		foreach (GameObject g in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if ((g.GetComponent<RandomScale>() != null && g.GetComponent<RandomScale>().enabled) || (g.GetComponent<RandomRotate>() != null && g.GetComponent<RandomRotate>().enabled))
				objsToSelect.Add (g);
		}
		GameObject[] objsToSelect2 = new GameObject[GameObject.FindObjectsOfType(typeof(GameObject)).Length];
		for (int i = 0; i < objsToSelect.Count; i ++)
			objsToSelect2[i] = (GameObject) objsToSelect[i];
		Selection.objects = objsToSelect2;
	}
}
