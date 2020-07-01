using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GerenDialogScript gerenDialogScript;
    public GameObject caixaDialago;
    public DialagoScript dialagoHistoria;
    bool dialagoHistoriaCarregado;

    private void Start()
    {
        gerenDialogScript = GameObject.FindGameObjectWithTag("GerenciadorDialog").GetComponent<GerenDialogScript>();
    }
    private void Update()
    {
        if (GetComponent<Transform>().transform.position.z >= 1.36f)
        {
            if (!dialagoHistoriaCarregado)
            {
                gerenDialogScript.ComecarDialago(dialagoHistoria);
                dialagoHistoriaCarregado = true;
            }
            if (caixaDialago.activeSelf == false)
                SceneManager.LoadScene(1);
            
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(4);
        }
    }
    public void iniciado()
    {
        GetComponent<Animator>().SetBool("Iniciado", true);
    }

    public void Sair()
    {
        Application.Quit();
    }
}