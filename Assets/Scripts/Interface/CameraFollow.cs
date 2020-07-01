using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("Target")]
    [SerializeField]
    private Transform objetoAlvo;
    public float ajusteCameraAfrente;
    public float zonaMorta;

    [SerializeField]
    private float velocidadePerseguicao;

    [Header("Limites da camêra")]
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
            ajusteCameraAfrente = 5;
        else
            ajusteCameraAfrente = 0;
    }

    void FixedUpdate()
    {
        if (objetoAlvo != null)
        {
            //POSIÇÃO X
            if (transform.position.x < objetoAlvo.transform.position.x - zonaMorta || transform.position.x > objetoAlvo.transform.position.x + zonaMorta)
                transform.position = new Vector3(
                    Mathf.Lerp(transform.position.x, Mathf.Clamp(objetoAlvo.transform.position.x + (ajusteCameraAfrente* objetoAlvo.transform.localScale.x), minX, maxX), velocidadePerseguicao), 
                    transform.position.y, 
                    transform.position.z);

            //POSIÇÃO Y
            if (transform.position.y < objetoAlvo.transform.position.y - zonaMorta || transform.position.y > objetoAlvo.transform.position.y + zonaMorta)
                transform.position = new Vector3(
                    transform.position.x, 
                    Mathf.Lerp(transform.position.y, Mathf.Clamp(objetoAlvo.transform.position.y, minY, maxY), velocidadePerseguicao), 
                    transform.position.z);
        }
    }
}
