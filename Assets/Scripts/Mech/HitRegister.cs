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
    WeaponExecutable weapon;

    // Start is called before the first frame update
    void Start()
    {
        hitBoxes = GetComponents<Collider>();

        section = GetComponent<Section>();
        weapon = GetComponent<WeaponExecutable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RegisterHit(float damage, MechManager source)
    {
        if (weapon != null)
        {
            weapon.sectionParent.TakeDamage(damage, source);

            return true;
        }
        else if (section != null)
        {
            section.TakeDamage(damage, source);

            return true;
        }

        return false;
    }
}
