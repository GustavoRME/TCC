using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class minigunScript : baseArmaScript {

    // Use this for initialization
    void Awake()
    {
        base.Awake();
        if (SceneManager.GetActiveScene().buildIndex >= 3)
            armaDisponivel = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
