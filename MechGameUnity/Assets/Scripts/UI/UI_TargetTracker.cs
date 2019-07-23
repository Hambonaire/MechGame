using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_TargetTracker : MonoBehaviour
{
    public Image reticle;
    //public GameObject target;
    public GameObject playerMech;

    public ScriptableGameObject playerTarget;
    public ScriptableVector3 playerTargetTransform;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTarget.value != null)
        {
            //Debug.Log("Tracking");

            reticle.enabled = true;

            Vector2 space = Camera.main.WorldToScreenPoint(playerTargetTransform.value);

            reticle.transform.position = space;
        }

        if (playerTarget.value == null)
        {
            //Debug.Log("Searching");

            reticle.enabled = false;
        }
    }

}
