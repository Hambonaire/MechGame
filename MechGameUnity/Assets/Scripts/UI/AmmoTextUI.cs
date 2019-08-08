using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmmoTextUI : MonoBehaviour
{
    public Datatype_Weapon data_ref;
    public Image ammoFillBar;
    public Image reloadFillBar;
    public Image reloadImage;
    public Image ammoIcon;
    public Text text;
    public int current;
    public int max;
    
    void Start () 
    {
    
    }
    
    void LateUpdate ()
    {
        current = data_ref.executable.currentAmmo;

        text.text = current + "/" + max;

        ammoFillBar.fillAmount = current/max;
        
        if (data_ref.executable.isReloading) {
            reloadImage.enabled = true;
            // TODO: Do more stuff here, make it spin or something

            reloadFillBar.fillAmount = data_ref.executable.nextReloadStart/data_ref.executable.nextReloadEnd;
        }
        
        
    }
    
    public void Rebuild () {
        max = data_ref.executable.maxAmmo;
        current = max;
        ammoIcon = data_ref.executable.ammoIcon;
    }
}
 
