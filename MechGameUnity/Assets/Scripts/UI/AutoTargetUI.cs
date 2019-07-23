using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetUI : MonoBehaviour
{
    public GameObject Obj;

    public Camera mCamera;
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Obj != null)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(mCamera, Obj.transform.position);
            rt.position = pos;
        }
    }
}
