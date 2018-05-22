using UnityEngine;
using System.Collections;

public class DamageableThing : MonoBehaviour
{

    //The box's current health point total
    public float currentHealth = 100;
    public float totalHealth = 100;
    public GameObject explosionParticle;
    public delegate void TookDamageEventHandler(float damage, float currentHealth);
    public event TookDamageEventHandler TookDamage;
    public void Damage(float damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (gameObject.tag != "Player")
            {
                //if health has fallen below zero, deactivate it 
                gameObject.SetActive(false);
                if (explosionParticle)
                {
                    Instantiate(explosionParticle, this.transform.position, Quaternion.identity);
                    // var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // obj.name = "tester";
                    // obj.transform.localScale = new Vector3(10, 10, 10);
                    // obj.transform.position = gameObject.transform.position;
                    // print("radius is " + obj.GetComponent<SphereCollider>().radius.ToString());
                    DamageUtility.AreaDamageThings(gameObject.transform.position, 20, 70.0f);
                }

            }
            else {

                 UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name );
            }
        }

         if(TookDamage != null)
            TookDamage(damageAmount, currentHealth);
    }
}
