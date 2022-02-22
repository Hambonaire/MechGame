using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using cakeslice;

public class OutlineHandler : MonoBehaviour
{
    Outline[] childOutlines;

    static Color[] colors = {new Color(1, 1, 1, 1), new Color(0, 1, 0, 1), new Color(1, 0, 0, 1)};

    public void Initialize(Faction faction)
    {
        childOutlines = GetComponentsInChildren<Outline>();

        for (int i = 0; i < childOutlines.Length; i++)
        {
            childOutlines[i].OutlineColor = colors[(int)faction];
            //childOutlines[i].OutlineWidth = 6;
        }

        EnableOutlines(false);
    }

    public void EnableOutlines(bool val)
    {
        for (int i = 0; i < childOutlines.Length; i++)
        {
            childOutlines[i].enabled = val;
        }
    }
}
