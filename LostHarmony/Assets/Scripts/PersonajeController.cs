using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");
        
        Vector2 movimiento = new Vector2(movimientoHorizontal, movimientoVertical);
        
        // Normalizar el vector para evitar movimiento más rápido en diagonal
        if (movimiento.magnitude > 1)
        {
            movimiento.Normalize();
        }
        
        rb.linearVelocity = movimiento * velocidad;
    }
}