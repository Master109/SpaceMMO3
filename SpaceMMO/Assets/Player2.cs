using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour
{
	public int speed = 25;
	public float hp = 100.0f;
	public GameObject go;
	public GameObject bullet;
	public GameObject bullet2;
	public int shootTimer = 0;
	public int shootRate = 5;
	public int shootTimer2 = 0;
	public int shootRate2 = 250;
	public GUISkin guiSkin1;
	public GUISkin guiSkin2;
	public RaycastHit hit;
	public bool showText = false;
	public bool shielded = false;
	public float shield = 100.0f;
	public int shieldMax = 100;
	public float shieldLossRate = .25f;
	public float shieldRechargeRate = .05f;
	public Vector3 vel;
	public bool runOnce = false;
	public bool rightIsForward = false;
	public Vector3 dir;
	public int changeDirectionTimer;

	void Awake ()
	{
		Screen.lockCursor=true;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		rigidbody.velocity = transform.right * Input.GetAxis("Vertical") * speed + (-transform.forward * Input.GetAxis("Horizontal") * speed) + (transform.up * Input.GetAxis("Z") * speed);
		shootTimer ++;
		if (shootTimer > shootRate && Input.GetAxisRaw("Fire1") == 1)
		{
			go = (GameObject) GameObject.Instantiate(bullet, transform.position, transform.rotation);
			go.GetComponent<Bullet>().shooter = gameObject;
			go.GetComponent<Bullet>().shootLoc = transform.position;
			go.GetComponent<Bullet>().multiplayer = false;
			go.GetComponent<Bullet>().madeByPlayer = true;
			shootTimer = 0;
		}
		shootTimer2 ++;
		if (shootTimer2 > shootRate2 && Input.GetAxisRaw("Fire2") == 1)
		{
			go = (GameObject) GameObject.Instantiate(bullet2, transform.position, transform.rotation);
			go.GetComponent<Bullet>().shooter = gameObject;
			go.GetComponent<Bullet>().shootLoc = transform.position;
			go.GetComponent<Bullet>().multiplayer = false;
			go.GetComponent<Bullet>().madeByPlayer = true;
			shootTimer2 = 0;
		}
		if (Input.GetAxisRaw("Jump") == 1 && !shielded && shield >= shieldMax / 2)
			shielded = true;
		if (shielded)
		{
			shield -= shieldLossRate;
			if (shield <= 0)
			{
				shield = 0;
				shielded = false;
			}
		}
		else if (shield < shieldMax)
		{
			shield += shieldRechargeRate;
		}
		if (hp <= 0)
			Application.LoadLevel(Application.loadedLevel);
	}

	void OnGUI ()
	{
		GUI.Label(new Rect(0, 0, 99999, 99999), "HP: " + Mathf.RoundToInt(hp));
		if (shielded)
			GUI.Label(new Rect(0, 10, 99999, 99999), "Shield Remaining: " + Mathf.RoundToInt(shield));
		else
		{
			if (shield < shieldMax / 2)
				GUI.Label(new Rect(0, 10, 99999, 99999), "Shield Recharging: " + Mathf.RoundToInt(shield) + "%");
			else if (shield < shieldMax)
				GUI.Label(new Rect(0, 10, 99999, 99999), "Shield Recharging: " + Mathf.RoundToInt(shield) + "% (Ready to use)");
			else
				GUI.Label(new Rect(0, 10, 99999, 99999), "Shield: " + Mathf.RoundToInt(shield) + "% (Ready to use)");
		}
	}
}
