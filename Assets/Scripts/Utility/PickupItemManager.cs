using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemManager : MonoBehaviour
{

    void RenderPickupItem(PickupItem item)
    {
        switch (item.itemType)
        {
            case PlayerInventory.PickupItemType.MachineGun:
                {
                    var gun = (GameObject)Instantiate(Resources.Load("MachineGun"), item.transform.position, Quaternion.identity, item.transform);
                    var scripts = gun.GetComponents<MonoBehaviour>();
                    foreach (var s in scripts)
                    {
                        s.enabled = false;
                    }
                    break;
                }
            case PlayerInventory.PickupItemType.MachineGunBullets:
                {
                    var bullets = (GameObject)Instantiate(Resources.Load("MachineGunBullets"), item.transform.position, Quaternion.identity, item.transform);
                    var scripts = bullets.GetComponents<MonoBehaviour>();
                    foreach (var s in scripts)
                    {
                        s.enabled = false;
                    }
                    break;
                }
        }
    }

    void Awake()
    {

        SpawnPickupItemInFrontOfPlayer(PlayerInventory.PickupItemType.MachineGun);
        SpawnPickupItemFurtherInFrontOfPlayer(PlayerInventory.PickupItemType.MachineGunBullets);
    }

    public void SpawnPickupItemInFrontOfPlayer(PlayerInventory.PickupItemType itemType)
    {
        var pos = Camera.main.transform.position + Camera.main.transform.forward * 5.0f;
        float posy = Terrain.activeTerrain.SampleHeight(new Vector3(pos.x, 0, pos.z));
        var pickup = (GameObject)Instantiate(Resources.Load("PickupItem"), new Vector3(pos.x, posy + 0.5f, pos.z), Quaternion.identity);
        var pickupItem = pickup.GetComponent<PickupItem>();
        pickupItem.itemType = itemType;
        RenderPickupItem(pickupItem);
    }

	public void SpawnPickupItemFurtherInFrontOfPlayer(PlayerInventory.PickupItemType itemType)
    {
        var pos = Camera.main.transform.position + Camera.main.transform.forward * 10.0f;
        float posy = Terrain.activeTerrain.SampleHeight(new Vector3(pos.x, 0, pos.z));
        var pickup = (GameObject)Instantiate(Resources.Load("PickupItem"), new Vector3(pos.x, posy + 0.5f, pos.z), Quaternion.identity);
        var pickupItem = pickup.GetComponent<PickupItem>();
        pickupItem.itemType = itemType;
        RenderPickupItem(pickupItem);
    }
}
