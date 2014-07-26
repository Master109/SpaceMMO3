#pragma strict

var createTimer = 0;
var createRate = 1000;
var enemy1 : GameObject;
var enemy2 : GameObject;
var r = 0;
var randomVector : Vector3;
var createDist = 100;
var waves = true;
var difficulty = 1;
var guiSkin1 : GUISkin;

function Start ()
{
	r = Random.Range(1, 3);
	randomVector = transform.position + (Vector3(Random.Range(-99999999, 99999999), Random.Range(-99999999, 99999999), Random.Range(-99999999, 99999999)).normalized * createDist);
	if (r == 1)
		GameObject.Instantiate(enemy1, randomVector, Quaternion.identity);
	else if (r == 2)
		GameObject.Instantiate(enemy2, randomVector, Quaternion.identity);
}

function Update ()
{
	createTimer ++;
	if (createTimer > createRate && !waves)
	{
		r = Random.Range(1, 3);
		randomVector = transform.position + (Vector3(Random.Range(-99999999, 99999999), Random.Range(-99999999, 99999999), Random.Range(-99999999, 99999999)).normalized * createDist);
		if (r == 1)
			GameObject.Instantiate(enemy1, randomVector, Quaternion.identity);
		else if (r == 2)
			GameObject.Instantiate(enemy2, randomVector, Quaternion.identity);
		createRate *= .99;
		createTimer = 0;
	}
	else if (waves && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
	{
		difficulty ++;
		for (var i = 0; i < difficulty; i ++)
		{
			r = Random.Range(1, 3);
			randomVector = transform.position + (Vector3(Random.Range(-99999999, 99999999), Random.Range(-99999999, 99999999), Random.Range(-99999999, 99999999)).normalized * createDist);
			if (r == 1)
				GameObject.Instantiate(enemy1, randomVector, Quaternion.identity);
			else if (r == 2)
				GameObject.Instantiate(enemy2, randomVector, Quaternion.identity);
		}
	}
}

function OnGUI ()
{
	GUI.skin = guiSkin1;
	GUI.Label(Rect(0, 30, 99999, 99999), "Wave: " + difficulty);
}