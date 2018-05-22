using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetect : MonoBehaviour
{

    private Camera fpsCam;
    private float viewRange = 50;
    private TextMesh healthText;
    void Start()
    {
        fpsCam = GetComponentInParent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        var x = GameObject.Find("HealthObj");
        if (x)
            Destroy(x.gameObject);
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, viewRange))
        {
            // determine if shootable to draw health text
            DamageableThing health = hit.collider.GetComponent<DamageableThing>();
            if (health != null)
            {
                var hObj = new GameObject("HealthObj");
                hObj.AddComponent<TextMesh>();
                TextMesh textMeshComponent = hObj.GetComponent(typeof(TextMesh)) as TextMesh;
                textMeshComponent.text = System.Math.Round(health.currentHealth, 0).ToString() + "/" + System.Math.Round(health.totalHealth,0).ToString();
                hObj.transform.parent = hit.collider.transform;
                hObj.transform.position = hit.transform.position;
                hObj.transform.LookAt(2 * hObj.transform.position - fpsCam.transform.position);

            }
        }
    }
}
