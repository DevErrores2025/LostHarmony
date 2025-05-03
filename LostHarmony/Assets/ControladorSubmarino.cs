using UnityEngine;

public class ControladorSubmarino : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadHorizontal = 5f;
    public float velocidadVertical = 3f;
    public Transform red;
    public float distanciaRed = 1f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movimiento;
    
    void Awake()
    {
        // Obtener componentes una sola vez al inicio
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Configurar Rigidbody2D si existe
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    
    void Update()
    {
        // Calcular movimiento en Update (mejor respuesta a input)
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");
        
        // Guardar el vector de movimiento para usar en FixedUpdate
        movimiento = new Vector2(
            movimientoHorizontal * velocidadHorizontal, 
            movimientoVertical * velocidadVertical
        );
        
        // Actualizar animación
        if (animator != null)
        {
            animator.SetFloat("Velocidad", movimiento.magnitude);
        }
        
        // Orientar el sprite
        if (movimientoHorizontal != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movimientoHorizontal), 1, 1);
        }
    }
    
    void FixedUpdate()
    {
        // Aplicar movimiento en FixedUpdate (mejor para físicas)
        if (rb != null)
        {
            rb.linearVelocity = movimiento;
        }
        
        // Actualizar posición de la red
        ActualizarPosicionRed();
    }
    
    void ActualizarPosicionRed()
    {
        if (red != null)
        {
            // Actualizar posición de la red
            red.position = new Vector3(
                transform.position.x,
                transform.position.y - distanciaRed,
                red.position.z
            );
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contactCount > 0 && rb != null)
        {
            // Obtener la normal de contacto
            Vector2 normal = collision.contacts[0].normal;
            Vector2 velocidadActual = rb.linearVelocity;
            
            // Detener movimiento en dirección de la colisión
            if (Mathf.Abs(normal.x) > 0.5f)
            {
                velocidadActual.x = 0;
            }
            if (Mathf.Abs(normal.y) > 0.5f)
            {
                velocidadActual.y = 0;
            }
            
            rb.linearVelocity = velocidadActual;
        }
    }
}