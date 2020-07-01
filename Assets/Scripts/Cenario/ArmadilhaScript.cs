using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaScript : MonoBehaviour
{
    [SerializeField] private float danoArmadilha;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerScript>().SetVida -= danoArmadilha;
            
        }

        if (collision.tag == "Inimigo")
        {
            collision.GetComponent<InimigoBaseIA>().GetVidaEnemy -= danoArmadilha;
        }
    }
}
