using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float damage;
    
    public float explosionRadius = 0;

    Vector3 prevPos;

    void Start()
    {
        prevPos = transform.position;
    }

    public void Initialize(WeaponItem refItem)
    {
        damage = refItem.damage;

        Destroy(gameObject, refItem.bulletLife);
    }

    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        if (hits.Length > 0) 
        {
            //Debug.Log(hits[0].collider.gameObject.name);

            hits[0].collider.gameObject.GetComponent<HitRegister>()?.RegisterHit(damage);

            Destroy(gameObject);
        }

        prevPos = transform.position;
    }
}
