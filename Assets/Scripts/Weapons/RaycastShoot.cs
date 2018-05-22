using UnityEngine;

using System.Collections;

public class RaycastShoot : MonoBehaviour
{

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.1f);
    private AudioSource gunAudio;
    //private LineRenderer laserLine;
    private WeaponProperties weapon;
    private PlayerInventory inventory;
    private float nextFire;
    private Animator anim;
    public delegate void ShotFiredEventHandler();
    public event ShotFiredEventHandler ShotFired;
    void Start()
    {
        //laserLine = GetComponent<LineRenderer>();
        //gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        weapon = GetComponent<WeaponProperties>();
        inventory = GetComponentInParent<PlayerInventory>();
        anim = GetComponentInChildren<Animator>();
        print("player inventory is " + inventory);
    }


    void Update()
    {
 

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            
            // check ammo first
            
            if (inventory.UseAmmo(1))
            {
                nextFire = Time.time + weapon.fireRate;

                StartCoroutine(ShotEffect());

                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                RaycastHit hit;

                //laserLine.SetPosition (0, weapon.gunEnd.position);
                if (weapon.gunFireParticle)
                    Instantiate(weapon.gunFireParticle, weapon.gunEnd.position, Quaternion.identity);

                if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weapon.weaponRange))
                {
                    if (weapon.targetShotParticle)
                        Instantiate(weapon.targetShotParticle, hit.point, Quaternion.identity);
                    //laserLine.SetPosition (1, hit.point);
                    DamageableThing health = hit.collider.GetComponent<DamageableThing>();
                    if (health != null)
                    {
                        health.Damage(weapon.gunDamage);
                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(-hit.normal * weapon.hitForce);
                        }
                    }
                    var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = hit.point;
                    //var newSphere = (GameObject)Instantiate(Resources.Load("SphereCol"), , Quaternion.Euler(0,0,0));
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Destroy(sphere.GetComponent<Collider>());
                    Destroy(sphere, 0.1f);
                }
                else
                {
                    //laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weapon.weaponRange));
                }
                if (ShotFired != null)
                    ShotFired();
            }
        }
      
    }


    private IEnumerator ShotEffect()
    {
        //gunAudio.Play ();

        //laserLine.enabled = true;
        if(anim != null)
            anim.Play("MachineGun_Fire");
        yield return shotDuration;

        //laserLine.enabled = false;
    }
}