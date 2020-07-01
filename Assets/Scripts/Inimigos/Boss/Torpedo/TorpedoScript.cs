using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoScript : BaseSpeciaisBossScript
{
    public GameObject propulsorMissil;
    private bool propulsorInstanciado = false;
    public GameObject explosao;    
    private Rigidbody2D rb;
    private float destino;
    private bool propulsorAtivo = false;
    
    void Start () {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
	
	public void Atirar () {
        destino = transform.position.y - (lane * 2);
        int valor = Random.Range(0, 2);
        if (valor == 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (propulsorAtivo == false)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, destino, velocidade / 100), transform.position.z);
            if (Mathf.RoundToInt(transform.position.y) == Mathf.RoundToInt(destino))
                propulsorAtivo = true;
        }
        else
        {
            if (propulsorInstanciado == false)
            {
                propulsorInstanciado = true;
                GameObject propulsor = Instantiate(propulsorMissil);
                propulsor.transform.position = new Vector2(transform.position.x - (1.36f*transform.localScale.x), transform.position.y);
                propulsor.transform.localScale = transform.localScale;
            }
            rb.AddForce((Vector3.right * velocidade)* transform.localScale.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerScript>().SetVida -= dano;
            Reiniciar();
        }
        else if (collision.CompareTag("Cenario"))
        {
            Reiniciar();
        }
    }

    private void Reiniciar()
    {
        propulsorInstanciado = false;
        propulsorAtivo = false;

        GameObject fx = Instantiate(explosao);
        fx.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
    }
}
