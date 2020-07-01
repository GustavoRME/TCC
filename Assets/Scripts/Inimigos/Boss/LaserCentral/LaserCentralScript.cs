using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCentralScript : MonoBehaviour {

    float dano = 10;
    BoxCollider2D boxCollider2D;
    SpriteRenderer sr;
    AudioSource audioSource;
    bool laserAtivo = false;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(laserAtivo)
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, 1, 0.1f);
        else
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, 0, 0.1f);
    }

    public void AtivarLaser() {
        laserAtivo = true;
        boxCollider2D.enabled = true;
        sr.enabled = true;
    }

    public void DesativarLaser() {
        laserAtivo = false;
        boxCollider2D.enabled = false;
        sr.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().SetVida -= dano;
        }
    }
}
