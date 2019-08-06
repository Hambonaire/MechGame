using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetUI : MonoBehaviour
{
    public ScriptableGameObject target;

    public Camera mCamera;
    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Obj != null)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(mCamera, target.transform.position);
            rt.position = pos;
        }
    }
}
