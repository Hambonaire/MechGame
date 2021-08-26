using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 *  HitRegister registers hits from bullets/lasers 
 *  - Tries to find the Weapon or Section it is attached to,
 *    gets the Section and tells it to take damage
 */
public class HitRegister : MonoBehaviour
{
    Collider[] hitBoxes;

    Section section;
    Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        hitBoxes = GetComponents<Collider>();

        section = GetComponent<Section>();
        weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RegisterHit(float damage)
    {
        if (weapon != null)
        {
            weapon.sectionParent.TakeDamage(damage);

            return true;
        }
        else if (section != null)
        {
            section.TakeDamage(damage);

            return true;
        }

        return false;
    }
}
