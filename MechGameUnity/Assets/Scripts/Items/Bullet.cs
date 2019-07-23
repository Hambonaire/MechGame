using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float ballisticDam;
    float energyDam;

    public ParticleSystem hitEffect;
    
    public float explosionRadius = 0;

    Vector3 prevPos;

    void Start()
    {
        prevPos = transform.position;
    }

    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);

        if (hits.Length > 0) 
        {
            if (hits[0].collider.gameObject.tag == "Enemy" || hits[0].collider.gameObject.tag == "Player")
            {
                //Debug.Log("Deal Damage");

                hits[0].collider.gameObject.GetComponent<Stats>().TakeDamage(ballisticDam, energyDam);

                Destroy(this.gameObject);
            }
        }
        prevPos = transform.position;
    }

    /*
    void OnTriggerEnter(Collider col)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (col.gameObject.tag == "Enemy")
        {

            //Destroy(col.gameObject);

            //add an explosion or something
            //destroy the projectile that just caused the trigger collision

            // Look for Stats script in object and call takeDamage...? ----------------

            col.gameObject.GetComponent<Stats>().TakeDamage(ballisticDam, energyDam);

            Destroy(this.gameObject);
        }

    }
    */

    public void SetBallisticDamage(float inDam)
    {
        ballisticDam = inDam;
    }

    public void SetEnergyDamage(float inDam)
    {
        energyDam = inDam;
    }

    public void SetFaction(int inFaction)
    {
        if (inFaction == 0)
        {

        }
        else if (inFaction == 1)
        {

        }
    }
}
