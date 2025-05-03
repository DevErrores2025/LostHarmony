using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform personaje;
    public float suavizado = 0.125f;
    
    // Límites basados en tus cortinas
    public Vector2 limiteMinimo = new Vector2(-1.5f, -1f);
    public Vector2 limiteMaximo = new Vector2(1.5f, 1f);
    
    private Vector3 velocidad = Vector3.zero;
    
    void Start()
    {
        // Verificar que tenemos asignado un personaje para seguir
        if (personaje == null)
        {
            Debug.LogError("¡Error! No se ha asignado un personaje para seguir en CamaraController");
        }
    }
    
    void LateUpdate()
    {
        if (personaje == null)
            return;
            
        // Calcular posición objetivo
        Vector3 posicionObjetivo = new Vector3(personaje.position.x, personaje.position.y, transform.position.z);
        
        // Limitar la posición dentro de los bordes definidos
        posicionObjetivo.x = Mathf.Clamp(posicionObjetivo.x, limiteMinimo.x, limiteMaximo.x);
        posicionObjetivo.y = Mathf.Clamp(posicionObjetivo.y, limiteMinimo.y, limiteMaximo.y);
        
        // Mover la cámara suavemente hacia el objetivo
        transform.position = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref velocidad, suavizado);
    }
    
    // Esta función es útil para visualizar los límites en el editor
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
            
        Gizmos.color = Color.yellow;
        
        // Dibujar rectángulo de límites
        Vector3 tamano = new Vector3(
            limiteMaximo.x - limiteMinimo.x,
            limiteMaximo.y - limiteMinimo.y,
            0.1f
        );
        
        Vector3 centro = new Vector3(
            (limiteMinimo.x + limiteMaximo.x) / 2,
            (limiteMinimo.y + limiteMaximo.y) / 2,
            0
        );
        
        Gizmos.DrawWireCube(centro, tamano);
    }
}