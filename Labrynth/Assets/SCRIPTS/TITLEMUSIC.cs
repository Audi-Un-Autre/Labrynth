using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TITLEMUSIC : MonoBehaviour {

    void Awake()
    {
        GameObject[] musics = GameObject.FindGameObjectsWithTag("TitleMusic");
        if (musics.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GAME TEST")
        {
            Destroy(this.gameObject);
        }
    }
}
