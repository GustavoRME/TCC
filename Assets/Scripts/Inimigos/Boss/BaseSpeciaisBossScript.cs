using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpeciaisBossScript : MonoBehaviour {

    public float dano;
    public float velocidade;
    public int lane;

    protected BoxCollider2D boxCollider2D;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected bool colidiu;

    protected void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
