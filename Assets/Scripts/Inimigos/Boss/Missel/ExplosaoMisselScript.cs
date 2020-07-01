using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosaoMisselScript : MonoBehaviour {

    public float tempoParaMorte;

    void Start () {
        tempoParaMorte = GetComponent<ParticleSystem>().main.duration;
    }
	
	void Update () {
        tempoParaMorte -= Time.deltaTime;
        if (tempoParaMorte <= 0)
            Destroy(this.gameObject);
    }
}
