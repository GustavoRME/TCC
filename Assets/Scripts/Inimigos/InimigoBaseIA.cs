using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InimigoBaseIA : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Transform tr;
    protected Animator an;
    protected Collider2D co;
    protected CapsuleCollider2D capsuleCollider2D;
    protected SpriteRenderer sr;

    [Header("Atributos")]
    [SerializeField] protected float velocidade;
    [SerializeField] protected float forcaPulo;
    [SerializeField] protected float vidaEnemy;
    [SerializeField] protected float danoCorpo;
    [SerializeField] protected float knockBack;

    [Header("Drop")]
    [SerializeField] protected GameObject[] drops;

    [Header("SensorAlvo")]
    [SerializeField] protected GameObject playerAlvo;
    [SerializeField] protected bool alvoAcima;
    [SerializeField] protected bool alvoAbaixo;
    [SerializeField] protected bool alvoAEsquerda;
    [SerializeField] protected bool AlvoADireita;
    [SerializeField] protected float zonaCega;
    [SerializeField] protected float raioDePerseguicao;
    [SerializeField] protected LayerMask layerAlvo;
    [SerializeField] protected bool perseguindo;

    [Header("SensorAmbiente")]
    [SerializeField] protected Transform detecChao;
    [SerializeField] protected float alcanceChao;
    [SerializeField] protected LayerMask solido;
    [SerializeField] protected Transform tranformDetecL0C1;
    [SerializeField] protected Transform tranformDetecL0C2;
    [SerializeField] protected Transform tranformDetecL1C1;
    [SerializeField] protected Transform tranformDetecL1C2;
    [SerializeField] protected Transform tranformDetecL2C1;
    [SerializeField] protected Transform tranformDetecL2C2;
    [SerializeField] protected Transform tranformDetecL3C1;
    [SerializeField] protected Transform tranformDetecL3C2;
    [SerializeField] protected float raioDetectores;

    [SerializeField] protected bool estaNoChao;
    [SerializeField] protected bool detectorL0C1;
    [SerializeField] protected bool detectorL0C2;
    [SerializeField] protected bool detectorL1C1;
    [SerializeField] protected bool detectorL1C2;
    [SerializeField] protected bool detectorL2C1;
    [SerializeField] protected bool detectorL2C2;
    [SerializeField] protected bool detectorL3C1;
    [SerializeField] protected bool detectorL3C2;
    [SerializeField] protected string direcaoPuloReferencia;

    [SerializeField] protected List<GameObject> referenciasCenario;

    [Header("Logica")]
    [SerializeField] protected bool perseguicaoHorizontal;
    [SerializeField] protected float persistenciaEixo;
    [SerializeField] protected GameObject refMaisProxima;
    [SerializeField] protected float variacaoBlocos;
    [SerializeField] protected bool seguindoReferencia;

    [Header("Interno")]
    [SerializeField] protected Material materialPiscada;
    [SerializeField] protected float contPerseguicaoHorizontal;
    [SerializeField] protected bool andarL;
    [SerializeField] protected bool andarR;
    [SerializeField] protected bool pular;
    [SerializeField] protected bool atacar;

    [SerializeField] protected float tempoTomadaDeDecisao;
    [SerializeField] protected float contTempoTomadaDeDecisao;

    [SerializeField] protected float contadorMorte = 3f;
    protected int quantidadePiscadas;

    protected bool lDesligadoJump = false, rDesligadoJump = false;
    protected float defaultVida, defaultVelocidade, defaultContMorte;
    protected Material defaultMaterial;
    
    // Use this for initialization
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        //RECONHECE O ALVO
        playerAlvo = GameObject.FindGameObjectWithTag("Player");

        //FAZ O RECONHECIMENTO DO CENARIO
        referenciasCenario = new List<GameObject>();
        referenciasCenario.AddRange(GameObject.FindGameObjectsWithTag("pontoDeRefCenario"));

        //ARMAZENA OS PARAMETROS PADRÕESPARA REAPLICAR DEPOIS
        defaultVelocidade = velocidade;
        defaultContMorte = contadorMorte;
        quantidadePiscadas = (int)contadorMorte * 5;

        //ARMAZENA A VIDA PADRÃO PARA REAPLICAR DEPOIS
        defaultVida = vidaEnemy;
        defaultMaterial = sr.material;

        //DEFINE A ALTURA MÁXIMA QUE O PERSONAGEM CONSEGUE PULAR
        variacaoBlocos = Mathf.Abs(transform.position.y - tranformDetecL2C1.position.y);
    }

    protected void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            raioDePerseguicao += 1;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            raioDePerseguicao -= 1;
        }
    }

    protected void FixedUpdate()
    {
        if (vidaEnemy > 0)
        {
            Sensores();
            if (perseguindo)
            {
                LogicaAgressiva();
                velocidade = defaultVelocidade * 2;
            }
            else
            {
                LogicaPacifica();
                velocidade = defaultVelocidade / 2;
            }
            AtualizaAcao();

        }
        else 
            Morrer();
    }

    #region Sensores
    protected void Sensores()
    {
        //VERIFICA SE ESTÁ NO CHAO
        estaNoChao = Physics2D.Linecast(detecChao.transform.position, new Vector2(detecChao.transform.position.x + alcanceChao * 1.5f, detecChao.transform.position.y), solido) &&
                     Physics2D.Linecast(detecChao.transform.position, new Vector2(detecChao.transform.position.x - alcanceChao * 1.5f, detecChao.transform.position.y), solido);

        //VERIFICA SE HÁ BURACOS NA FRENTE
        detectorL0C1 = Physics2D.OverlapCircle(tranformDetecL0C1.transform.position, raioDetectores, solido);
        detectorL0C2 = Physics2D.OverlapCircle(tranformDetecL0C2.transform.position, raioDetectores, solido);

        //VERIFICA SE HÁ OBSTACULOS NA FRENTE
        detectorL1C1 = Physics2D.OverlapCircle(tranformDetecL1C1.transform.position, raioDetectores, solido);
        detectorL1C2 = Physics2D.OverlapCircle(tranformDetecL1C2.transform.position, raioDetectores, solido);

        //VERIFICA SE HÁ PLATAFORMA ACIMA
        detectorL2C1 = Physics2D.Linecast(tranformDetecL2C1.transform.position, new Vector2(tranformDetecL2C1.transform.position.x - alcanceChao * 10, tranformDetecL2C1.transform.position.y), solido);
        detectorL2C2 = Physics2D.Linecast(tranformDetecL2C2.transform.position, new Vector2(tranformDetecL2C2.transform.position.x + alcanceChao * 10, tranformDetecL2C2.transform.position.y), solido);

        //VERIFICA SE HÁ OBSTACULOS NA PLATAFORMA ACIMA
        detectorL3C1 = Physics2D.Linecast(tranformDetecL3C1.transform.position, new Vector2(tranformDetecL3C1.transform.position.x - alcanceChao * 8, tranformDetecL3C1.transform.position.y), solido);
        detectorL3C2 = Physics2D.Linecast(tranformDetecL3C2.transform.position, new Vector2(tranformDetecL3C2.transform.position.x + alcanceChao * 8, tranformDetecL3C1.transform.position.y), solido);

        //VERIFICA SE O ALVO ESTÁ DENTRO DO RAIO
        perseguindo = Physics2D.OverlapCircle(transform.position, raioDePerseguicao, layerAlvo);

        if (!detectorL2C1 && !lDesligadoJump)
        {
            lDesligadoJump = true;
            direcaoPuloReferencia = "Esquerda";
        }
        else if (detectorL2C1 && lDesligadoJump) {
            lDesligadoJump = false;
        }

        if (!detectorL2C2 && !rDesligadoJump)
        {
            rDesligadoJump = true;
            direcaoPuloReferencia = "Direita";
        }
        else if (detectorL2C2 && rDesligadoJump)
        {
            rDesligadoJump = false;
        }

        //LOCALIZA A POSIÇÃO HORIZONTAL E VERTICAL DO ALVO
        if (playerAlvo != null)
        {
            //INIMIGO EM CIMA OU EMBAIXO
            if (this.transform.position.y < playerAlvo.transform.position.y - zonaCega)
            {
                alvoAcima = true;
                alvoAbaixo = false;
            }
            else if (this.transform.position.y > playerAlvo.transform.position.y + zonaCega)
            {
                alvoAcima = false;
                alvoAbaixo = true;
            }
            else
            {
                alvoAcima = false;
                alvoAbaixo = false;
            }


            //INIMIGO À DIREITA OU ESQUERDA
            if (this.transform.position.x < playerAlvo.transform.position.x - zonaCega)
            {
                AlvoADireita = true;
                alvoAEsquerda = false;
            }
            else if (this.transform.position.x > playerAlvo.transform.position.x + zonaCega)
            {
                AlvoADireita = false;
                alvoAEsquerda = true;
            }
            else
            {
                AlvoADireita = false;
                alvoAEsquerda = false;
            }
        }        
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(detecChao.transform.position, new Vector2(detecChao.transform.position.x + alcanceChao, detecChao.transform.position.y));
        Gizmos.DrawLine(detecChao.transform.position, new Vector2(detecChao.transform.position.x - alcanceChao, detecChao.transform.position.y));

        Gizmos.DrawWireSphere(tranformDetecL0C1.transform.position, raioDetectores);
        Gizmos.DrawWireSphere(tranformDetecL0C2.transform.position, raioDetectores);
        Gizmos.DrawWireSphere(tranformDetecL1C1.transform.position, raioDetectores);
        Gizmos.DrawWireSphere(tranformDetecL1C2.transform.position, raioDetectores);
        Gizmos.DrawLine(tranformDetecL2C1.transform.position, new Vector2(tranformDetecL2C1.transform.position.x - alcanceChao * 10 * transform.localScale.x, tranformDetecL2C2.transform.position.y));
        Gizmos.DrawLine(tranformDetecL2C2.transform.position, new Vector2(tranformDetecL2C2.transform.position.x + alcanceChao * 10 * transform.localScale.x, tranformDetecL2C2.transform.position.y));
        Gizmos.DrawLine(tranformDetecL3C1.transform.position, new Vector2(tranformDetecL3C1.transform.position.x - alcanceChao * 8, tranformDetecL3C1.transform.position.y));
        Gizmos.DrawLine(tranformDetecL3C2.transform.position, new Vector2(tranformDetecL3C2.transform.position.x + alcanceChao * 8, tranformDetecL3C2.transform.position.y));

        Gizmos.DrawWireSphere(transform.position, raioDePerseguicao);
    }
    #endregion

    #region Logica Agressiva
    protected void LogicaAgressiva()
    {
        #region Perseguição Eixo X (Seguindo jogador)
        //PERSEGUIÇÃO EIXO X ===========================================================================================        
        if (perseguicaoHorizontal)
        {
            //PERSEGUE NO EIXO X
            if (alvoAEsquerda)
            {
                andarL = true;
                andarR = false;
            }
            else
            if (AlvoADireita)
            {
                andarL = false;
                andarR = true;
            }
            else
            {
                andarL = false;
                andarR = false;
            }

            //SE O PLAYER ESTIVER MUITO ALTO OU MUITO BAIXO, AUTOMATICAMENTE MUDARÁ PARA PERSEGUIÇÃO VERTICAL
            if (Mathf.Abs(tr.position.y - playerAlvo.transform.position.y) > variacaoBlocos)
                perseguicaoHorizontal = false;

            //SE O ALVO NÃO ESTIVER A ESQUERDA E NEM A DIREITA, DEPOIS DE UMA QUANTIDADE DE SEGUNDOS, MUDA O EIXO DE PERSEGUIÇÃO
            if (!alvoAEsquerda && !AlvoADireita)
            {
                contPerseguicaoHorizontal -= Time.deltaTime;
                if (contPerseguicaoHorizontal <= 0)
                {
                    perseguicaoHorizontal = false;
                    contPerseguicaoHorizontal = persistenciaEixo;
                }
            }
        }
        #endregion
        else
        #region Perseguição Eixo Y  (Seguindo referência)      
        {
            //SE ELE QUERER SE ALINHAR NA POSIÇÃO Y E NÃO ESTIVER SEGUINDO A REFERENCIA, O INIMIGO CALCULARÁ A REFERENCIA MAIS PROXIMA PARA SUBIR OU DESCER
            //CASO TENHA REFERENCIA, ELE APENAS VAI "SETAR" OS PARAMETROS PARA SEGUIR.
            if (seguindoReferencia)
            {
                if (estaNoChao)
                {
                    //SE A REFERENCIA ESTIVER À ESQUERDA, VAI PARA ESQUERDA
                    if (this.transform.position.x < refMaisProxima.transform.position.x - zonaCega)
                    {
                        andarL = false;
                        andarR = true;
                    }
                    else
                    //SE A REFERENCIA ESTIVER À DIREITA, VAI PARA DIREITA   
                    if (this.transform.position.x > refMaisProxima.transform.position.x + zonaCega)
                    {
                        andarL = true;
                        andarR = false;
                    }
                    else
                    {
                        andarL = false;
                        andarR = false;

                        //SE ELE ESTIVER NA ZONA MORTA E NO CHÃO, PULARÁ
                        if (!detectorL2C1 && !detectorL2C2 && alvoAcima)
                        {
                            rb.velocity = new Vector2(transform.localScale.x * (forcaPulo / 5), forcaPulo * 1.1f);
                        }
                    }
                    contPerseguicaoHorizontal -= Time.deltaTime;
                    if (contPerseguicaoHorizontal <= 0)
                    {
                        perseguicaoHorizontal = false;
                        contPerseguicaoHorizontal = persistenciaEixo;
                        seguindoReferencia = false;
                    }
                }

                //SE A POSIÇÃO DO INIMIGO FOR IGUAL QUE A POSIÇÃO DE REFERENCIA, ELE VAI PARAR DE SEGUIR AQUELA REFERENCIA.
                if (transform.position.y > refMaisProxima.transform.position.y)
                {
                    seguindoReferencia = false;
                }

                //SE A REFERENCIA FICAR MUITO LONGE NO EIXO Y ELE BUSCARÁ UMA NOVA REFERENCIA
                if ((refMaisProxima.transform.position.y - tr.position.y) > variacaoBlocos)
                    seguindoReferencia = false;

                //SE O ALVO ESTIVER ALINHADO NO EIXO VERTICAL, VAI VOLTAR NOVAMENTE PARA ALINHAMENTO HORIZONTAL
                if (!alvoAcima && !alvoAbaixo)
                {
                    perseguicaoHorizontal = true;
                    seguindoReferencia = false;
                }

                
            }
            else
            {
                BuscarReferencia();
                if (!alvoAcima && !alvoAbaixo)
                    perseguicaoHorizontal = true;
            }
        }

        //SE ESTIVER PERSEGUINDO NO EIXO X E TIVER UM OBSTACULO, ELE PULARÁ =======================================
        if (estaNoChao && !alvoAbaixo)
        {
            if (!detectorL0C1 && detectorL0C2)
            {
                rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * forcaPulo / 2, forcaPulo / 2);
            }
            else if (detectorL1C1 && !detectorL2C1)
            {
                rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * forcaPulo / 20, forcaPulo);
            }
        }

        #endregion
    }
    #endregion

    #region Logica Pacifica
    protected void LogicaPacifica()
    {
        if (estaNoChao)
        {
            //FAZ UM RANDOM PARA ESCOLHER ENTRE ANDAR PARA ESQUERDA OU DIREITA OU PARAR
            if (contTempoTomadaDeDecisao <= 0f)
            {
                float temp1;
                temp1 = UnityEngine.Random.Range(0f, 1f);

                if (temp1 > 2f / 3f)
                {
                    andarL = true;
                    andarR = false;
                }
                else if (temp1 > 1f / 3f)
                {
                    andarL = false;
                    andarR = true;
                }
                else
                {
                    andarL = false;
                    andarR = false;
                }
                //Debug.Log(temp1 + " " + tempoTomadaDeDecisao);
                contTempoTomadaDeDecisao = tempoTomadaDeDecisao;
            }

            //SE TEM BURACOS A FRENTE, ELE INVERTE A POSIÇÃO DE ANDAR
            if (!detectorL0C1 || detectorL1C1)
            {
                if (andarL)
                {
                    andarL = false;
                    andarR = true;
                }
                else if (andarR)
                {
                    andarL = true;
                    andarR = false;
                }
            }
            contTempoTomadaDeDecisao -= Time.deltaTime;
        }
        else
        {
            andarL = false;
            andarR = false;
            contTempoTomadaDeDecisao = 0;
        }
    }
    #endregion

    #region Atualiza Ações
    protected void AtualizaAcao()
    {
        if (estaNoChao && Mathf.Round(rb.velocity.y) == 0f)
        {
            //ANDA PARA A ESQUERDA OU DIREITA
            if (andarL)
            {
                rb.velocity = new Vector2(-velocidade, rb.velocity.y);
            }
            else if (andarR)
            {
                rb.velocity = new Vector2(velocidade, rb.velocity.y);
            }

            //FICA PARADO
            else if (!andarL || !andarR)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
            //
            //FAZ O PULO
            if (pular)
            {
                rb.velocity = new Vector2(rb.velocity.x, forcaPulo);
            }    
        }

        //ATUALIZA ESCALA
        if (andarL)
        {
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
        else if (andarR)
        {
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }


        //ATUALIZA ANIMAÇÃO
        if (estaNoChao)
        {
            an.SetBool("isJUMPING", false);
            if (andarL || andarR)
                an.SetBool("isRUNING", true);
            else
            {
                an.SetBool("isRUNING", false);
            }
        }
        else
        {
            an.SetBool("isJUMPING", true);
        }

        
    }
    #endregion

    #region Causar dano corporal
    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerScript>().SetRigidbody2D = new Vector2(Mathf.Sign(rb.velocity.x) * knockBack, knockBack / 2);
            other.GetComponent<PlayerScript>().SetVida -= danoCorpo;
        }
    }
    #endregion

    #region Buscar referência
    protected void BuscarReferencia()
    {
        //SE NÃO TIVER UMA REFERENCIA ELE VAI FAZER O CALCULO DA REFERENCIA MAIS PROXIMA
        GameObject tempValor;
        tempValor = null;
        foreach (GameObject pontoRefTemp in referenciasCenario)
        {
            //VERIFICA SE A REFERENCIA ESTÁ MAIS ALTA QUE A POSIÇÃO DO PERSONAGEM 
            if (alvoAcima)
            {
                //VERIFICA SE A REFERENCIA É MENOR QUE A variacaoBlocos , CASO CONTRARIO O INIMIGO NÃO CONSEGUE PULAR E A REFERENCIA É DESCONSIDERADA
                if ((pontoRefTemp.transform.position.y > tr.transform.position.y) && Mathf.Abs(pontoRefTemp.transform.position.y - tr.position.y) <= variacaoBlocos)
                {
                    //PEGA A PRIMEIRA REFERENCIA PARA PREENCHER A VARIAVEL TEMPVALOR, SE NÃO TIVESSE PREENCHIDO, ELE IRIA PODERIA PEGAR O PARAMETRO 0,0,0 COMO MAIS PROXIMO
                    if (tempValor == null)
                        tempValor = pontoRefTemp;

                    //VERIFICA O OBJETO MAIS PROXIMO DO JOGADOR || ELE VERIFICA O MAIS PROXIMO NA POSIÇÃO X E Y
                    if (Mathf.Abs((tempValor.transform.position.x - tr.position.x) + (tempValor.transform.position.y - tr.position.y)) >
                        Mathf.Abs((pontoRefTemp.transform.position.x - tr.position.x) + (pontoRefTemp.transform.position.y - tr.position.y)))
                    {
                        tempValor = pontoRefTemp;
                    }
                }
            }
            else if (alvoAbaixo)
            {
                //PEGA A PRIMEIRA REFERENCIA PARA PREENCHER A VARIAVEL TEMPVALOR, SE NÃO TIVESSE PREENCHIDO, ELE IRIA PODERIA PEGAR O PARAMETRO 0,0,0 COMO MAIS PROXIMO
                if (tempValor == null)
                    tempValor = pontoRefTemp;

                //VERIFICA SE A REFERENCIA É MENOR QUE A variacaoBlocos , CASO CONTRARIO O INIMIGO NÃO CONSEGUE PULAR E A REFERENCIA É DESCONSIDERADA
                //VERIFICA O OBJETO MAIS PROXIMO DO JOGADOR
                if ((pontoRefTemp.transform.position.y < tr.transform.position.y) && Mathf.Abs(pontoRefTemp.transform.position.y - tr.position.y) <= variacaoBlocos)
                {
                    if (Mathf.Abs((tempValor.transform.position.x - tr.position.x) + (tempValor.transform.position.y - tr.position.y)) >
                        Mathf.Abs((pontoRefTemp.transform.position.x - tr.position.x) + (pontoRefTemp.transform.position.y - tr.position.y)))
                    {
                        tempValor = pontoRefTemp;
                        Debug.Log("Referencia trocada" + pontoRefTemp);
                    }
                    Debug.Log("Referencia NÃO trocada" + pontoRefTemp);
                }
            }
        }
        //APÓS O CALCULO DA REFERENCIA MAIS PROXIMA ACIMA OU ABAIXO, ELE VAI PASSAR AS INFORMAÇÕES PARA O INIMIGO PERSEGUIR
        refMaisProxima = tempValor;
        if (tempValor != null)
            seguindoReferencia = true;

    }
    #endregion

    #region Receber dano
    //COMUNICADORES
    public float GetVidaEnemy
    {
        get { return vidaEnemy; }
        set { vidaEnemy = value; }
    }
    #endregion

    protected void Morrer()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        capsuleCollider2D.enabled = false;
        contadorMorte--; 

        //MORRENDO
        if (contadorMorte <= 0 && quantidadePiscadas >= 0)
        {
            sr.material = materialPiscada;
            if (sr.color.a > 0f)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
            else if (sr.color.a == 0f)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 255f);

            contadorMorte = defaultContMorte;
            quantidadePiscadas--;
        }

        //MORREU
        else if(contadorMorte <= 0) {
            int indexSorteada = UnityEngine.Random.Range(0, drops.Length);
            GameObject objSorteado = Instantiate(drops[indexSorteada]);
            objSorteado.transform.position = transform.position;

            Resetar();
        }        
    }

    protected void Resetar()
    {
        gameObject.SetActive(false);
        rb.gravityScale = 1;
        contadorMorte = defaultContMorte;
        vidaEnemy = defaultVida;
        sr.material = defaultMaterial;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        capsuleCollider2D.enabled = true;
        quantidadePiscadas = (int)contadorMorte * 2;                
    }
}
