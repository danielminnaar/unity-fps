using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

	public PlayerInventory.PickupItemType itemType;
	public float rotateSpeed = 90.0f;
	void Update() {
		transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
