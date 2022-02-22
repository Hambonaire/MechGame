using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Waypoint : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI distanceText;

    [HideInInspector]
    public Camera refCamera;
    [HideInInspector]
    public Transform target;

    bool disabled = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize(Camera cam, Transform target)
    {
        refCamera = cam;
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            var minX = image.GetPixelAdjustedRect().width / 2;
            var maxX = Screen.width - minX;

            var minY = image.GetPixelAdjustedRect().height / 2;
            var maxY = Screen.height - minY;

            Vector2 pos = refCamera.WorldToScreenPoint(target.position);

            if (Vector3.Dot((target.position - refCamera.transform.position), refCamera.transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                    pos.x = minX;
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            image.transform.position = pos;

            distanceText.text = Mathf.Round(Vector3.Distance(refCamera.transform.position, target.position)) + "m";
        }
    }

    public void Disable()
    {
        disabled = true;

        image.gameObject.SetActive(false);
    }
}
