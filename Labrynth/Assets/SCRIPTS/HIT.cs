using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIT : MonoBehaviour {

    public HealthManager health;
    public CanvasGroup UI;
    public bool fade = false;
    public AudioSource audio;

    public void Start()
    {
        UI = GetComponent<CanvasGroup>();
        audio = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (health.currHealth < 10 && UI.alpha != 1){
            FadeIn();
            if (!audio.isPlaying)
                audio.Play(0);
        }
        else if (health.currHealth >= 10 && UI.alpha != 0){
            FadeOut();
            if (audio.isPlaying)
                audio.Pause();
        }
    }

    // FADE OUT CANVAS BLOOD
    public void FadeOut(){
        StartCoroutine(FadeCanvasGroup(true));
    }

    // FADE IN CANVAS BLOOD
    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(false));
    }

    // COROUTINE
    public IEnumerator FadeCanvasGroup(bool fade)
    {
        UI = GetComponent<CanvasGroup>();
        if (fade){
            while (UI.alpha > 0){
                UI.alpha -= Time.deltaTime / 200;
                yield return null;
            }
        }

        else if(!fade)
        {
            while (UI.alpha < 1)
            {
                UI.alpha += Time.deltaTime / 200;
                yield return null;
            }
        }

        //UI.interactable = false;
        yield return null;
    }
}
