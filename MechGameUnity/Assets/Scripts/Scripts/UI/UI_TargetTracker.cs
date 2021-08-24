using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_TargetTracker : MonoBehaviour
{
    public Image reticle;
    //public GameObject target;
    public GameObject playerMech;

    public MechManager mechManager;

    //public ScriptableBool autoTarget;
    public GameObject playerTarget;
    //public ScriptableVector3 playerTargetTransform;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerTarget = mechManager.playerTarget;

        if (playerTarget != null)// && autoTarget.value)
        {
            reticle.enabled = true;

            Vector2 space = Camera.main.WorldToScreenPoint(playerTarget.GetComponent<EnemyController>().TorsoRoot.transform.position);

            reticle.transform.position = space;
        }
        else
        {
            reticle.enabled = false;
        }
    }

}
