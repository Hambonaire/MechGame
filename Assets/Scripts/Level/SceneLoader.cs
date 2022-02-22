using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *  Handles loading levels
 *  - Primed scene
 * 
 */
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader _instance;

    public string primedSceneName;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPrimedlevel()
    {
        LoadScene(primedSceneName);

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (sceneName.Contains("Level"))
        {
            OnLevelLoaded();
        }
    }

    public void OnLevelLoaded()
    {

    }

    public void OnHangarLoad()
    {

    }
}
