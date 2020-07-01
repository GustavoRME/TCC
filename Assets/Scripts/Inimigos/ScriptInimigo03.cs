using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptInimigo03 : MonoBehaviour
{
    [Header("Logica Atirar")]
    [SerializeField] protected GameObject playerAlvo;
    [SerializeField] public GameObject arma;
    [SerializeField] private Transform referenciaTiro;
    [SerializeField] private RaycastHit2D pontoFinalLaser;
    [SerializeField] private bool alvoNaMira;
    [SerializeField] private bool atirando;

    [SerializeField] private Vector3 posicaoAngulo;
    [SerializeField] private float angulo;
    [SerializeField] public int distanciaMax;
    [SerializeField] public LayerMask layersColisao; //COLIDIR COM MAPA E PERSONAGEM
    private LineRenderer lineRenderer;

    float tempPDeslLaser, contagemAtirar;
    float velocidadeLookOff;

    void Awake()
    {
        playerAlvo = GameObject.FindGameObjectWithTag("Player");
        velocidadeLookOff = 0.3f;
        lineRenderer = arma.GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        Sensores();
        RotacionarCano();
        LogicaAtirar();
    }


    void Sensores()
    {
        pontoFinalLaser = Physics2D.Raycast(referenciaTiro.transform.position, playerAlvo.transform.position - referenciaTiro.transform.position, distanciaMax, layersColisao);
        alvoNaMira = pontoFinalLaser.collider && pontoFinalLaser.collider.tag == "Player";
        if (alvoNaMira)
        {
            tempPDeslLaser = 1f;
            arma.GetComponent<Animator>().SetFloat("Velocidade", 1);
        }
        else
        {
            //SE ESTIVER MIRANDO NO ALVO, ELE PEGARÁ ESSA POSIÇÃO DA MIRA, CASO CONTRARIO, A MIRA FICARÁ NA POSIÇÃO PADRÃO
            pontoFinalLaser = Physics2D.Raycast(referenciaTiro.transform.position,  Vector2.right * transform.lossyScale.x);
            arma.GetComponent<Animator>().SetFloat("Velocidade", Mathf.Lerp(arma.GetComponent<Animator>().GetFloat("Velocidade"), 0f, 0.05f));
        }
    }

    void RotacionarCano()
    {
        #region Rotacionar Cano
        if (alvoNaMira)
        {
            //MIRA NO JOGADOR
            posicaoAngulo.x = pontoFinalLaser.transform.position.x - transform.position.x;
            posicaoAngulo.y = pontoFinalLaser.transform.position.y - transform.position.y;

            angulo = Mathf.Atan2(posicaoAngulo.y, posicaoAngulo.x) * Mathf.Rad2Deg;


            if (angulo > 90f)
                angulo = 90f;
            if (angulo < -90f)
                angulo = -90f;

            //if (angulo > 90f || angulo < -90f)
            //    arma.transform.localScale = new Vector3(1f, arma.transform.localScale.y, arma.transform.localScale.z);
            //else
            //    arma.transform.localScale = new Vector3(-1f, arma.transform.localScale.y, arma.transform.localScale.z);
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
            arma.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angulo));

        #endregion

        #region Desenhar mira
        Vector3 point;
        if (pontoFinalLaser.collider == null)
            point = referenciaTiro.transform.position + (Vector3.right * transform.lossyScale.x) * distanciaMax * 10;
        else
            point = pontoFinalLaser.point;
        point.z = transform.position.z;
        lineRenderer.SetPosition(0, referenciaTiro.transform.position);
        lineRenderer.SetPosition(1, new Vector3(point.x, Mathf.Lerp(lineRenderer.GetPosition(1).y, point.y, velocidadeLookOff)));;
        #endregion
    }

    private void LogicaAtirar()
    {
        if (tempPDeslLaser >= 0)
        {
            if (alvoNaMira || atirando)
            {
                lineRenderer.enabled = true;
                arma.GetComponent<armaInimigoScript>().InstanciarProjetil(transform.localScale.x);
            }
            tempPDeslLaser -= Time.deltaTime;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

}
