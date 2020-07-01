using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    private Rigidbody2D rb;
    private Transform tr;
    private Animator an;
    private SpriteRenderer sr;

    [Header("Atributos")]
    [SerializeField]private float velocidade;
    [SerializeField]private float forcaPulo;
    [SerializeField]private float vidaPlayer;
    [SerializeField]private float tempoDeImunidade;
    [SerializeField]private float invulneravelContagem;

    [Header("Armas")]
    [SerializeField] public List<GameObject> armas;
    [SerializeField] public int armaAtual;

    [Header("Outros")]
    [SerializeField]private Transform detecChao;
    [SerializeField]private float alcanceChao;
    [SerializeField]private LayerMask layerChao;
    [SerializeField]private bool estaNoChao;
    [SerializeField]bool estaAndando;
    [SerializeField]private Vector3 mousePosicao;
    [SerializeField]private Vector3 WorldTPoint;
    [SerializeField]private int posOffSet;
    [SerializeField]private float angulo;
    [SerializeField] GameObject canvas;
    [SerializeField] ControleUI controleUI;

    [SerializeField] private float contadorPiscadas = 5f;
    private float defaultContadorPiscadas;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        controleUI = canvas.GetComponent<ControleUI>();
        defaultContadorPiscadas = contadorPiscadas;
    }
	
	void Update () {
        if(Time.timeScale > 0)
            AtualizaInput();

        if (vidaPlayer <= 0f)
            Morrer();
	}
    private void FixedUpdate()
    {
        AtualizaSensores();
        AtualizaAnimator();
        AtualizaOrientacao();
        AtualizaAcao();
    }

    private void AtualizaInput()
    {
        //SE ESTIVER NO CHAO E COM O BOTÃO "ESPACO" APERTADO ELE PULA
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
        }

        //ATIRAR
        if (Input.GetMouseButton(0))
        {
            if (armas[armaAtual].GetComponent<baseArmaScript>().armaDisponivel)
            {
                armas[armaAtual].GetComponent<baseArmaScript>().InstanciarProjetil(transform.localScale.x);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (armas[armaAtual].GetComponent<baseArmaScript>().armaDisponivel)
            {
                Debug.Log("Recarregado");
                armas[armaAtual].GetComponent<baseArmaScript>().RecarregarArma();
            }
        }


        //TROCA DE ARMA
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (armas[0].GetComponent<baseArmaScript>().armaDisponivel == true)
            {
                armaAtual = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (armas[1].GetComponent<baseArmaScript>().armaDisponivel == true)
            {
                armaAtual = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (armas[2].GetComponent<baseArmaScript>().armaDisponivel == true)
            {
                armaAtual = 2;
            }
        }

        foreach (GameObject armasEach in armas)
        {
            if (armasEach == armas[armaAtual])
            {
                if (armasEach.active == false)
                {
                    armasEach.SetActive(true);
                    armasEach.GetComponent<baseArmaScript>().ArmaSelecionada();
                }
            }
            else
                armasEach.SetActive(false);
        }
    }

    private void AtualizaSensores() {
        //MOVIMENTA ESQUERDA E DIREITA
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocidade, rb.velocity.y);

        //VERIFICA SE ESTÁ NO CHAO
        estaNoChao = Physics2D.Linecast(detecChao.transform.position, new Vector2(detecChao.transform.position.x + alcanceChao, detecChao.transform.position.y), layerChao) ||
                     Physics2D.Linecast(detecChao.transform.position, new Vector2(detecChao.transform.position.x - alcanceChao, detecChao.transform.position.y), layerChao);
    }

    private void AtualizaAcao()
    {
        //ATUALIZA INVULNERABILIDADE APÓS DANO
        if (invulneravelContagem > 0)
        {
            invulneravelContagem -= Time.deltaTime;
            if (invulneravelContagem <= 0)
                invulneravelContagem = 0;
        }
    }

    private void AtualizaOrientacao()
    {
        //ATUALIZA ORIENTAÇÃO DO PERSONAGEM
        mousePosicao = Input.mousePosition;
        mousePosicao.z = 0;

        WorldTPoint = Camera.main.WorldToScreenPoint(transform.position);

        mousePosicao.x = mousePosicao.x - WorldTPoint.x;
        mousePosicao.y = mousePosicao.y - WorldTPoint.y;

        angulo = Mathf.Atan2(mousePosicao.y, mousePosicao.x) * Mathf.Rad2Deg;

        if (angulo > 90f || angulo < -90f)
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);

        //ATUALIZA ORIENTAÇÃO DA ARMA
        if (angulo > 90f)
            armas[armaAtual].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo+180));
        else if (angulo < -90f)
            armas[armaAtual].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo-180));
        else
            armas[armaAtual].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));
    }

    private void AtualizaAnimator()
    {
        if (Input.GetAxis("Horizontal") == 0) {
            estaAndando = false;
        }
        else {
            estaAndando = true;
        }
        an.SetBool("playerJump", !estaNoChao);
        an.SetBool("playerRun", estaAndando);

        if (invulneravelContagem > 0 && contadorPiscadas <= 0)
        {
            if (sr.color.a > 0f)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            else if (sr.color.a == 0f)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 255f);
            contadorPiscadas = defaultContadorPiscadas;
        }
        else
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 255f);

        contadorPiscadas--;
    }

    private void Morrer()
    {
        controleUI.FimDeJogo();
        vidaPlayer = 100f;
        gameObject.SetActive(true);
    }

    //COMUNICADORES
    public float SetVida
    {
        get { return vidaPlayer; }
        set {
            if (invulneravelContagem == 0)
            {
                if(vidaPlayer > value)
                    invulneravelContagem = tempoDeImunidade;
                vidaPlayer = value;
            }

            if (vidaPlayer <= 0)
                vidaPlayer = 0;
        }
    }

    public Vector2 SetRigidbody2D
    {
        set
        {
            if (invulneravelContagem == 0)
            {
                rb.velocity = value;
            }
        }
    }

    private void OnDrawGizmos()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(detecChao.transform.position, new Vector2(detecChao.transform.position.x + alcanceChao, detecChao.transform.position.y));
        Gizmos.DrawLine(detecChao.transform.position, new Vector2(detecChao.transform.position.x - alcanceChao, detecChao.transform.position.y));

    }
}
