using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventosScript : MonoBehaviour {

    [SerializeField] GameObject camera;
    [SerializeField] private bool tremerCamera;

    [SerializeField] private bool GameOver;

    float contagemTremeCamera;
    void Start() {
        camera = GameObject.FindGameObjectWithTag("MainCamera");

    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TremerCamera(5);
            contagemTremeCamera = 10;
        }

        contagemTremeCamera -= Time.deltaTime;
    }

    public void Morrer(bool entrada) {
        GameOver = entrada;
    }

    public void TremerCamera(float intensidade) {
        while(contagemTremeCamera > 0)
        {
            camera.GetComponent<Transform>().position += Vector3.right * intensidade;
            camera.GetComponent<Transform>().position += Vector3.up * intensidade;
            
            camera.GetComponent<Transform>().position += Vector3.left * intensidade;
            camera.GetComponent<Transform>().position += Vector3.down * intensidade;
            
        }
    }
}
