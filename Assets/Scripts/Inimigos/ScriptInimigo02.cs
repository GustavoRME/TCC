using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptInimigo02 : InimigoBaseIA
{
    [Header("Logica Atirar")]
    [SerializeField]
    public GameObject arma;
    [SerializeField] private Transform referenciaTiro;
    [SerializeField] private RaycastHit2D pontoFinalLaser;
    private Vector3 direcaoLaser;

    [SerializeField] private bool alvoNaMira;
    [SerializeField] private bool atirando;
    [SerializeField] private float tempoParaAtirar;

    [SerializeField] private Vector3 posicaoAngulo;
    [SerializeField] private float angulo;
    [SerializeField] public int distanciaMax;
    private LineRenderer lineRenderer;
    [SerializeField] public LayerMask layersColisao; //COLIDIR COM MAPA E PERSONAGEM

    public float tempPDeslLaser, contagemAtirar;
    float velocidadeLookOff = 0.3f;

    // Use this for initialization
    void Awake()
    {
        base.Awake();
        direcaoLaser = Vector3.zero;
        contagemAtirar = tempoParaAtirar;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vidaEnemy > 0)
        {
            Sensores();

            if (!atirando)
            {
                if (perseguindo)
                {
                    if (!alvoNaMira)
                    {
                        LogicaAgressiva();
                        velocidade = defaultVelocidade * 2;
                    }
                    else if (alvoNaMira)
                    {
                        andarL = false;
                        andarR = false;
                        tempPDeslLaser = 2f;
                    }
                }
                else
                {
                    LogicaPacifica();
                    velocidade = defaultVelocidade / 2;
                }
            }

            LogicaAtirar();
            AtualizaAcao();
        }
        else
            Morrer();
    }

    void Sensores()
    {
        base.Sensores();
        atirando = lineRenderer.startColor == Color.red; //SE A COR DO LASER FOR VERMELHO, ELE ESTÁ ATIRANDO, CASO CONTRARIO NÃO ESTARÁ
        
        if (!atirando) 
        {
            //SE ESTIVER ATIRANDO ELE NÃO ATUALIZARÁ A POSIÇÃO DO LASER
            direcaoLaser = playerAlvo.transform.position - referenciaTiro.transform.position;

            //CALCULA PONTO FINAL
            pontoFinalLaser = Physics2D.Raycast(referenciaTiro.transform.position, direcaoLaser, distanciaMax, layersColisao);

            //SE ESTIVER ATIRANDO ELE NAO VERIFICARÁ SE O ALVO ESTÁ NA MIRA (SIMPLESMENTE ELE IRÁ IGNORAR QUALQUER MOVIMENTO DO JOGADOR)
            alvoNaMira = pontoFinalLaser.collider && pontoFinalLaser.collider.tag == "Player";
        }
        else
            pontoFinalLaser = Physics2D.Raycast(referenciaTiro.transform.position, direcaoLaser, distanciaMax*10, layersColisao);


        if (!atirando)
        {
            if (alvoNaMira)
                //SE ESTIVER NA MIRA, SOMARÁ O CONTADOR
                tempPDeslLaser = tempoParaAtirar;
            else
                //SE ESTIVER MIRANDO NO ALVO, ELE PEGARÁ ESSA POSIÇÃO DA MIRA, CASO CONTRARIO, A MIRA FICARÁ NA POSIÇÃO PADRÃO
                pontoFinalLaser = Physics2D.Raycast(referenciaTiro.transform.position, Vector2.right * transform.lossyScale.x);
        }
    }

    private void AtualizaAcao()
    {
        if (!atirando)
        {
            #region RotacaoArma
            if (alvoNaMira)
            {
                //MIRA NO JOGADOR
                posicaoAngulo.x = pontoFinalLaser.transform.position.x - transform.position.x;
                posicaoAngulo.y = pontoFinalLaser.transform.position.y - transform.position.y;

                angulo = Mathf.Atan2(posicaoAngulo.y, posicaoAngulo.x) * Mathf.Rad2Deg;
            }
            else
            {
                //SE NÃO ESTIVER MIRANDO NO INIMIGO, O CANO VOLTARÁ AO NORMAL COM LERP
                if (angulo > 90f)
                    angulo = Mathf.Lerp(angulo, 180, velocidadeLookOff);
                else if (angulo < -90f)
                    angulo = Mathf.Lerp(angulo, -180, velocidadeLookOff);
                else
                    angulo = Mathf.Lerp(angulo, 0, velocidadeLookOff);
            }

            if (angulo > 90f)
                arma.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo + 180));
            else if (angulo < -90f)
                arma.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo - 180));
            else
                arma.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));

            #endregion

            #region Virar inimigo Esquerda/Direita
            if (alvoNaMira) //SE O ALVO ESTIVER NA MIRA, ELE VAI VIRAR NA DIREÇÃO DO ALVO
            {
                if (transform.position.x < playerAlvo.transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1);
                else if (transform.position.x > playerAlvo.transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1);
            }
            #endregion
        }

        #region Desenhar mira
        Vector3 point = pontoFinalLaser.point;
        point.z = transform.position.z;
        lineRenderer.SetPosition(0, referenciaTiro.transform.position);
        lineRenderer.SetPosition(1, new Vector3(point.x, Mathf.Lerp(lineRenderer.GetPosition(1).y, point.y, velocidadeLookOff)));
        #endregion

        base.AtualizaAcao();
    }

    private void LogicaAtirar()
    {
        if (tempPDeslLaser >= 0)
        {
            if (alvoNaMira || atirando)
            {
                lineRenderer.enabled = true;

                if (tempoParaAtirar / 2 >= contagemAtirar)
                {
                    lineRenderer.startColor = Color.yellow;
                    lineRenderer.endColor = Color.yellow;
                }

                if (tempoParaAtirar / 3 >= contagemAtirar)
                {
                    lineRenderer.startColor = Color.red;
                    lineRenderer.endColor = Color.red;
                }

                if (contagemAtirar <= 0)
                {
                    arma.GetComponent<armaInimigoScript>().InstanciarProjetil(transform.localScale.x);
                    lineRenderer.startColor = Color.green;
                    lineRenderer.endColor = Color.green;
                    contagemAtirar = tempoParaAtirar;
                }
                contagemAtirar -= Time.deltaTime;
            }
            else {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
                contagemAtirar = tempoParaAtirar;
            }

            tempPDeslLaser -= Time.deltaTime;
        }
        else
        {            
            lineRenderer.enabled = false;   
        }
    }
}
