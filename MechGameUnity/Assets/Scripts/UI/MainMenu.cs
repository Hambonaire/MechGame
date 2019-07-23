using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public Vector3 levelPos;
    //public Vector3 levelRot;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Variables.TestInt);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToLevel()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void ToHangar()
    {
        SceneManager.LoadScene("Hangar");
        //Variables.TestInt = 1;
        //Debug.Log(Variables.TestInt);
    }

    public void Quit()
    {
        //Variables.TestInt = 3;
        //Debug.Log(Variables.TestInt);
        Application.Quit();
    }
}
