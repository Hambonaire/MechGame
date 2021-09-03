using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrimedSceneButton : MonoBehaviour
{
    public void LoadPrimedScene()
    {
        SceneLoader._instance.LoadPrimedlevel();
    }
}
