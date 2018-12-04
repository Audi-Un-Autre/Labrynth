using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PAUSED : MonoBehaviour {


    public PlayerController player;
    public static bool paused = false;
    public GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
            if (paused){
                Resume();
            }
            else{
                
                Pause();
            }
        }

	}

    public void Resume(){
        AudioListener.pause = false;
        player.enabled = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Pause()
    {
        AudioListener.pause = true;
        player.enabled = false;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

}
