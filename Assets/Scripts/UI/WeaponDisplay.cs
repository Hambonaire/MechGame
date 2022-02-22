using System.Collections;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;

public class WeaponDisplay : MonoBehaviour
{
    [HideInInspector]
    public WeaponExecutable refExec;

    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ammoCounter;
    public Image heatBar;

    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f)
        {
            UpdateDisplay();
            timer = 0;
        }
    }

    void UpdateDisplay()
    {
        if (refExec == null || !refExec.gameObject.activeInHierarchy)
            return;

        switch (refExec.weaponItemRef.ammoType)
        {
            case AmmoType.Ballistic:
                ammoCounter.text = refExec.ammoCurr.ToString();
                break;
            case AmmoType.Energy:
                heatBar.fillAmount = 0.2f;

                break;
            case AmmoType.Missile:
                break;
            default:
                // Grey out
                break;
        }
    }
}
