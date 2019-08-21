using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHangar : MonoBehaviour
{
    public struct WeaponMapStruct
    {
        public int a;
        public int b;
        public int c;
    }

    public CharacterController characterController;

    public Stats playerStats;

    public GameObject mechManager;

    private Vector3 inputDirection;

    public ScriptableFloat playerHealthCurrent;
    public ScriptableFloat playerHealthMax;
    public ScriptableGameObject playerTarget;
    [HideInInspector]
    public Vector3 enemyCenterMass;
    float targetDistance;
    float firingAngle;

    private Vector3 forward;

    [HideInInspector]
    public float gravity = -9.8f;
    [HideInInspector]
    public float walkSpeed;
    [HideInInspector]
    public float runSpeed;

    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0;
    private float targetSpeed = 0;
    private float movementSmoothVelocity = 0;
    private float movementSmoothTime = 0.1f;
    private bool movementEnabled = true;
    private bool running;

    private float turnSmoothVelocity_Torso = 0;
    private float turnSmoothTime_Torso = .025f;
    private float turnSmoothVelocity_Legs = 0;
    private float turnSmoothTime_Legs = .2f;

    public CapsuleCollider hitBox;
    [HideInInspector]
    public float baseHitBoxRadius = 1f;
    [HideInInspector]
    public float baseHitBoxHeight = 3f;
    [HideInInspector]
    public float baseHitBoxCenter = 1.55f;

    public GameObject cam;
    public GameObject torsoRoot;
    public GameObject legsRoot;

    //public Transform legsBase;
    [HideInInspector]
    public Transform cockpitRotationCenter;
    //[HideInInspector]
    //public Transform torsoConnection;

    [HideInInspector]
    public float overallScaleFactor = 1;

    Animator legsAnimator;              // TODO: Look into Cockpit bobbing thru transform movement, not animations? Think u can "animate" movement in Unity... check into that
    //Animator rightWeaponAnimator;
    //Animator leftWeaponAnimator;

    public List<List<List<Datatype_Weapon>>> weapon_data = new List<List<List<Datatype_Weapon>>>(){
        new List<List<Datatype_Weapon>>() {
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>()
        },
        new List<List<Datatype_Weapon>>() {
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>(),
            new List<Datatype_Weapon>()
        }
    };
    [HideInInspector]
    public GameObject cockpit;
    [HideInInspector]
    public GameObject legs;

    public List<WeaponMapStruct> reloadStructs = new List<WeaponMapStruct>();
    //public List<WeaponMapStruct> cooldownStructs = new List<WeaponMapStruct>();

    List<List<List<int>>> testmap0 = new List<List<List<int>>>() {
        new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } },
        new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } }
    };
    List<List<List<int>>> testmap1 = new List<List<List<int>>>() {
        new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } }, new List<List<int>> { new List<int> { 0 } },
        new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } }, new List<List<int>> { new List<int> { 1 } }
    };

    void Start()
    {
        //legsRoot = this.transform.Find("LegsRoot").gameObject;
        playerStats = GetComponent<Stats>();
        //Variables.PlayerHealth_Max = playerStats.GetMaxHealth();
    }

    public void Rebuild(Cockpit cockpit, Legs legs, List<List<List<Weapon>>> weps, List<Accessory> accs)
    {
        #region Remove Current Items

        if (cockpit != null)
        {
            Destroy(this.cockpit);
        }
        if (legs != null)
        {
            Destroy(this.legs);
        }
        for (int i = 0; i < weapon_data.Count; i++)
        {
            for (int j = 0; j < weapon_data[i].Count; j++)
            {
                for (int k = 0; k < weapon_data[i][j].Count; k++)
                {
                    weapon_data[i][j][k].Delete();
                    weapon_data[i][i].RemoveAt(k);
                }
            }
        }

        #endregion

        #region Create New Items

        this.legs = Instantiate(legs.prefab, legsRoot.transform.position, transform.rotation);
        this.legs.transform.parent = legsRoot.transform;
        this.legs.transform.localScale = Vector3.one * cockpit.scaleFactor; // new Vector3(cockpit.scaleFactor, cockpit.scaleFactor, cockpit.scaleFactor);

        torsoRoot.transform.position = this.legs.transform.Find("TorsoConnection").position;

        this.cockpit = Instantiate(cockpit.prefab, torsoRoot.transform.position, torsoRoot.transform.rotation);
        this.cockpit.transform.parent = torsoRoot.transform;

        cockpitRotationCenter = this.cockpit.transform.Find("RotationAxis");

        for (int i = 0; i < weps.Count; i++)
        {
            for (int j = 0; j < weps[i].Count; j++)
            {
                for (int k = 0; k < weps[i][j].Count; k++)
                {
                    Datatype_Weapon data = new Datatype_Weapon();
                    GameObject wepTemp = Instantiate(weps[i][j][k].prefab, this.cockpit.transform.Find("Connection_" + i + j + k).position, this.cockpit.transform.rotation) as GameObject;
                    wepTemp.transform.parent = cockpitRotationCenter.transform;

                    data.weapon_object = wepTemp;
                    data.executable = new WeaponExecutable(weps[i][j][k], data.weapon_object.transform.Find("Barrel"));
                    weapon_data[i][j].Add(data);
                }
            }
        }

        // TODO: Do UI passing stuff here? (ammo, reloading, etc.) Loop thru Executables or just plug in the existing loop^^^
        // TODO: Reminder: make sure stats references scriptableFloats, etc. for UI passing
        #endregion

        // TODO: All the animation stuff here, prob need to do loop stuff, Find(), etc. Calling the animators takes place in Fire & OnCooldown
        #region Animators
        /*
        if (this.legs.transform.Find("AnimatorHolder") != null)
        {
            legsAnimator = this.legs.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (rightArmWeapon.transform.Find("AnimatorHolder") != null && rightArmWeapon != null)
        {
            rightWeaponAnimator = rightArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }

        if (leftArmWeapon.transform.Find("AnimatorHolder") != null && leftArmWeapon != null)
        {
            leftWeaponAnimator = leftArmWeapon.transform.Find("AnimatorHolder").GetComponent<Animator>();
        }
		*/
        #endregion

        #region Misc.
        walkSpeed = legs.walkSpeed * legs.scaleFactor;
        runSpeed = legs.runSpeed * cockpit.scaleFactor;
        //hitBox.radius = baseHitBoxRadius * cockpit.scaleFactor;
        //hitBox.height = baseHitBoxHeight * cockpit.scaleFactor;
        //hitBox.center = new Vector3(0, hitBox.height / 2 + .05f, hitBox.center.z);
        #endregion

        this.cockpit.transform.localScale = Vector3.one * cockpit.scaleFactor; // new Vector3(currentCockpit.scaleFactor, currentCockpit.scaleFactor, currentCockpit.scaleFactor);

    }
}
