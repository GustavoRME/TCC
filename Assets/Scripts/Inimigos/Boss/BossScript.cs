using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    private GameObject alvo;

    [Header("Vida")]
    [SerializeField] float vidaPrincipal = 1000;
    private float defaultVidaPrincipal;
    [SerializeField] private Text textVida;
    [SerializeField] private Text textAlvo;
    [SerializeField] private Image imageVida;
    [SerializeField] private GameObject imgBarraVida;


    [Header("Movimentação")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    
    [Header("Olhos")]
    [SerializeField] private OlhosScript olhos;
    [SerializeField] float tempoOlhosAbertos;
    private bool olhosAbertos = true;
    private bool rotinaIniciada = false;

    [Header("Braco Esquerdo")]
    [SerializeField]
    private GameObject bracoL;
    [SerializeField] private Quaternion defaultRotBracoL;
    [SerializeField] private float tempoParaAtirarL;
    [SerializeField] private bool podeAtirarL = true;
    [Header("Atributos")]
    [SerializeField]
    private float danoArma01;
    [SerializeField] private float velocidadeArma01;

    [Header("Braco Direito")]
    [SerializeField]
    private GameObject bracoR;
    [SerializeField] private Quaternion defaultRotBracoR;
    [SerializeField] private float tempoParaAtirarR;
    [SerializeField] private bool podeAtirarR = true;
    [Header("Atributos")]
    [SerializeField]
    private float danoArma02;
    [SerializeField] private float velocidadeArma02;

    [Header("Centro-Torpedo")]
    [SerializeField]
    private GameObject portaTorpedo;
    [SerializeField] private float tempoParaAtirarTorpedo;
    [SerializeField] private int torpedoIntervalo; // Um pequeno intervalo a cada uma quantidade de lasers
    [SerializeField] private int defaultTorpedoIntervalo;
    [SerializeField] private bool podeAtirarTorpedo = true;
    [Header("Atributos")]
    [SerializeField]
    private float danoTorpedo;
    [SerializeField] private float velocidadeTorpedo;

    [Header("Centro-Laser")]
    [SerializeField]
    private GameObject portaLaser;
    [SerializeField] private bool laserCentralPreparado;
    [SerializeField] private bool laserCentralPosicionado;
    [SerializeField] private bool laserCentralAtivo;
    [SerializeField] private float alinhamentoPos;

    [Header("Outros")]
    [SerializeField] private float contagemTrocaDeAtaque;
    [SerializeField] private int modoAtual;
    [SerializeField] private bool modoFuria;
    [SerializeField] private float fatorVelocidade;
    [SerializeField] private Material materialBranco;
    private Material defaultMaterial;
    [SerializeField] bool morreu = false;
    [SerializeField] GameObject explosaoNave;
    [SerializeField] GameObject fogoNaveL;
    [SerializeField] GameObject fogoNaveR;

    void Start()
    {
        alvo = GameObject.FindGameObjectWithTag("Player");
        defaultVidaPrincipal = vidaPrincipal;
        defaultMaterial = GetComponent<SpriteRenderer>().material;
        defaultTorpedoIntervalo = torpedoIntervalo;
        defaultRotBracoL = bracoL.transform.localRotation;
        defaultRotBracoR = bracoR.transform.localRotation;
    }

    private void FixedUpdate()
    {
        if (!morreu)
        {
            #region Randomizador de ataque
            if (contagemTrocaDeAtaque <= 0)
            {
                TrocarAcao();
            }

            if (!bracoL.activeSelf && !bracoR.activeSelf)
            {
                modoFuria = true;
                fatorVelocidade = 2;
            }

            #endregion

            #region Modo Normal
            if (!modoFuria)
            {
                if (modoAtual == 0)
                {
                    Movimentar();
                }
                else

                if (modoAtual == 1)
                {
                    Movimentar();
                    AtirarBracoL();
                }

                else if (modoAtual == 2)
                {
                    Movimentar();
                    AtirarBracoR();
                }
            }
            #endregion

            #region Modo Furia
            else
            {
                if (modoAtual == 0)
                {
                    Movimentar();
                }
                else

                if (modoAtual == 3)
                {
                    AtirarTorpedo();
                }
                if (modoAtual == 4)
                {
                    laserCentralAtivo = true;
                    AtivarLaserCentral();
                }
            }
            #endregion

            Animacao();

            #region Reset padrao (se não estiver sendo utilizado)
            if (modoAtual != 1 && bracoL.activeSelf)
            {
                bracoL.transform.localRotation = Quaternion.Lerp(bracoL.transform.localRotation, defaultRotBracoL, 0.2f);
            }

            if (modoAtual != 2 && bracoR.activeSelf)
            {
                bracoR.transform.localRotation = Quaternion.Lerp(bracoR.transform.localRotation, defaultRotBracoR, 0.2f);
            }
            if (modoAtual != 3)
            {

            }
            if (modoAtual != 4)
            {
                laserCentralPreparado = false;
                laserCentralPosicionado = false;
                portaLaser.GetComponent<LaserCentralScript>().DesativarLaser();
            }

            #endregion

            contagemTrocaDeAtaque -= Time.deltaTime;
        }
        else {
            portaLaser.GetComponent<LaserCentralScript>().DesativarLaser();
            Color cor = GetComponent<SpriteRenderer>().color;
            cor.a = Mathf.Lerp(cor.a, 0, 0.05f);
            GetComponent<SpriteRenderer>().color = cor;
            portaTorpedo.GetComponent<SpriteRenderer>().color = cor;
            olhos.GetComponent<SpriteRenderer>().color = cor;
            
        }
    }

    private void Animacao()
    {
        if (olhosAbertos)
        {
            if (!rotinaIniciada)
            {
                StartCoroutine(OlhosPiscadas(tempoOlhosAbertos));
                rotinaIniciada = true;
            }

            olhos.AbrirOlhos();
        }
        else
        {
            if (!rotinaIniciada)
            {
                StartCoroutine(OlhosPiscadas(tempoOlhosAbertos / 10));
                rotinaIniciada = true;
            }
            olhos.FecharOlhos();
        }

        if (modoFuria)
        {
            olhos.AbrirOlhosFuria();
            olhos.AlterarCor(Color.red);
        }
    }

    void Movimentar()
    {
        transform.position = new Vector3((Mathf.Clamp(Mathf.Lerp(transform.position.x, alvo.transform.position.x, 0.05f * fatorVelocidade), minX, maxX)), transform.position.y);
    }

    void TrocarAcao()
    {
        if (modoAtual == 1 || modoAtual == 2 || modoAtual == 3 || modoAtual == 4)
        {
            modoAtual = 0;
            contagemTrocaDeAtaque = 2 / fatorVelocidade;
        }
        else if (!modoFuria)
        {
            bool validado = false;
            while (!validado)
            {
                int novoModo;
                int estadoRandom = UnityEngine.Random.Range(0, 50);
                if (estadoRandom < 25)
                    novoModo = 1;
                else
                    novoModo = 2;

                if (novoModo == 1 && bracoL.activeSelf == true)
                {
                    modoAtual = novoModo;
                    validado = true;
                }
                else

                if (novoModo == 2 && bracoR.activeSelf == true)
                {
                    modoAtual = novoModo;
                    validado = true;
                }
            }
            contagemTrocaDeAtaque = 5 / fatorVelocidade;
        }

        else
        {
            int estadoRandom;
            estadoRandom = UnityEngine.Random.Range(0, 50);
            if (estadoRandom < 10)
                modoAtual = 0;
            else if (estadoRandom < 30)
                modoAtual = 3;
            else
                modoAtual = 4;
            contagemTrocaDeAtaque = 5 / fatorVelocidade;
        }
    }

    #region Braco esquerdo
    private void AtirarBracoL()
    {
        //ROTACIONAR BRAÇO
        bracoL.transform.localRotation = Quaternion.Lerp(bracoL.transform.localRotation, Mirar(bracoL), 0.5f);

        //ATIRAR
        if (podeAtirarL)
        {
            podeAtirarL = false;
            bracoL.GetComponent<BaseBracoScript>().Atirar(velocidadeArma01 * fatorVelocidade, danoArma01);
            StartCoroutine(ContagemAtirarL());
        }
    }
    #endregion

    #region Braco direito
    private void AtirarBracoR()
    {
        //ROTACIONAR BRAÇO
        bracoR.transform.localRotation = Quaternion.Lerp(bracoR.transform.localRotation, Mirar(bracoR), 0.5f);

        if (podeAtirarR)
        {
            podeAtirarR = false;
            bracoR.GetComponent<BaseBracoScript>().Atirar(velocidadeArma02 * fatorVelocidade, danoArma02);
            StartCoroutine(ContagemAtirarR());
        }
    }
    #endregion

    #region AtirarTorpedo
    private void AtirarTorpedo()
    {
        if ((int)transform.position.x == 0)
        {
            if (podeAtirarTorpedo)
            {
                podeAtirarTorpedo = false;
                portaTorpedo.GetComponent<BaseBracoScript>().Atirar(velocidadeTorpedo * fatorVelocidade, danoTorpedo, torpedoIntervalo);
                if (torpedoIntervalo <= 1)
                {
                    tempoParaAtirarTorpedo = tempoParaAtirarTorpedo * 3;
                    StartCoroutine(ContagemAtirarTorpedo());
                    tempoParaAtirarTorpedo = tempoParaAtirarTorpedo / 3;
                    torpedoIntervalo = defaultTorpedoIntervalo;
                    contagemTrocaDeAtaque = 0;
                }
                else
                {
                    StartCoroutine(ContagemAtirarTorpedo());
                    torpedoIntervalo--;
                }
            }
        }
        else
            transform.localPosition = Vector2.Lerp(transform.position, new Vector2(0, transform.position.y), 0.3f);
    }
    #endregion

    #region AtivarLaserCentral
    void AtivarLaserCentral()
    {
        if (laserCentralAtivo)
        {
            if (laserCentralPreparado == false)
            {
                int AlvoSorteado = UnityEngine.Random.Range(0, 2);
                if (AlvoSorteado == 0)
                    alinhamentoPos = minX;
                if (AlvoSorteado == 1)
                    alinhamentoPos = maxX;
                laserCentralPreparado = true;
            }
            else if (laserCentralPosicionado == false)
            {
                transform.position = new Vector3((Mathf.Clamp(Mathf.Lerp(transform.position.x, alinhamentoPos, 0.05f * fatorVelocidade), minX, maxX)), transform.position.y);
                if ((int)transform.position.x == (int)alinhamentoPos)
                {
                    laserCentralPosicionado = true;
                    if (alinhamentoPos == minX)
                        alinhamentoPos = maxX;
                    else
                        alinhamentoPos = minX;
                }
            }
            else
            {
                portaLaser.GetComponent<LaserCentralScript>().AtivarLaser();
                if (transform.position.x < alinhamentoPos)
                    transform.position += (Vector3.right / 5) * fatorVelocidade;
                else
                if (transform.position.x > alinhamentoPos)
                    transform.position += (Vector3.left / 5) * fatorVelocidade;

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
                if ((int)transform.position.x == (int)alinhamentoPos)
                {
                    portaLaser.GetComponent<LaserCentralScript>().DesativarLaser();
                    contagemTrocaDeAtaque = 0;
                }
            }
        }
    }

    #endregion

    #region Essenciais
    Quaternion Mirar(GameObject braco)
    {
        Vector3 posicaoAngulo;
        posicaoAngulo.x = alvo.transform.position.x - braco.transform.position.x;
        posicaoAngulo.y = alvo.transform.position.y - braco.transform.position.y;

        float angulo = (Mathf.Atan2(posicaoAngulo.y, posicaoAngulo.x) * Mathf.Rad2Deg) + 90;
        return Quaternion.Euler(0, 0, angulo);
    }

    IEnumerator ContagemAtirarL()
    {
        yield return new WaitForSeconds(tempoParaAtirarL / fatorVelocidade);
        podeAtirarL = true;
    }

    IEnumerator ContagemAtirarR()
    {
        yield return new WaitForSeconds(tempoParaAtirarR / fatorVelocidade);
        podeAtirarR = true;
    }

    IEnumerator ContagemAtirarTorpedo()
    {
        yield return new WaitForSeconds(tempoParaAtirarR / fatorVelocidade);
        podeAtirarTorpedo = true;
    }

    IEnumerator ContagemLaserCentral()
    {
        yield return new WaitForSeconds(tempoParaAtirarR / fatorVelocidade);
        podeAtirarTorpedo = true;
    }

    IEnumerator OlhosPiscadas(float valor) {
        yield return new WaitForSeconds(valor);
        olhosAbertos = !olhosAbertos;
        rotinaIniciada = false;
    }

    IEnumerator Piscar()
    {
        GetComponent<SpriteRenderer>().material = materialBranco;
        yield return new WaitForSeconds(0.005f);
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("projetil_player") && !bracoL.activeSelf && !bracoR.activeSelf)
        {
            vidaPrincipal -= collision.GetComponent<projetil_player>().danoArma;
            if (vidaPrincipal <= 0)
            {
                vidaPrincipal = 0;
                Morrer();
            }
            imageVida.overrideSprite = GetComponent<SpriteRenderer>().sprite;
            imgBarraVida.transform.localScale = new Vector3((1 / defaultVidaPrincipal) * vidaPrincipal, 1, 1);
            textVida.text = vidaPrincipal + " / " + defaultVidaPrincipal;
            textAlvo.text = "Corpo";
            StartCoroutine(Piscar());
        }
    }

    private void Morrer()
    {
        morreu = true;
        explosaoNave.SetActive(true);
    }
}
