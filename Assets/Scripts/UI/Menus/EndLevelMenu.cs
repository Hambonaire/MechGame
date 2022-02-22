using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelMenu : MonoBehaviour
{
    public Button successContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        successContinueButton.onClick.AddListener(LevelUIManager._instance.EndLevelGoToStats);
    }
}
