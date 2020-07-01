using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemMunicaoScript : MonoBehaviour {
    
    [SerializeField] private float multiplicadorMunicao;
    [SerializeField] private AudioClip som;
    [SerializeField] private bool destruindo;
    [SerializeField] private SpriteRenderer[] spriteRenderer;
    private AudioSource audioSource;

    private void Awake()
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
                foreach (GameObject tempArmas in collision.GetComponent<PlayerScript>().armas)
                {
                    tempArmas.GetComponent<baseArmaScript>().quantidadeMunicao += tempArmas.GetComponent<baseArmaScript>().tamanhoCartucho * multiplicadorMunicao;
                }
                audioSource.PlayOneShot(som);
                foreach(SpriteRenderer tempfilhoSprite in spriteRenderer)
                    tempfilhoSprite.enabled = false;
                destruindo = true;
            }
        }
    }
}
