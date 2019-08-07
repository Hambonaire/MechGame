using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_inGame : MonoBehaviour
{
    public Controller_Player controller_player;
    
    int primaryAmmo;
    int secondaryAmmo;

    public ScriptableFloat playerRightFillBar;
    public ScriptableFloat playerLeftFillBar;
    public ScriptableFloat playerRightAmmoCurrent;
    public ScriptableFloat playerLeftAmmoCurrent;
    public ScriptableFloat playerRightAmmoMax;
    public ScriptableFloat playerLeftAmmoMax;
    public ScriptableBool playerRightReloading;
    public ScriptableBool playerLeftReloading;
    public ScriptableFloat playerHealthCurrent;
    public ScriptableFloat playerHealthMax;

    public GameObject levelEndUI;
    public GameObject pauseUI;
    public GameObject playerHUD;

    //public Text primaryAmmoText;
    //public Text secondaryAmmoText;
    public AmmoTextUI[5] ammoLR = new AmmoTextUI[];
    public AmmoTextUI[5] ammoLU = new AmmoTextUI[];
    public AmmoTextUI[5] ammoLS = new AmmoTextUI[];
    public AmmoTextUI[5] ammoRR = new AmmoTextUI[];
    public AmmoTextUI[5] ammoRU = new AmmoTextUI[];
    public AmmoTextUI[5] ammoRS = new AmmoTextUI[];
    
    public List<List<AmmoTextUI[]>> ammoTexts = new List<List<AmmoTextUI[]>>() {
        new List<AmmoTextUI[]>(),
        new List<AmmoTextUI[]>()
    };

    public Image primaryChargeBar;
    public Image secondaryChargeBar;

    public Image cautionPrimary;
    public Image cautionSecondary;

    public Image mainReticle;

    public Image healthBar;

    public GameObject playerObject;

    float myTime = 0;

    Color cautionColor;
    //Color alphaWave;

    void Awake ()
    {
        ammoTexts[0][0] = ammoLR;
        ammoTexts[0][1] = ammoLR;
        ammoTexts[0][2] = ammoLR;
        ammoTexts[1][0] = ammoLR;
        ammoTexts[1][1] = ammoLR;
        ammoTexts[1][2] = ammoLR;
    }

    void Start()
    {
        /*
        primaryChargeBar.fillMethod = Image.FillMethod.Radial180;
        primaryChargeBar.fillOrigin = (int)Image.Origin180.Bottom;
        primaryChargeBar.fillClockwise = true;
        
        secondaryChargeBar.fillMethod = Image.FillMethod.Radial180;
        secondaryChargeBar.fillOrigin = (int)Image.Origin180.Top;
        secondaryChargeBar.fillClockwise = false;
        */
        // Get main reticle from weapon (thru variables..?)

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        levelEndUI.SetActive(false);
        pauseUI.SetActive(false);
        playerHUD.SetActive(true);

        cautionColor = cautionPrimary.color;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myTime += Time.deltaTime;

        if (myTime > 10) myTime = 0;
        const float frequency = .8f; // Frequency in Hz
        float amplitude = 0.5f * (1 + Mathf.Sin(2 * Mathf.PI * frequency * myTime));

        cautionColor.a = amplitude;

        //Debug.Log("Fill amount: " + Variables.PlayerPrimaryChargeVal);

        //primaryChargeBar.fillAmount = playerRightFillBar.value;

        //secondaryChargeBar.fillAmount = playerLeftFillBar.value;

        //Debug.Log(cautionPrimary.color.a);

        UpdateAmmo();

/*
        if (playerRightReloading.value)
        {
            primaryAmmoText.text = "RELOADING    " + playerRightAmmoCurrent.value + " / " + playerRightAmmoMax.value;

            cautionPrimary.enabled = true;
            cautionPrimary.color = cautionColor;
        }
        else
        {
            primaryAmmoText.text =playerRightAmmoCurrent.value + " / " + playerRightAmmoMax.value;

            cautionPrimary.enabled = false;
        }

        if (playerLeftReloading.value)
        {
            secondaryAmmoText.text = playerLeftAmmoCurrent.value + " / " + playerLeftAmmoMax.value + "    RELOADING";

            cautionSecondary.enabled = true;
            cautionSecondary.color = cautionColor;
        }
        else
        {
            secondaryAmmoText.text = playerLeftAmmoCurrent.value + " / " + playerLeftAmmoMax.value;

            cautionSecondary.enabled = false;
        }
       */
    
        healthBar.fillAmount = playerHealthCurrent.value / playerHealthMax.value;
        //Debug.Log(Variables.PlayerHealth_Curr + " OF " + Variables.PlayerHealth_Max);
    }

    public void OnLevelEnd()
    {
        playerHUD.SetActive(false);

        levelEndUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TogglePause()
    {
        if (!levelEndUI.activeSelf)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            playerHUD.SetActive(!pauseUI.activeSelf);

            if (pauseUI.activeSelf)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseUI.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void Options()
    {

    }
    
    public void Rebuild()
    {    
        /*
        for (int i = 0; i < controller_player.weapon_data.Count; i++) {
            for (int j = 0; i < controller_player.weapon_data[i].Count; j++) {
                ammoTexts[i][j].Clear();
                for (int k = 0; i < controller_player.weapon_data.[i][j]Count; k++) {
                    ammoTexts[i][j][k].max = controller_player.weapon_data[i][j][k].executable.maxAmmo;
                    ammoTexts[i][j][k].current = ammoTexts[i][j][k].max;
                    ammoTextx[i][j][k].icon = EquipmentManager.Instance.currentWeapons[i][j][k].ammoIcon.
                }
            }
        }
        */
        
        for (int i = 0; i < controller_player.weapon_data.Count; i++) {
            for (int j = 0; i < controller_player.weapon_data[i].Count; j++) {
                for (int k = 0; i < controller_player.weapon_data.[i][j]Count; k++) {
                    ammoTexts[i][j][k].data_ref = controller_player.weapon_data[i][j][k];
                    // Do other stuff inside ammoTexts script itself with ref?
                }
            }
        }
        
    }
    
    // Each thing should take care of this on their own, dont use this... hopefully
    
    /*
    public void UpdateAmmo ()
    {
        //foreach () // ?
        //ammoSlots[] = GetComponentsInChildren<AmmoUISlot>() //??
        
        for (int i = 0; i < controller_player.weapon_data.Count; i++) {
            for (int j = 0; i < controller_player.weapon_data[i].Count; j++) {
                for (int k = 0; i < controller_player.weapon_data.[i][j]Count; k++) {
                    if (controller_player.weapon_data[i][j][k].executable.isReloading) {
                        ammoTexts[i][j][k].reloadImage.enabled = true; // Do stuff here for showing reloading is happening
                    }
                    else {
                        ammoTextx[i][j][k].current = controller_player.weapon_data[i][j][k].executable.currentAmmo;
                    }
                }
            }
        }
        
        
        for (int i = 0; i < ammoTexts.Count; i++) {
            for (int j = 0; j < ammoTexts[i].Count; j++) {
                for (int k = 0; k < ammoTexts[i][j].Length; k++) {
                    if (ammoTexts[i][j][k].data_ref.isReloading) {
                        ammoTexts[i][j][k].reloadImage.enabled = true;
                    } 
                    else {
                        ammotTexts[i][j][k].current = ammoTexts[i][j][k].data_ref.currentAmmo;
                    }
                }
            }
        }
        
    }
    */
}
 
