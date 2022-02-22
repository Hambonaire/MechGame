using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarConfirmSubsectionButton : MonoBehaviour
{
    public void OnPress()
    {
        HangarManager._instance.ConfirmSection();
    }
}
