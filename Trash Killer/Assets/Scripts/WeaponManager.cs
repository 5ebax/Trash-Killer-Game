using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public GameObject[] weapons;
	public int actualWeapon;
	[System.NonSerialized]
	public string specialWeapon;
	[System.NonSerialized]
	public int numDeads;
	private Quaternion rotation;
	private PlayerController pjCont;

    void Awake()
    {
		
    }

    // ==========================
    void Start()
	{
		pjCont = GetComponent<PlayerController>();
		numDeads = 0;
		this.ChangeWeapon();
	}

	// ==========================
	void Update()
	{
		if (pjCont.EnemyDeaths >= numDeads && actualWeapon >= 5)
			actualWeapon = 0;

		rotation = weapons[actualWeapon].transform.rotation;
		if (Input.GetKeyDown(KeyCode.Alpha1) && actualWeapon < 5)
		{
			this.actualWeapon = 0;
			this.ChangeWeapon();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2) && actualWeapon < 5)
		{
			this.actualWeapon = 1;
			this.ChangeWeapon();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3) && actualWeapon < 5)
		{
			this.actualWeapon = 2;
			this.ChangeWeapon();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4) && actualWeapon < 5)
		{
			this.actualWeapon = 3;
			this.ChangeWeapon();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5) && actualWeapon < 5)
		{
			this.actualWeapon = 4;
			this.ChangeWeapon();
		}
		else if (Input.GetButtonDown("Fire2") && actualWeapon < 5)
		{
			this.actualWeapon +=1;
			if(actualWeapon >= 5) { this.actualWeapon = 0; }
			this.ChangeWeapon();
		}
		else if(specialWeapon == "Lanzallamas")
		{
			this.actualWeapon = 5;
			this.ChangeWeapon();
		}
		else if (specialWeapon == "Hacha")
		{
			this.actualWeapon = 6;
			this.ChangeWeapon();
		}
		else if (specialWeapon == "Arpon")
		{
			this.actualWeapon = 7;
			this.ChangeWeapon();
		}
	}
	// ==========================
	void ChangeWeapon()
	{
		for (int i = 0; i < this.weapons.Length; i++)
		{
			if (i == this.actualWeapon)
			{
				weapons[actualWeapon].transform.rotation = rotation;
				this.weapons[i].SetActive(true);
			}
			else
			{
				this.weapons[i].SetActive(false);
			}
		}
	}
}
