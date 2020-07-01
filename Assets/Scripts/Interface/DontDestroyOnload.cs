using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnload : MonoBehaviour {

    public AudioClip musica01;
    public AudioClip musica02;
    public AudioSource audioSource;
    bool fimdeJogo = false;

    void Start () {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (fimdeJogo && SceneManager.GetActiveScene().buildIndex != 4)
        {
            Destroy(this.gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 && audioSource.clip != musica02)
        {
            audioSource.clip = musica02;
            audioSource.Play();
            audioSource.spatialBlend = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2 && audioSource.clip != musica02)
        {
            audioSource.clip = musica02;
            audioSource.Play();
            audioSource.spatialBlend = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex == 3 && audioSource.clip != musica02)
        {
            audioSource.clip = musica02;
            audioSource.Play();
            audioSource.spatialBlend = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex == 4 && audioSource.clip != musica01)
        {
            audioSource.clip = musica01;
            audioSource.Play();
            audioSource.spatialBlend = 0;
            fimdeJogo = true;
        }
    }
}
