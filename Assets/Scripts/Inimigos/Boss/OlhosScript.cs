using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhosScript : MonoBehaviour {

    public Sprite olhosAbertos;
    public Sprite olhosFechados;
    public Sprite olhosFuria;

    private SpriteRenderer spriteRenderer;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void AbrirOlhos() {
        spriteRenderer.sprite = olhosAbertos;
    }

    public void FecharOlhos()
    {
        spriteRenderer.sprite = olhosFechados;
    }

    public void AbrirOlhosFuria()
    {
        spriteRenderer.sprite = olhosFuria;
    }

    public void AlterarCor(Color cor) {
        spriteRenderer.color = cor;
    }
}
