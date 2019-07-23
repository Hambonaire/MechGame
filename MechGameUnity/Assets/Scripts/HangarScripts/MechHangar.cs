using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHangar : MonoBehaviour
{
    public Stats playerStats;

    public GameObject torsoRoot;
    public GameObject legsRoot;
    //public CharacterController characterController;

    float overallScaleFactor = 1;

    Animator legsAnimator;
    Animator rightWeaponAnimator;
    Animator leftWeaponAnimator;

    public Legs legsItem;
    public Cockpit cockpitItem;
    public Weapon rightWeaponItem;
    public Weapon leftWeaponItem;

    GameObject legs;
    GameObject rightArmWeapon;
    GameObject leftArmWeapon;
    GameObject cockpit;

    Transform legsBase;
    Transform cockpitRotationCenter;
    Transform torsoConnection;
    Transform rightArmBarrel;
    Transform leftArmBarrel;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {

    }

    public void Initialize()
    {
        #region Beforehand Calculations
        overallScaleFactor = cockpitItem.scaleFactor;
        #endregion

        #region Remove Current Items
        if (cockpit != null)
        {
            Destroy(cockpit);
        }
        if (legs != null)
        {
            Destroy(legs);
        }
        if (rightArmWeapon != null)
        {
            Destroy(rightArmWeapon);
        }
        if (leftArmWeapon != null)
        {
            Destroy(leftArmWeapon);
        }
        #endregion

        #region Part Variables
        legs = Instantiate(legsItem.prefab, legsRoot.transform.position, transform.rotation);
        legs.transform.parent = legsRoot.transform;
        // Scaling
        legs.transform.localScale = new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);

        torsoRoot.transform.position = legs.transform.Find("TorsoConnection").position;

        cockpit = Instantiate(cockpitItem.prefab, torsoRoot.transform.position, torsoRoot.transform.rotation);
        cockpit.transform.parent = torsoRoot.transform;
        // Scaling
        //cockpit.transform.localScale = new Vector3(torsoScaleFactor, torsoScaleFactor, torsoScaleFactor);

        cockpitRotationCenter = cockpit.transform.Find("RotationAxis");
        #endregion

        #region Weapon Variables
        if (rightWeaponItem.rightPrefab != null)
        {
            rightArmWeapon = Instantiate(rightWeaponItem.rightPrefab, cockpit.transform.Find("RightArmConnection").position, cockpit.transform.rotation) as GameObject;
            rightArmWeapon.transform.parent = cockpitRotationCenter.transform;
        }

        if (leftWeaponItem.leftPrefab != null)
        {
            leftArmWeapon = Instantiate(leftWeaponItem.leftPrefab, cockpit.transform.Find("LeftArmConnection").position, cockpit.transform.rotation) as GameObject;
            leftArmWeapon.transform.parent = cockpitRotationCenter.transform;
        }
        #endregion

        #region Animators
        if (legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (rightArmWeapon.transform.Find("AnimatorHolder") != null && rightArmWeapon != null)
        {
            rightWeaponAnimator = rightArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (leftArmWeapon.transform.Find("AnimatorHolder") != null && leftArmWeapon != null)
        {
            leftWeaponAnimator = leftArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }
        #endregion

        cockpit.transform.localScale = new Vector3(overallScaleFactor, overallScaleFactor, overallScaleFactor);

        //Debug.Log(Variables.PlayerHealth_Curr);
    }

    public void SetNewWeapons(Weapon newLeft, Weapon newRight)
    {
        this.leftWeaponItem = newLeft;
        this.rightWeaponItem = newRight;

        Initialize();
    }

    public void SetNewCockpit(Cockpit newCockpit)
    {
        this.cockpitItem = newCockpit;

        Initialize();
    }

    public void SetNewLegs(Legs newLegs)
    {
        this.legsItem = newLegs;

        Initialize();
    }

    public float GetScaleFactor()
    {
        return overallScaleFactor;
    }

    public void ToggleArms()
    {
        rightArmWeapon.SetActive(!rightArmWeapon.activeSelf);
        leftArmWeapon.SetActive(!leftArmWeapon.activeSelf);
    }
}
