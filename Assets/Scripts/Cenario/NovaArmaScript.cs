using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaArmaScript : MonoBehaviour {

    public int armaASerDesbloqueada;
    public GameObject portaASerAberta;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (armaASerDesbloqueada != 0)
            {
                collision.GetComponent<PlayerScript>().armas[armaASerDesbloqueada].GetComponent<baseArmaScript>().armaDisponivel = true;
                collision.GetComponent<PlayerScript>().armaAtual = armaASerDesbloqueada;
            }

            portaASerAberta.GetComponent<PortaScript>().AbrirPorta();
            Destroy(this.gameObject);
        }
    }
}
