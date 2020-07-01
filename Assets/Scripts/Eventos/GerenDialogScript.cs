using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerenDialogScript : MonoBehaviour {

    public Text nomeTexto;
    public Text dialagoTexto;
    public AudioClip teclando;
    private AudioSource audioSource;

    public Queue<string> sentencas;
    public GameObject caixaDialago;
    public Canvas canvas;
    private ControleUI controleUI;

	void Start () {
        sentencas = new Queue<string>();
        controleUI = canvas.GetComponent<ControleUI>();
        audioSource = GetComponent<AudioSource>();
    }


    public void ComecarDialago(DialagoScript dialago) {
        Time.timeScale = 0;
        controleUI.enabled = false;
        Debug.Log("Começando dialago com: " + dialago.nome);

        caixaDialago.SetActive(true);
        nomeTexto.text = dialago.nome;
        sentencas.Clear();

        foreach (string sentenca in dialago.sentencas) {
            sentencas.Enqueue(sentenca);
        }

        MostrarProximaSentenca();
    }

      public void MostrarProximaSentenca()
    {
        if (sentencas.Count == 0) {
            EncerrarDialago();
            return;
        }

        string sentenca = sentencas.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TipoSentenca(sentenca));
    }

    IEnumerator TipoSentenca(string sentenca) {
        dialagoTexto.text = "";
        foreach (char letra in sentenca.ToCharArray()) {
            dialagoTexto.text += letra;
            if(audioSource.isPlaying == false)
                audioSource.PlayOneShot(teclando);
            yield return null;
        }
    }

    void EncerrarDialago() {
        Debug.Log("Conversa encerrada");
        caixaDialago.SetActive(false);
        controleUI.enabled = true ;
        Time.timeScale = 1;
    }

}
