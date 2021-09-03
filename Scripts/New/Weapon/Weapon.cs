using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  Weapon to hold some weapon references
 *  - SectionParent for hitRegister to tell it to take damage when Register's hitboxes are hit
 *
 */
[RequireComponent(typeof(HitRegister))]
[RequireComponent(typeof(WeaponExecutable))]
public class Weapon : MonoBehaviour
{
    /* Should be set automatically by mechBuilder */
    [HideInInspector]
    public Section sectionParent;

    [HideInInspector]
    public WeaponExecutable myExecutable;
	
	public Transform[] bulSpwnLoc;
	
	public Animator wepAnim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
