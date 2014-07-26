using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public Vector3 vel;
	public int speed = 100;
	public float damage = 1.0f;
	public bool dead = false;
	public bool madeByPlayer = false;
	public Vector3 shootLoc;
	public int range = 50;
	public bool showHitNotification = false;
	public GUISkin guiSkin1;
	public bool homing = false;
	public int deathTime = 500;
	public float curveLimit = .02f;
	public int lifeTimer = 0;
	public bool rightIsForward = true;
	public bool multiplayer = true;
	public GameObject shooter;
	TrailRenderer trail;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((Vector3.Distance(transform.position, shootLoc) > range && !homing) || (lifeTimer > deathTime && homing))
			dead = true;
		if (!dead)
		{
			if (rightIsForward)
				rigidbody.velocity = transform.right.normalized * speed;
			else
				rigidbody.velocity = transform.forward.normalized * speed;
			if (homing)
			{
				lifeTimer ++;
				if (!madeByPlayer)
				{
					if (GameObject.Find("Player") == null)
						vel = FindPlayer().transform.position - transform.position;
					else
						vel = GameObject.Find("Player").transform.position - transform.position;
				}
				else if (multiplayer && madeByPlayer && GameObject.FindGameObjectsWithTag("Player").Length > 1)
				{
					var closestPlayer = 0;
					if (GameObject.FindGameObjectsWithTag("Player")[closestPlayer] == shooter)
						closestPlayer = 1;
					for (var i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i ++)
						if (Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Player")[closestPlayer].transform.position) > Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Player")[i].transform.position))
							closestPlayer = i;
					vel = GameObject.FindGameObjectsWithTag("Player")[closestPlayer].transform.position - transform.position;
				}
				else if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
				{
					var closestEnemy = 0;
					for (var i2 = 0; i2 < GameObject.FindGameObjectsWithTag("Enemy").Length; i2 ++)
						if (Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Enemy")[closestEnemy].transform.position) > Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Enemy")[i2].transform.position))
							closestEnemy = i2;
					vel = GameObject.FindGameObjectsWithTag("Enemy")[closestEnemy].transform.position - transform.position;
				}
				vel = Vector3.ClampMagnitude(vel, curveLimit);
				if (rightIsForward)
					transform.right += vel;
				else
					transform.forward += vel;
			}
		}
		else
			rigidbody.velocity = Vector3.zero;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().pvpMode)
		{
			if (madeByPlayer && other.gameObject != shooter && !dead)
			{
				other.gameObject.SendMessage("ApplyShieldDamage", damage, SendMessageOptions.DontRequireReceiver);
				dead = true;
				StartCoroutine(NotifyHit ());
			}
		}
		else
		{
		if (madeByPlayer && other.gameObject.tag == "Enemy" && !dead)
		{
			other.gameObject.GetComponent<Enemy>().enabled = true;
			other.gameObject.GetComponent<Enemy>().hp -= damage;
			dead = true;
			StartCoroutine(NotifyHit ());
			GameObject.Instantiate(other.GetComponent<Enemy>().explosion, transform.position, Quaternion.identity);
		}
		else if (other.gameObject.tag == "Player" && !madeByPlayer && !dead)
		{
			other.gameObject.SendMessage("ApplyShieldDamage", damage, SendMessageOptions.DontRequireReceiver);
			dead = true;
		}
		else if (!madeByPlayer && other.gameObject.name == "Player" && !dead)
		{
			if (other.gameObject.GetComponent<Player2>().shielded)
				other.gameObject.GetComponent<Player2>().shield -= damage;
			else
				other.gameObject.GetComponent<Player2>().hp -= damage;
			dead = true;
		}
		else if (other.gameObject.tag == "Collider" && !dead)
		{
			dead = true;
		}
		}
	}
	
	void OnGUI ()
	{
		GUI.skin = guiSkin1;
		if (showHitNotification)
			GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 125, 100, 100), "HIT!");
	}
	
	IEnumerator NotifyHit ()
	{
		showHitNotification = true;
		yield return new WaitForSeconds(0.5f);
		showHitNotification = false;
	}

	Transform FindPlayer ()
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
}
