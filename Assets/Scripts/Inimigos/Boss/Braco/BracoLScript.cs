using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BracoLScript : BaseBracoScript {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("projetil_player"))
        {
            vidaBraco -= collision.GetComponent<projetil_player>().danoArma;
            if (vidaBraco <= 0)
                vidaBraco = 0;
            AtualizarUI();
            StartCoroutine(Piscar());
            if (vidaBraco == 0)
                Morrer();
        }
    }

    private void AtualizarUI() {
        imgVida.overrideSprite = this.GetComponent<SpriteRenderer>().sprite;
        string textoUI = vidaBraco + " / " + defaultVidaBraco;
        imgBarraVida.transform.localScale = new Vector3((1 / defaultVidaBraco) * vidaBraco, 1, 1);
        textVida.text = textoUI;
        textAlvo.text = "Braco esquerdo";
    }
}
