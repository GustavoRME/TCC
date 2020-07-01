using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class armaInimigoScript : MonoBehaviour
{
    //ATRIBUTOS
    public float velocidade;
    public float danoArma;
    public float tempoPProxTiro;
    private float ContTempoPProxTiro;

    //PARAMETROS DE SPAWN DO PROJETIL
    [SerializeField] private GameObject referenciaSpawnProjetil;
    [SerializeField] private GameObject referenciaProjetil;
    [SerializeField] private List<GameObject> projeteis;

    //COMPONENTES
    [SerializeField] private AudioClip fogo;
    [SerializeField] private float barulhoPitch;
    private AudioSource audioSource;

    // Use this for initialization
    protected void Start()
    {
        //INICIALIZA SOM
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = barulhoPitch;

        //INSTANCIA O CARTUCHO
        projeteis = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject tempBullet = Instantiate(referenciaProjetil);
            tempBullet.SetActive(false);
            projeteis.Add(tempBullet);
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (ContTempoPProxTiro > 0)
        {
            ContTempoPProxTiro -= Time.deltaTime;
        }
        else
        {
            ContTempoPProxTiro = 0;
        }
    }

    public void InstanciarProjetil(float direcaoProjetil)
    {
        if (ContTempoPProxTiro == 0)
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
        }
    }
}
