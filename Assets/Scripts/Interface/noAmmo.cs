using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noAmmo : MonoBehaviour {
    
    public PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void FixedUpdate () {
        int index = playerScript.armaAtual;
        if (playerScript.armas[index].GetComponent<baseArmaScript>().quantidadeNoCartucho == 0)
            GetComponent<SpriteRenderer>().enabled = true;
        else
            GetComponent<SpriteRenderer>().enabled = false;
    }

    
}
