using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunFire : MonoBehaviour {
	// Use this for initialization
	private RaycastShoot shooter;
	void Start () {
		shooter = GetComponent<RaycastShoot>();
		if(shooter) {
			shooter.ShotFired += new RaycastShoot.ShotFiredEventHandler(ShotFired);
		}
	}
	public void ShotFired() {
		var model = GetComponentInParent<WeaponProperties>().weaponModel;
		if(model) {
			model.transform.Rotate(1000*Time.deltaTime,0, 0);
		}
	}
}
