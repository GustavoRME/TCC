using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barraDeVida : MonoBehaviour {

    [SerializeField] private GameObject objAlvo;
    [SerializeField] private Text quantidadeVida;

    void Start () {
        objAlvo = GameObject.FindGameObjectWithTag("Player");
	}
	
	void FixedUpdate () {
        transform.localScale = new Vector3(Mathf.Clamp(objAlvo.GetComponent<PlayerScript>().SetVida/100,0,1), 1, 1);
        string temp = objAlvo.GetComponent<PlayerScript>().SetVida + "%";
        quantidadeVida.text = temp;

    }
}
