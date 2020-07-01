using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaScript : MonoBehaviour {

    private Rigidbody2D rb;
    private float posY;
    private bool portaAberta = false;
        // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        posY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (!(transform.position.y > posY + 2f))
        {
            if (portaAberta)
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime, transform.position.z);
        }
	}

    public void AbrirPorta() {
        portaAberta = true;
    }
}
