using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public int speed = 25;
	public float hp = 100.0f;
	public int xp;
	public int gold;
	public GameObject go;
	public GameObject bullet;
	public GameObject bullet2;
	public float shootTimer = 0;
	public float shootRate = 5;
	public float shootTimer2 = 0;
	public float shootRate2 = 250;
	public GUISkin guiSkin1;
	public GUISkin guiSkin2;
	public RaycastHit hit;
	public bool showText = false;
	public bool shielded = false;
	public float shield = 100.0f;
	public int shieldMax = 100;
	public float shieldLossRate = .25f;
	public float shieldRechargeRate = .05f;
	public bool pvpMode = false;
	public bool mmoMode = false;

	//public string questInfo = new string[1];

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		//GameObject.Find("Generalscripts").GetComponent<Quest>().Test();
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncPosition = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody.position;
		}
	}
	
	void Awake()
	{
		lastSynchronizationTime = Time.time;
	}
	
	void Update()
	{
		if (networkView.isMine)
		{
			rigidbody.velocity = transform.right * Input.GetAxis("Vertical") * speed + (-transform.forward * Input.GetAxis("Horizontal") * speed) + (transform.up * Input.GetAxis("Z") * speed);
			shootTimer += 1;
			if (shootTimer > shootRate && Input.GetAxisRaw("Fire1") == 1)
			{
				go = (GameObject) Network.Instantiate(bullet, transform.Find("ShootPoint").position, transform.Find("ShootPoint").rotation, 1);
				Bullet b = go.GetComponent<Bullet>();
				b.shooter = gameObject;
				b.shootLoc = transform.position;
				b.madeByPlayer = true;
				shootTimer = 0;
			}
			shootTimer2 ++;
			if (shootTimer2 > shootRate2 && Input.GetAxisRaw("Fire2") == 1)
			{
				go = (GameObject) Network.Instantiate(bullet2, transform.Find("ShootPoint").position, transform.Find("ShootPoint").rotation, 1);
				Bullet b = go.GetComponent<Bullet>();
				b.shooter = gameObject;
				b.shootLoc = transform.position;
				b.madeByPlayer = true;
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
			networkView.RPC("SetShieldVars", RPCMode.Others, shielded, shield);
		}
		else
		{
			SyncedMovement();
		}
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	void OnGUI ()
	{
		if (networkView.isMine)
		{
			Rect rect = new Rect(0, 0, 99999, 99999);
			GUI.Label(rect, "HP: " + Mathf.RoundToInt(GetComponent<PlayerScript>().hp).ToString());
			rect = new Rect(0, 10, 99999, 99999);
			if (shielded)
				GUI.Label(rect, "Shield Remaining: " + Mathf.RoundToInt(shield).ToString());
			else
			{
				if (shield < shieldMax / 2)
					GUI.Label(rect, "Shield Recharging: " + Mathf.RoundToInt(shield) + "%");
				else if (shield < shieldMax)
					GUI.Label(rect, "Shield Recharging: " + Mathf.RoundToInt(shield) + "% (Ready to use)");
				else
					GUI.Label(rect, "Shield: " + Mathf.RoundToInt(shield) + "% (Ready to use)");
			}
		}
	}
	
	void ApplyShieldDamage (int damage)
	{
		if (shielded)
			shield -= damage;
		else
			gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
	}
	
	[RPC]
	void SetShieldVars (bool isShielded, float remainingShield)
	{
		shielded = isShielded;
		shield = remainingShield;
	}
}