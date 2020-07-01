using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class ControleUI : MonoBehaviour {

    private GameObject player;
    public GameObject pauseUI;
    public GameObject gameOver;
    public GameObject camera;
    public bool pausado = false;
    public bool fimDeJogo = false;
    public bool jogoFinalizado = false;
    public GameObject telaJogoFinalizado;
    public float contagemFimJogo = 0;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseUI.SetActive(false);
        gameOver.SetActive(false);
        pausado = false;
        fimDeJogo = false;
        contagemFimJogo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //REGRAS DE INPUT 
        //TELA PAUSE
        if (!fimDeJogo)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !gameOver.activeSelf)
            {
                pausado = !pausado;
                pauseUI.SetActive(pausado);
                if (pausado)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            }
        }

        //TELA GAMEOVER
        else
        {
            contagemFimJogo += Time.deltaTime * 2;
            contagemFimJogo = Mathf.Clamp(contagemFimJogo, 0, 1);
            if (contagemFimJogo == 1)
            {
                gameOver.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (jogoFinalizado) {
                telaJogoFinalizado.SetActive(true);
                Time.timeScale = 0;
        }

        //EXIBIÇÃO
        var vignetteSettings = camera.GetComponent<PostProcessingBehaviour>().profile.vignette.settings;
        vignetteSettings.intensity = contagemFimJogo;
        camera.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vignetteSettings;
        
    }

    public void FimDeJogo() {
        fimDeJogo = true;
    }

    public void Continuar() {
        pausado = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetScript();
    }

    public void VoltarAoMenu()
    {
        SceneManager.LoadScene(0);
        ResetScript();
    }

    public void ResetScript() {
        fimDeJogo = false;
        pausado = false;
        pauseUI.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        var vignetteSettings = camera.GetComponent<PostProcessingBehaviour>().profile.vignette.settings;
        vignetteSettings.intensity = 0;
        camera.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vignetteSettings;
    }
}
