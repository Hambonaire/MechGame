using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button ContinueButton;
    public Button RestartButton;
    public Button QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        ContinueButton.onClick.AddListener(LevelUIManager._instance.PauseToggle);
        RestartButton.onClick.AddListener(LevelUIManager._instance.Restart);
        QuitButton.onClick.AddListener(LevelUIManager._instance.Quit);
    }
}
