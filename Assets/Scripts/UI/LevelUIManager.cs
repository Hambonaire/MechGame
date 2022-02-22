using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager _instance;

    public GameObject levelEndMenu;

    public GameObject pauseMenu;
    bool paused;

    // Post Processing 
    public Volume globalPPVolume;

    // Player specific UI
    public GameObject playerVCamPrefab;
    public GameObject playerHUDPrefab;
    List<HUDManager> playerHUDs = new List<HUDManager>();

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ShowLevelEndMenu();
        }
    }

    void ShowUISetup(bool val)
    {
        GameManager._instance.SetTimeScale(val ? 1 : 0);
        InputManager._instance.ToggleInput(!val);

        Cursor.lockState = val ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = val;
    }

    // Pause Menuing functions
    public void PauseToggle()
    {
        paused = !paused;

        pauseMenu.SetActive(paused);

        ShowUISetup(paused);
    }

    public void Restart()
    {
        GameManager._instance.SetTimeScale(1);

        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    // End Level Menuing functions
    public void ShowLevelEndMenu()
    {
        ShowUISetup(true);

        levelEndMenu.SetActive(true);
    }

    public void EndLevelGoToStats()
    {
        Restart();
    }


    // Game UI setup functions
    public void AttachUIElementsToPlayer(PlayerController player)
    {
        // Spawn a HUD Canvas & Camera
        CinemachineVirtualCamera vCam = Instantiate(playerVCamPrefab).GetComponent<CinemachineVirtualCamera>();
        HUDManager playerHUD = Instantiate(playerHUDPrefab).GetComponent<HUDManager>();

        player.AttachCamera(Camera.main, vCam, playerHUD);

        playerHUD.globalVolume = globalPPVolume;
        playerHUD.vCam = vCam;

        //playerHUD.Initialize();

        playerHUDs.Add(playerHUD);
    }

    public void InitObjectiveHUDs(Objective objective)
    {
        foreach (HUDManager manager in playerHUDs)
            manager.InitObjective(objective);
    }
}
