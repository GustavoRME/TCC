using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseProjetil : MonoBehaviour {

    //ATRIBUTOS
    public float velocidade;
    public float danoArma;
    public float direcao;
    public float tempoMorte;
    public float tempoMorteContagem;
    public string tagAlvo;
    [SerializeField] public AudioClip somColisao;

    //PROPRIEDADES DE MOVIMENTO
    public bool colidiu = false;

    //Componentes
    public Animator bulletAnimation;
    public AudioSource audioSource;

    protected void Awake()
    {
        bulletAnimation = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void FixedUpdate()
    {
        //FAZ A BALA CORRER PARA UM DOS LADOS
        if (colidiu != true)
        {
            if (direcao > 0)
                transform.Translate(Vector3.right * velocidade * Time.deltaTime);
            if (direcao < 0)
                transform.Translate(Vector3.left * velocidade * Time.deltaTime);
        }

        tempoMorteContagem += Time.deltaTime;
        if (tempoMorteContagem >= tempoMorte)
        {
            ReiniciarPadrao();            
        }
    }

    protected void ReiniciarPadrao() {
        gameObject.SetActive(false);
        bulletAnimation.SetBool("enemy_hit", false);
        bulletAnimation.SetBool("object_hit", false);        
        tempoMorteContagem = 0;
        colidiu = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Light>().enabled = true;
    }
}
