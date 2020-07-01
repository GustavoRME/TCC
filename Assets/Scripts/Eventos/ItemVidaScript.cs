using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemVidaScript : MonoBehaviour {
    
    [SerializeField] private float multiplicadorVida;
    [SerializeField] private AudioClip som;
    [SerializeField] private bool destruindo;
    [SerializeField] private SpriteRenderer[] spriteRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(destruindo && !audioSource.isPlaying)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destruindo)
        {
            if (collision.tag == "Player" || collision.tag == "Player auxiliar")
            {
                if (collision.GetComponent<PlayerScript>().SetVida < 100)
                {
                    collision.GetComponent<PlayerScript>().SetVida += 10 * multiplicadorVida;
                    audioSource.PlayOneShot(som);
                    foreach (SpriteRenderer tempFilho in spriteRenderer)
                        tempFilho.enabled = false;
                    destruindo = true;
                }
            }
        }
    }
}
