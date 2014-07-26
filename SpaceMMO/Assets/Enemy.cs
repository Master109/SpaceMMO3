using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public int speed = 10;
	public float hp = 100.0f;
	public int[] attackRanges;
	GameObject go;
	//public GameObject bullet;
	public GameObject[] bullets;
	//public float shootTimer = 0;
	//public float shootRate = 10;
	float[] shootTimers;
	public float[] shootRates;
	//public bool rightIsForward = true;
	//public int directionInverse = 1;
	public Vector3 dir;
	public bool runOnce = true;
	public Vector3 vel;
	public Vector3 pLoc;
	public int changeDirectionTimer = 0;
	public int changeDirectionRateMin = 5;
	public int changeDirectionRateMax = 25;
	public int changeDirectionAmount = 1;
	public float orbitSpeed = .25f;
	public float respawnTimer = -1;
	public float respawnTimerMax = 1000;
	public bool dead = false;
	public GameObject thisGO;
	public Transform shootTrs;
	public float[] shootAngles;
	public float[] randomAngModifiers;
	public float[] coneDepths;
	public Transform spawnTrs;
	public Vector3 rotationOffset;
	public int explosionRadius;
	public GameObject shrapanel;
	public int shrapanelNum;
	public GameObject explosion;
	public GameObject coins;
	public int goldRewardDrop;
	public int goldRewardExtra;
	public int xpRewardDrop;
	public int xpRewardExtra;

	void Awake ()
	{
		//Debug.Log("AWAKE");
		spawnTrs = transform;
	}

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < bullets.Length; i ++)
		{
			if (attackRanges[i] == -1)
			{
				if (bullets[i].GetComponent<Bullet>().homing)
					attackRanges[i] = bullets[i].GetComponent<Bullet>().deathTime * (bullets[i].GetComponent<Bullet>().speed / 10) / 2;
				else
					attackRanges[i] = bullets[i].GetComponent<Bullet>().range;
			}
		}
		shootTimers = new float[bullets.Length];
		vel = FindPlayer().position - transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (hp <= 0)
		{ 
			if (transform.root.name != "RespawnGroup")
				Destroy(gameObject);
			else if (!dead)
			{
				respawnTimer = respawnTimerMax;
				renderer.enabled = false;
				collider.enabled = false;
				tag = "Untagged";
				go = (GameObject) Network.Instantiate(coins, transform.position, Quaternion.identity, 1);
				go.GetComponent<Coins>().amount = goldRewardDrop;
				for (int i = 0; i <= shrapanelNum; i ++)
				{
					Vector3 offset = (new Vector3(Random.Range(-9999999999, 9999999999), Random.Range(-9999999999, 9999999999), Random.Range(-9999999999, 9999999999)) * 1).normalized * explosionRadius;
					go = (GameObject) Network.Instantiate(shrapanel, transform.position + offset, Quaternion.identity, 1);
				}
				Collider[] players = Physics.OverlapSphere(transform.position, 250, LayerMask.NameToLayer("Player"));
				foreach (Collider c in players)
				{
					c.GetComponent<Player>().gold += goldRewardExtra;
					c.GetComponent<Player>().xp += xpRewardExtra;
				}
				dead = true;
			}
			respawnTimer -= Time.deltaTime;
			if (respawnTimer < 0 && respawnTimer + Time.deltaTime > 0)
			{
				go = (GameObject) Network.Instantiate(thisGO, spawnTrs.position, spawnTrs.rotation, 1);
				go.transform.parent = thisGO.transform.parent;
				Enemy e = go.GetComponent<Enemy>();
				Destroy(gameObject);
			}
		}
		if (dead)
			return;
		for (int i = 0; i < bullets.Length; i ++)
		{
		shootTimers[i] += Time.deltaTime;
		if (Vector3.Distance(transform.position, FindPlayer().position) > attackRanges[i])
		{
			vel = FindPlayer().position - transform.position;
			rigidbody.velocity = vel.normalized * speed;
			runOnce = true;
		}
		else
		{
			vel = FindPlayer().position - transform.position;
			if (runOnce)
			{
				changeDirectionTimer = Random.Range(changeDirectionRateMin, changeDirectionRateMax);
				dir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
				runOnce = false;
			}
			changeDirectionTimer --;
			if (changeDirectionTimer == 0)
			{
				changeDirectionTimer = Random.Range(changeDirectionRateMin, changeDirectionRateMax);
				float r = Mathf.RoundToInt(Random.Range(1, 3));
				if (r == 1)
					dir.x += Random.Range(-changeDirectionAmount, changeDirectionAmount);
				else if (r == 2)
					dir.y += Random.Range(-changeDirectionAmount, changeDirectionAmount);
				else if (r == 3)
					dir.z += Random.Range(-changeDirectionAmount, changeDirectionAmount);
			}
			pLoc = transform.position;
			float radius = Vector3.Distance(transform.position, FindPlayer().position);
			float circumference = radius * 2 *  Mathf.PI;
			transform.RotateAround(FindPlayer().position, dir, (circumference / (radius * (speed / 10))) * orbitSpeed);
			RotateToForward ();
			if (shootTimers[i] > shootRates[i])
			{
				Vector3 bulletVel;
				if (coneDepths[i] != 0)
				{
				float shootAng = shootAngles[i] + Random.Range(-randomAngModifiers[i], randomAngModifiers[i]);
				float bulletVelX = Mathf.Cos(shootAng);
				float bulletVelY = Mathf.Sin(shootAng);
				Vector3 toPlayer = FindPlayer().position - shootTrs.position;
				bulletVel = Vector3.zero;
				bulletVel += transform.forward * 1 / (coneDepths[i] + Random.Range(-coneDepths[i], 0));
				bulletVel += transform.right * bulletVelX;
				bulletVel += transform.up * bulletVelY;
				}
				else
					bulletVel = FindPlayer().position - transform.position;
				go = (GameObject) Network.Instantiate(bullets[i], shootTrs.position, Quaternion.LookRotation(bulletVel), 1);
				go.GetComponent<Bullet>().multiplayer = false;
				go.GetComponent<Bullet>().shootLoc = transform.position;
				go.GetComponent<Bullet>().range = attackRanges[i];
				shootTimers[i] = 0;
			}
		}
		}
	}

	void RotateToForward ()
	{
		transform.LookAt(FindPlayer().position);
		transform.Rotate(rotationOffset);
	}

	Transform FindPlayer ()
	{
		if (Application.loadedLevelName != "Survival")
		{
		int closestPlayerID = 0;
		for (int i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i ++)
		{
			GameObject player = GameObject.FindGameObjectsWithTag("Player")[i];
			if (Vector3.Distance (transform.position, player.transform.position) < Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Player")[closestPlayerID].transform.position))
				closestPlayerID = i;
		}
		GameObject closestPlayer = GameObject.FindGameObjectsWithTag("Player")[closestPlayerID];
		return closestPlayer.transform;
		}
		else
			return GameObject.Find("Player").transform;
	}
}
