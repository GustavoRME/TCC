using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : BaseSpeciaisBossScript
{
    private AudioSource audioSource;
    private bool ContagemIniciada = false;
    private void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }


    void Update() {
        if (colidiu == false)
            transform.Translate((Vector3.down / 50) * velocidade);
        else
            Reiniciar();

        if (!ContagemIniciada)
        {
            ContagemIniciada = true;
            StartCoroutine(ContagemLaserDesativar());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().SetVida -= dano;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            colidiu = true;
        }
        else if (collision.CompareTag("Cenario"))
        {
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            colidiu = true;
        }        
    }

    IEnumerator ContagemLaserDesativar() {
        yield return new WaitForSeconds(2f);
        Reiniciar();
        ContagemIniciada = false;
    }

    void Reiniciar()
    {
        if (audioSource.isPlaying == false)
        {
            this.gameObject.SetActive(false);
            boxCollider2D.enabled = true;
            spriteRenderer.enabled = true;
            colidiu = false;
        }
    }
}
