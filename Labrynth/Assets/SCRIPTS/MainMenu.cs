using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update () {

    }

    public void NewGame(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
