using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    private PlayerMotor motor;
    private Rigidbody rigidBody;
    private DamageableThing dmg;
    private RaycastShoot weaponRaycast;
    private PlayerInventory inventory;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
        dmg = GetComponent<DamageableThing>();
        dmg.TookDamage += new DamageableThing.TookDamageEventHandler(TookDamage);
        inventory = GetComponent<PlayerInventory>();
        inventory.WeaponEquipped += new PlayerInventory.WeaponEquippedEventHandler(WeaponEquipped);
        inventory.AmmoAdded +=  new PlayerInventory.AmmoAddedEventHandler(AmmoAdded);
        EquipDefaultGun();
        UpdateHealth();
        UpdateAmmo();
    }

    private void EquipDefaultGun()
    {
        //inventory.AddWeapon(WeaponManager.WeaponTypes.RailGun, -1);
        // inventory.AddWeapon(WeaponManager.WeaponTypes.MachineGun, -1);
        // inventory.AddAmmo(WeaponManager.WeaponTypes.MachineGun, 50);
        // inventory.AddWeapon(WeaponManager.WeaponTypes.RailGun, -1);
        // inventory.AddAmmo(WeaponManager.WeaponTypes.RailGun, 10);
    }

    private void TookDamage(float damage, float currentHealth)
    {
        UpdateHealth();
        print("player damage " + damage.ToString() + ", now " + currentHealth.ToString());
        var flashObj = GameObject.Find("PlayerDamageFlash");
        if (flashObj)
        {
            var flashImg = flashObj.GetComponent<UnityEngine.UI.Image>();
            flashImg.color = new Color(200, 0, 0, 255.0f);
            if (currentHealth > 0)
            {
                flashImg.CrossFadeAlpha(0.0f, 0.3f, false);
                flashImg.canvasRenderer.SetAlpha(1f); //reset
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
    }

    void WeaponEquipped(WeaponManager.ItemTypes weapon, int slot)
    {
        var cam = GameObject.Find("PlayerCamera");
        if (cam != null)
        {
            var existingWeapon = GameObject.Find("CurrentWeapon");
            if (existingWeapon != null)
                Destroy(existingWeapon);

            GameObject gun = null;
            switch (weapon)
            {
                case WeaponManager.ItemTypes.MachineGun:
                    {
                        gun = (GameObject)Instantiate(Resources.Load("MachineGun"), cam.transform, false);
                        break;
                    }
                case WeaponManager.ItemTypes.RailGun:
                    {
                        gun = (GameObject)Instantiate(Resources.Load("RailGun"), cam.transform, false);
                        break;
                    }
            }
            if (gun != null)
            {
                gun.name = "CurrentWeapon";
                gun.transform.localPosition = new Vector3(0.2f, -0.171f, 0.212f);
                gun.transform.localEulerAngles = new Vector3(0, 91.0f, 0);
                weaponRaycast = gun.GetComponent<RaycastShoot>();
                weaponRaycast.ShotFired += new RaycastShoot.ShotFiredEventHandler(ShotFired);
            }
            UpdateAmmo();
        }
    }

    public void ShotFired() {
        UpdateAmmo();
    }
    // Update is called once per frame
    void Update()
    {
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        //Apply camera rotation
        motor.RotateCamera(_cameraRotationX);

       

    }

    void OnTriggerEnter(Collider other)
    {
        var pickupItem = other.GetComponentInParent<PickupItem>();
        if(pickupItem != null)
            inventory.ItemPickupTrigger(pickupItem.itemType, pickupItem.gameObject);
    }

    private void UpdateAmmo()
    {
        var ammoText = GameObject.Find("AmmoText");
        if (ammoText)
        {
           
            ammoText.SetActive(true);
            var txt = ammoText.GetComponent<UnityEngine.UI.Text>();
            if (inventory.currentWeapon == WeaponManager.ItemTypes.None) {
                 txt.text = "";
                 return;
            }
            print("querying ammo");
            txt.text = "AMMO: " + inventory.GetAmmo().ToString();

        }
    }

    private void AmmoAdded(WeaponManager.ItemTypes weaponType) {
        UpdateAmmo();
    }

    private void UpdateHealth()
    {
        var healthText = GameObject.Find("HealthText");
        if (healthText)
        {
            var txt = healthText.GetComponent<UnityEngine.UI.Text>();
            var dmgThing = GetComponent<DamageableThing>();
            if (dmgThing)
                txt.text = "HP: " + System.Math.Round(dmgThing.currentHealth, 0);

        }
    }
}
