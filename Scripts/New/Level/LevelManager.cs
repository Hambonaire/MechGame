using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MechBuilder builder = new MechBuilder();

        GameObject playerMech = builder.BuildFromMechObj(GameManager._instance.availableMechs[0], Vector3.zero, true, true, false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
