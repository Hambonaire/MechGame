using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public MechBase testMechBase;

    /* List of mechs out of storage in the hangar */
    //[HideInInspector]
    public List<Mech> availableMechs = new List<Mech>();

    void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Mech testMech = new Mech(testMechBase);

        //availableMechs.Add(testMech);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
