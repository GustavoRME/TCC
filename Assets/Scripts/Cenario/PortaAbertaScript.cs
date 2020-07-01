using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAbertaScript : MonoBehaviour {

    public GameObject emitirSegundoDialago;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            emitirSegundoDialago.GetComponent<TransicaoCenaScript>().segundoDialago = true;
            Destroy(this);
        }
    }
}
