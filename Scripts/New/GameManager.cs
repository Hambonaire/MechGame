using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static GameManager _instance;

    public MechBase testMechBase;

    /* List of mechs out of storage in the hangar */
    //[HideInInspector]
    public List<Mech> availableMechs = new List<Mech>();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //Mech testMech = new Mech(testMechBase);

        //availableMechs.Add(testMech);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
