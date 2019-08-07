using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AmmoTextUI : MonoBehaviour
{
    public Datatype_Weapon data_ref;
    public Image fillBar;
    public Image reloadImage;
    public Text text;
    public int current;
    public int max;
    
    void Start () 
    {
    
    }
    
    void LateUpdate ()
    {
        current = data_ref.currentAmmo;
        
        fillBar.fillAmount = current/max;
        
        if (current == 0 || data_ref.isReloading) {
            reloadImage.enabled = true;
            // TODO: Do more stuff here, make it spin or something
        }
        
        current
    }
    
    public void Rebuild () {
        max = data_ref.maxAmmo;
        current = max;
        
    }
}
 
