using UnityEngine;

public class ControladorSubmarino : MonoBehaviour
{
    public float duracionEfectoDano = 0.3f;
    public bool probarDano = false; // Booleano para probar manualmente
    
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    private bool recibiendoDano = false;
    
    void Start()
    {
        // Obtenemos el SpriteRenderer del submarino
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Guardamos el color original
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }
    
    void Update()
    {
        // Si se activa el booleano en el inspector, probamos el daño
        if (probarDano && !recibiendoDano)
        {
            RecibirDano();
            probarDano = false; // Lo reseteamos para poder probar otra vez
        }
        
        // Alternativa: probar con una tecla
        if (Input.GetKeyDown(KeyCode.T)) // La tecla T para Test/Probar
        {
            RecibirDano();
        }
    }
    
    public void RecibirDano()
    {
        if (recibiendoDano) return; // Evitamos activarlo si ya está activo
        
        recibiendoDano = true;
        
        // Cambiamos el color a blanco para el efecto de daño
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        
        // Volvemos al color original después del tiempo especificado
        Invoke("DesactivarEfectoDano", duracionEfectoDano);
    }
    
    private void DesactivarEfectoDano()
    {
        recibiendoDano = false;
        
        // Restauramos el color original
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorOriginal;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo") || collision.gameObject.CompareTag("Obstaculo"))
        {
            RecibirDano();
        }
    }
}