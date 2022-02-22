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
        if (LevelManager._instance.playerMechManager.weaponSystem.target == null)
        {
            reticle.gameObject.SetActive(false);
        }
        else
        {
            //{
            MechManager mechTarget = LevelManager._instance.playerMechManager.weaponSystem.target;

            reticle.gameObject.SetActive(true);

            //    Vector2 space = Camera.main.WorldToScreenPoint(mechTarget.weaponSystem.targetCenter.position);

            //    reticle.transform.position = space;

            //    if (Vector3.Dot((mechTarget.transform.position - transform.position), transform.forward) < 0)
            //    {
            //        if (pos.x < Screen.width / 2)
            //        {
            //            pos.x = maxX;
            //        }
            //        else
            //            pos.x = minX;
            //    }
            //}
            //else
            //{
            //    reticle.enabled = false;
            //}

            var minX = reticle.GetPixelAdjustedRect().width / 2;
            var maxX = Screen.width - minX;

            var minY = reticle.GetPixelAdjustedRect().height / 2;
            var maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(mechTarget.GetTargetCenter().position);

            if (Vector3.Dot((mechTarget.GetTargetCenter().position - reticle.transform.position), reticle.transform.forward) < 0)
            {
                reticle.gameObject.SetActive(false);

                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                    pos.x = minX;
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            reticle.transform.position = pos;

            //distanceText.text = Mathf.Round(Vector3.Distance(transform.position, target.position)) + "m";
        }
    }

}
