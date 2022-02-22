using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float explosionRadius = 0;

    Vector3 prevPos;

    protected MechManager mySource;

    void Start()
    {
        prevPos = transform.position;
    }

    public virtual void Initialize(MechManager source, WeaponItem refItem, Transform target, Vector3 firingSolution, Vector3 spread)
    {
        mySource = source;

        damage = refItem.damage;

        if (firingSolution != Vector3.zero)
            transform.LookAt(firingSolution);

        transform.rotation *= Quaternion.Euler(spread);

        GetComponent<Rigidbody>().velocity = transform.forward * refItem.bulletSpeed;

        Destroy(gameObject, refItem.bulletLife);
    }

    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        if (hits.Length > 0) 
        {
            var hit = hits[0].collider.gameObject.GetComponent<HitRegister>();

            if (hit != null)
            {
                hit.RegisterHit(damage, mySource);
                if (mySource.controllerHUD != null)
                    mySource.controllerHUD.HitMarker();
            }

            Destroy(gameObject);
        }

        prevPos = transform.position;
    }
}
