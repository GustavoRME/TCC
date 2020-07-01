using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoCenaScript : MonoBehaviour {

    private Light luz;
    private float contador;
    public bool primeiroDialago;
    public bool segundoDialago;
    public bool trocarDeCena;
    private GerenDialogScript gerenDialogScript;
    public DialagoScript textoPrimeiroDialago;
    public DialagoScript textoSegundoDialago;
    public GameObject caixaDialago;
    
    void Awake () {
        luz = GetComponent<Light>();
        gerenDialogScript = GameObject.FindGameObjectWithTag("GerenciadorDialog").GetComponent<GerenDialogScript>();
        Time.timeScale = 0;
        luz.intensity = 10;
        primeiroDialago = true;
	}
	
	void Update () {
        if (primeiroDialago)
            PrimeiroDialago();
        if (segundoDialago)
            SegundoDialago();
        if (trocarDeCena)
        {
            if (caixaDialago.activeSelf == false)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void PrimeiroDialago()
    {
        float timeScala = Mathf.Clamp(Time.timeScale + Time.deltaTime, 0, 1);
        Time.timeScale = timeScala;
        
        luz.intensity -= Time.deltaTime * 3.8f;
        if (luz.intensity <= 0)
        {
            gerenDialogScript.ComecarDialago(textoPrimeiroDialago);
            primeiroDialago = false;
        }       
    }

    void SegundoDialago()
    {
        float timeScala = Mathf.Clamp(Time.timeScale - Time.deltaTime/3f, 0, 1);
        Time.timeScale = timeScala;

        luz.intensity += Time.deltaTime * 5f;
        if (luz.intensity >= 10)
        {
            gerenDialogScript.ComecarDialago(textoSegundoDialago);
            segundoDialago = false;
            trocarDeCena = true;
        }
    }
}
