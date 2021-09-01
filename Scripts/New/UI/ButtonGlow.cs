using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGlow : MonoBehaviour
{
    public GameObject glow;

    // Start is called before the first frame update
    void Start()
    {
        glow = transform.Find("Glow").gameObject;
    }

    public void SetGlowActive(bool val)
    {
        glow.SetActive(val);
    }
}
