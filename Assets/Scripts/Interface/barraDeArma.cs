using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barraDeArma : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private Image imgArmaAtual;
    [SerializeField] private Image imgProjetilAtual;
    [SerializeField] private Text textCartucho;
    [SerializeField] private Text textQtdMunicao;

    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
        imgArmaAtual = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        imgArmaAtual.sprite = player.GetComponent<PlayerScript>().armas[player.GetComponent<PlayerScript>().armaAtual].GetComponent<baseArmaScript>().spriteArma;
        imgProjetilAtual.sprite = player.GetComponent<PlayerScript>().armas[player.GetComponent<PlayerScript>().armaAtual].GetComponent<baseArmaScript>().referenciaProjetil.GetComponent<SpriteRenderer>().sprite;

        string tempCartucho = player.GetComponent<PlayerScript>().armas[player.GetComponent<PlayerScript>().armaAtual].GetComponent<baseArmaScript>().quantidadeNoCartucho
            + " / " + player.GetComponent<PlayerScript>().armas[player.GetComponent<PlayerScript>().armaAtual].GetComponent<baseArmaScript>().tamanhoCartucho;

        textCartucho.text = tempCartucho;

        string tempQtdMunicao = ""+player.GetComponent<PlayerScript>().armas[player.GetComponent<PlayerScript>().armaAtual].GetComponent<baseArmaScript>().quantidadeMunicao;
        textQtdMunicao.text = tempQtdMunicao;
    }
}
