using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum PickupItemType {
        MachineGun,
        RailGun,
        MachineGunBullets
    }
    public SortedDictionary<int, WeaponManager.ItemTypes> weapons = new SortedDictionary<int, WeaponManager.ItemTypes>();
    public Dictionary<WeaponManager.ItemTypes, int> ammo = new Dictionary<WeaponManager.ItemTypes, int>();
    public int maxWeapons = 3;
    public WeaponManager.ItemTypes currentWeapon;
    public delegate void WeaponEquippedEventHandler(WeaponManager.ItemTypes weapon, int slot);
    public event WeaponEquippedEventHandler WeaponEquipped;
    public delegate void AmmoAddedEventHandler(WeaponManager.ItemTypes weaponType);
    public event AmmoAddedEventHandler AmmoAdded;

    void Awake()
    {
        
        
    }

    public bool AddAmmo(WeaponManager.ItemTypes weaponType, int amount)
    {
        print("adding " + amount + " ammo for " +  weaponType);
        switch(weaponType) {
            case WeaponManager.ItemTypes.MachineGunBullets: {
                weaponType = WeaponManager.ItemTypes.MachineGun;
                break;
            }
        }
        if (ammo.ContainsKey(weaponType))
            ammo[weaponType] += amount;
        else
            ammo.Add(weaponType, amount);

        if(AmmoAdded != null)
            AmmoAdded(weaponType);
        return true;
    }

    public void ItemPickupTrigger(PickupItemType item, GameObject obj) {
        switch(item) {
            case PickupItemType.MachineGun: {
                AddWeapon(WeaponManager.ItemTypes.MachineGun, -1);
                AddAmmo(WeaponManager.ItemTypes.MachineGun, 50);
                print("Added ammo and weapon for  " + item);
                Destroy(obj);
                break;
            }  
            case PickupItemType.RailGun: {
                AddWeapon(WeaponManager.ItemTypes.RailGun, -1);
                AddAmmo(WeaponManager.ItemTypes.RailGun, 10);
                Destroy(obj);
                break;
            }  
            case PickupItemType.MachineGunBullets: {
                AddAmmo(WeaponManager.ItemTypes.MachineGunBullets, 100);
                Destroy(obj);
                break;
            }  
        }
    }

    public int GetAmmo() {
        if(ammo.ContainsKey(currentWeapon))
            return ammo[currentWeapon];
        else
            return 0;
            
    }

    public bool UseAmmo(int amount) {
        if(ammo.ContainsKey(currentWeapon) && ammo[currentWeapon] > 0) {
            ammo[currentWeapon]--;
            return true;
        }
        return false;
    }

    public bool AddWeapon(WeaponManager.ItemTypes weaponType, int slot = -1)
    {
        print("adding weapon " + weaponType.ToString());
        if (slot > 0 && (slot <= maxWeapons) && weapons.ContainsKey(slot))
        {
            weapons[slot] = weaponType;
        }
        else if (slot < 0 && weapons.Keys.Count < maxWeapons)
        {
            print("no slot specified, current weapons: " + weapons.Count.ToString());
            var lastKey = weapons.Keys.Count;
            weapons.Add(lastKey + 1, weaponType);
            slot = lastKey + 1;
        }

        if (weapons.Count == 1)
        {
            EquipWeapon(slot);
        }

        return false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.ContainsKey(1))
        {
            EquipWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.ContainsKey(2))
        {
            EquipWeapon(2);
        }
    }

    public void EquipWeapon(int slot)
    {
        if (slot <= maxWeapons)
        {
            print("equipping weapon " + weapons[slot].ToString());
            currentWeapon = (WeaponManager.ItemTypes)weapons[slot];
            if (WeaponEquipped != null)
                WeaponEquipped(currentWeapon, slot);
        }
    }
}
