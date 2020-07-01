using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteBossScript : MonoBehaviour {

    public GameObject nave;
    public GameObject UIBoss;
    public GameObject explosao;
    public GameObject eventoFinal;
    public Light luz;    
    
    public bool podeInstanciar = true;
    public int quantidadeInstancias = 15;
    public bool encerramento = false;
    public DialagoScript dialagoFinalBoss;
    public DialagoScript dialagoFinalPlayer;
    public GerenDialogScript gerenciadorDialago;

    private void Start()
    {
        luz = eventoFinal.GetComponent<Light>();
    }
    void FixedUpdate()
    {
        if (encerramento == false)
        {
            if (podeInstanciar && quantidadeInstancias > 0)
            {
                GameObject instancia = Instantiate(explosao, transform.position, transform.rotation);
                float valorX = Random.Range(-4, 5);
                float valorY = Random.Range(-4, 5);
                instancia.transform.position += new Vector3(valorX, valorY);
                StartCoroutine(ContagemPodeInstanciar());
                podeInstanciar = false;
                quantidadeInstancias--;
            }

            luz.intensity = Mathf.Lerp(luz.intensity, 70, 0.01f);
            if ((int)luz.intensity >= 69)
            {
                encerramento = true;
                gerenciadorDialago.ComecarDialago(dialagoFinalBoss);
                Destroy(UIBoss);
                Destroy(nave);
            }
        }
    }
    

    IEnumerator ContagemPodeInstanciar() {
        yield return new WaitForSeconds(0.5f);
        podeInstanciar = true;
    }
}
