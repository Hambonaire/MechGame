using System;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    public Action FireTrigger;
    public Action FireReset;

    public void OnFireTrigger()
    {
        if (FireTrigger != null)
            FireTrigger();
    }

    public void OnFireReset()
    {
        if (FireReset != null)
            FireReset();
    }
}

