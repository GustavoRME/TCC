﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projetil_inimigos : BaseProjetil
{

    // Use this for initialization
    void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
        if (colidiu)
            ReiniciarPadrao();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            other.GetComponent<PlayerScript>().SetVida -= danoArma;
            audioSource.pitch = 10f;
            bulletAnimation.SetBool("enemy_hit", true);
        }
        else
        if (other.tag == "Cenario")
        {
            audioSource.pitch = 3f;
            bulletAnimation.SetBool("object_hit", true);
        }

        audioSource.PlayOneShot(somColisao);
        colidiu = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Light>().enabled = false;
    }
}
