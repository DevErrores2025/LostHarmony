using UnityEngine;
using System.Collections;

public class EfectoDanoParpadeo : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    
    public float duracionTotal = 0.8f;
    public int cantidadParpadeos = 4;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            MostrarEfectoDano();
        }
    }
    
    public void MostrarEfectoDano()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(EfectoParpadeo());
        }
    }
    
    private IEnumerator EfectoParpadeo()
    {
        float tiempoPorParpadeo = duracionTotal / (cantidadParpadeos * 2);
        
        for (int i = 0; i < cantidadParpadeos; i++)
        {
            // Cambiar a blanco
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(tiempoPorParpadeo);
            
            // Volver al color original
            spriteRenderer.color = colorOriginal;
            yield return new WaitForSeconds(tiempoPorParpadeo);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo") || collision.gameObject.CompareTag("Obstaculo"))
        {
            MostrarEfectoDano();
        }
    }
}