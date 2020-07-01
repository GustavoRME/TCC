using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoFinalScript : MonoBehaviour {

    public GameObject boss;
    public DialagoScript dialagoPlayer;
    public bool falaFinalExecutada;
    public GerenDialogScript gerenDialogScript;
    public Canvas canvas;
    
    void Update () {
        if (boss == null && falaFinalExecutada == false) {
            if (Time.timeScale == 1)
            {
                falaFinalExecutada = true;
                gerenDialogScript.ComecarDialago(dialagoPlayer);
            }
        }
        if (falaFinalExecutada && Time.timeScale == 1) {
            Debug.Log("FINALIZADOOOOOOOOOOOOO");
            canvas.GetComponent<ControleUI>().jogoFinalizado = true;
        }        
	}
}
