using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisselScript : BaseSpeciaisBossScript
{
    private GameObject alvo;
    public GameObject explosaoMissel;
    private Rigidbody2D rb;

    private void Start()
    {
        base.Start();
        alvo = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(anguloImpulso() * 2);
        if (Mathf.Abs(rb.velocity.x) > velocidade / 2)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * (velocidade / 2), rb.velocity.y);
        if (Mathf.Abs(rb.velocity.y) > velocidade / 2)
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * (velocidade / 2));
        Rotacionar();
    }

    void Rotacionar()
    {
        transform.rotation = anguloObjeto();
    }

    Quaternion anguloObjeto()
    {
        Vector3 posicaoAngulo;
        posicaoAngulo.x = alvo.transform.position.x - transform.position.x;
        posicaoAngulo.y = alvo.transform.position.y - transform.position.y;

        float angulo = Mathf.Atan2(posicaoAngulo.y, posicaoAngulo.x) * Mathf.Rad2Deg;

        if (angulo > 180)
            return Quaternion.Euler(new Vector3(0, 0, angulo + 180));
        else if (angulo < -180f)
            return Quaternion.Euler(new Vector3(0, 0, angulo - 180));
        else
            return Quaternion.Euler(new Vector3(0, 0, angulo));
    }

    Vector2 anguloImpulso()
    {
        Vector2 posicaoAngulo;
        posicaoAngulo.x = alvo.transform.position.x - transform.position.x;
        posicaoAngulo.y = alvo.transform.position.y - transform.position.y;

        return posicaoAngulo;
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
        GameObject fx = Instantiate(explosaoMissel);
        fx.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
    }
}
