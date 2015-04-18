using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //this kills the application
    public void Exit() 
    {
        Application.Quit();
    }

    public void LoadLevel(string name)
    {
        Application.LoadLevel(name);
    }

    public void ReloadLevel()
    {
        var levelName = Application.loadedLevelName;
        LoadLevel(levelName);
    }
}
