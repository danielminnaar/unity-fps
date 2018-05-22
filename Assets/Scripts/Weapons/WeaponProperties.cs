using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProperties : MonoBehaviour {

	// Use this for initialization
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;   
	public Transform gunEnd;
    public GameObject gunFireParticle;
    public GameObject targetShotParticle;
	public GameObject weaponModel;
	public int ammo = 50;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
