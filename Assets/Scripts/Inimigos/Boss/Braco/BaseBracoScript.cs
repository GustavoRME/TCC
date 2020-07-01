using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBracoScript : MonoBehaviour {

    [SerializeField] protected GameObject municao;
    [SerializeField] protected List<GameObject> municoesInstanciadas;
    [SerializeField] protected Transform referenciaSpawnTiro;
    [SerializeField] protected float vidaBraco = 500;
    [SerializeField] protected float defaultVidaBraco;
    [SerializeField] protected Image imgVida;
    [SerializeField] protected GameObject imgBarraVida;
    [SerializeField] protected Text textVida;
    [SerializeField] protected Text textAlvo;
    [SerializeField] protected Material materialBranco;
    protected Material defaultMaterial;

    public GameObject fogoNave;
    public GameObject explosao;

    protected void Start()
    {
        defaultVidaBraco = vidaBraco;
        defaultMaterial = GetComponent<SpriteRenderer>().material;

        for (int i = 0; i < 10; i++) {
            GameObject tiro = Instantiate(municao);
            tiro.SetActive(false);
            municoesInstanciadas.Add(tiro);
        }        
    }

    public void Atirar(float velocidade, float dano)
    {
        foreach (GameObject municaoInstanciada in municoesInstanciadas)
        {
            if (!municaoInstanciada.activeSelf)
            {
                municaoInstanciada.GetComponent<BaseSpeciaisBossScript>().velocidade = velocidade;
                municaoInstanciada.GetComponent<BaseSpeciaisBossScript>().dano = dano;
                municaoInstanciada.transform.position = referenciaSpawnTiro.position;
                municaoInstanciada.transform.rotation = this.transform.rotation;
                municaoInstanciada.SetActive(true);
                break;
            }
        }
    }

    public void Atirar(float velocidade, float dano, int lane)
    {
        foreach (GameObject municaoInstanciada in municoesInstanciadas)
        {
            if (!municaoInstanciada.activeSelf)
            {
                municaoInstanciada.GetComponent<BaseSpeciaisBossScript>().velocidade = velocidade;
                municaoInstanciada.GetComponent<BaseSpeciaisBossScript>().dano = dano;
                municaoInstanciada.GetComponent<BaseSpeciaisBossScript>().lane = lane;
                municaoInstanciada.transform.position = referenciaSpawnTiro.position;
                municaoInstanciada.transform.rotation = this.transform.rotation;
                municaoInstanciada.GetComponent<TorpedoScript>().Atirar();
                municaoInstanciada.SetActive(true);
                break;
            }
        }
    }

    protected IEnumerator Piscar()
    {
        GetComponent<SpriteRenderer>().material = materialBranco;
        yield return new WaitForSeconds(0.005f);
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    protected void Morrer()
    {
        fogoNave.SetActive(true);
        Instantiate(explosao, transform.position, transform.rotation);
        this.gameObject.SetActive(false);
    }
}
