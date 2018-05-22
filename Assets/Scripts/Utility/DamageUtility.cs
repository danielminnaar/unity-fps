using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUtility : MonoBehaviour
{

    public static void AreaDamageThings(Vector3 location, float radius, float damage)
    {
        var objectsInRange = (Collider[])Physics.OverlapSphere(location, radius);
        foreach (Collider col in objectsInRange)
        {
            DamageableThing thing = col.GetComponent<DamageableThing>();
            if (thing != null)
            {
				var thingCollider = thing.GetComponent<Collider>();

                // test if thing is exposed to blast, or behind cover:
                RaycastHit hit;
                var exposed = false;
                Debug.DrawRay(location, (thingCollider.transform.position - location), Color.blue, 200);
                if (Physics.Raycast(location, (thingCollider.transform.position - location), out hit, radius, 1))
                {
                    exposed = (hit.collider == thingCollider);
                }

                if (exposed)
                {
                    // Damage Enemy! with a linear falloff of damage amount
                    float proximity = (location - thing.transform.position).magnitude;
                    float effect = 1 - (proximity / radius);
                    thing.Damage((damage * effect));
                }
            }
        }

    }
}
