using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class baseArmaScript : MonoBehaviour {

    //SE A ARMA ESTA DISPONIVEL PARA O PERSONAGEM
    public bool armaDisponivel;

    //ATRIBUTOS
    public float velocidade;
    public float distancia;
    public float danoArma;
    public float tempoPProxTiro;
    private float ContTempoPProxTiro;
    public float quantidadeNoCartucho;
    public float tamanhoCartucho;
    public float quantidadeMunicao;
    public Sprite spriteArma;

    //PARAMETROS DE SPAWN DO PROJETIL
    [SerializeField] private GameObject referenciaSpawnProjetil;
    [SerializeField] public GameObject referenciaProjetil;
    [SerializeField] private List<GameObject> projeteis;

    //COMPONENTES
    [SerializeField] private AudioClip fogo;
    [SerializeField] private AudioClip semMunicao;
    [SerializeField] private AudioClip recarregando;
    [SerializeField] private float barulhoPitch;
    private AudioSource audioSource;

    // Use this for initialization
    protected void Awake () {
        Debug.Log("Startado");
        //A ARMA JÁ INICIA INDISPONÍVEL PARA USO, PARA POSTERIORMENTE "PEGARMOS"

        //INICIALIZA SOM
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = barulhoPitch;
               
        //INSTANCIA O CARTUCHO
        projeteis = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            GameObject tempBullet = Instantiate(referenciaProjetil);
            tempBullet.SetActive(false);
            projeteis.Add(tempBullet);
        }

        //INICIA A ARMA COM CARTUCHO CHEIO
        quantidadeNoCartucho = tamanhoCartucho;
    }
	
	// Update is called once per frame
	protected void FixedUpdate () {
        if (ContTempoPProxTiro > 0)
        {
            ContTempoPProxTiro -= Time.deltaTime;
        }
        else {
            ContTempoPProxTiro = 0;
        }
    }

    public void InstanciarProjetil(float direcaoProjetil)
    {
        if (ContTempoPProxTiro == 0)
        {
            if (quantidadeNoCartucho > 0)
            {
                foreach (GameObject projetilDaLista in projeteis)
                {
                    if (projetilDaLista.activeInHierarchy == false)
                    {
                        //PASSA TODAS AS CARACTERISTICAS PARA O PROJETIL GENÉRICO APENAS EXECUTAR
                        projetilDaLista.transform.position = referenciaSpawnProjetil.transform.position;
                        projetilDaLista.transform.rotation = transform.rotation;
                        projetilDaLista.GetComponent<BaseProjetil>().direcao = direcaoProjetil;
                        projetilDaLista.GetComponent<BaseProjetil>().velocidade = this.velocidade;
                        projetilDaLista.GetComponent<BaseProjetil>().danoArma = this.danoArma;
                        projetilDaLista.SetActive(true);

                        //EXECUTA O SOM                        
                        audioSource.pitch = barulhoPitch;
                        audioSource.PlayOneShot(fogo);                        
                        break;
                    }
                }

                //DA UM DELAY PARA O PROXIMO TIRO
                ContTempoPProxTiro = tempoPProxTiro;

                //QUANDO VOCÊ ATIRA, REDUZ A QUANTIDADE DE BALAS QUE POSSUI
                quantidadeNoCartucho--;
            }
            else
            {
                //FAZ O BARULHO DE QUANDO VOCÊ PRECISA RECARREGAR A ARMA
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(semMunicao);
            }
        }
    }

    public void RecarregarArma()
    {
        //RECARREGA O CARTUCHO COM A QUANTIDADE DE MUNIÇÃO QUE VOCÊ TEM
        if (quantidadeNoCartucho < tamanhoCartucho)
        {
            while (quantidadeNoCartucho < tamanhoCartucho && quantidadeMunicao != 0)
            {
                --quantidadeMunicao;
                ++quantidadeNoCartucho;
            }
            audioSource.PlayOneShot(recarregando);
            ContTempoPProxTiro = tempoPProxTiro*2;
        }
    }

    public void ArmaSelecionada() {
        audioSource.PlayOneShot(recarregando);
    }
}
