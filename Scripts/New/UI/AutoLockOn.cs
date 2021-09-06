using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoLockOn : MonoBehaviour
{
    public Image reticle;

    //MechManager mechManager;

    [SerializeField]
    GameObject playerTarget;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerTarget = LevelManager._instance.playerMechController.GetTarget();

        if (playerTarget != null)
        {
            reticle.enabled = true;

            Vector2 space = Camera.main.WorldToScreenPoint(playerTarget.transform.position);

            reticle.transform.position = space;
        }
        else
        {
            reticle.enabled = false;
        }
    }

}
