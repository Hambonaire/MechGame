using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public int selectedMechIndex;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimeScale(float val)
    {
        Time.timeScale = val;
    }
}
