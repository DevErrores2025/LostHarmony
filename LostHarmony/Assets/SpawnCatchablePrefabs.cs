using UnityEngine;

public class SpawnCatchablePrefabs : MonoBehaviour
{
    public GameObject prefab; // Assign your ESBIRRO FELIZ prefab here
    
    [Header("Spawn Settings")]
    public float spawnHeight = 5f; // Altura de generación
    public float minX = -10f; // Posición X mínima
    public float maxX = 10f; // Posición X máxima
    public float spawnInterval = 2f; // Time between spawns
    
    [Header("Tags")]
    public string netTag = "Net"; // Tag for your net
    public string playerTag = "Player"; // Tag for your character
    
    private float nextSpawnTime;
    
    void Start()
    {
        // Verificar que tenemos prefab asignado
        if (prefab == null)
        {
            Debug.LogError("¡Error! No se ha asignado un prefab en SpawnCatchablePrefabs");
            enabled = false;
            return;
        }
        
        // Log información inicial
        Debug.Log("Spawner inicializado. Configuración: Height=" + spawnHeight + 
                  ", MinX=" + minX + ", MaxX=" + maxX + ", Interval=" + spawnInterval);
    }
    
    void Update()
    {
        // Spawn a new prefab when it's time
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    void SpawnObject()
    {
        // Generar una posición X aleatoria entre minX y maxX
        float randomX = Random.Range(minX, maxX);
        
        // Crear la posición de generación con Z = -1 (como en tu posición predeterminada)
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, -1);
        
        // Instantiate the prefab
        GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        
        Debug.Log("Esbirro generado en posición: " + spawnPosition);
        
        // Make sure it has the necessary components for physics
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // MODIFICACIÓN: asegurarse de que el Rigidbody está configurado correctamente
            rb.useGravity = true;
            rb.isKinematic = false;
            
            // Opcional: agregar una pequeña fuerza aleatoria horizontal
            rb.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 0, 0), ForceMode.Impulse);
            
            Debug.Log("Aplicada configuración al Rigidbody del esbirro (3D)");
        }
        else
        {
            // Intenta obtener un Rigidbody2D en su lugar
            Rigidbody2D rb2d = newObject.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                // MODIFICACIÓN: asegurarse de que el Rigidbody2D está configurado correctamente
                rb2d.gravityScale = 1f;
                rb2d.isKinematic = false;
                
                // Opcional: agregar una pequeña fuerza aleatoria horizontal
                // CORRECCIÓN: Usar ForceMode2D en lugar de ForceMode
                rb2d.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), 0), ForceMode2D.Impulse);
                
                Debug.Log("Aplicada configuración al Rigidbody2D del esbirro (2D)");
            }
        }
        
        // Asegurarse de que el objeto tiene el tag correcto
        newObject.tag = "Esbirro";
        
        // Add a component to handle being caught
        var catchableComponent = newObject.AddComponent<CatchableObject>();
        
        // Verificar o agregar colliders si es necesario
        if (newObject.GetComponent<Collider>() == null && newObject.GetComponent<Collider2D>() == null)
        {
            // Si no hay collider, añadir uno de cada tipo para asegurar colisiones
            newObject.AddComponent<BoxCollider>();
            newObject.AddComponent<BoxCollider2D>();
            Debug.Log("Añadidos colliders al esbirro");
        }
    }
}

public class CatchableObject : MonoBehaviour
{
    private bool isCaught = false;
    
    void Start()
    {
        // Asignar el tag "Esbirro" al objeto
        gameObject.tag = "Esbirro";
        
        // Verificar que tengamos un collider
        bool hasCollider = false;
        
        // Verificar/configurar collider 3D
        Collider collider3D = GetComponent<Collider>();
        if (collider3D != null)
        {
            // Configurar el collider como trigger
            collider3D.isTrigger = true;
            hasCollider = true;
            Debug.Log("Configurado Collider 3D para " + gameObject.name);
        }
        
        // Verificar/configurar collider 2D
        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D != null)
        {
            // Configurar el collider como trigger
            collider2D.isTrigger = true;
            hasCollider = true;
            Debug.Log("Configurado Collider 2D para " + gameObject.name);
        }
        
        // Si no hay collider, añadir uno
        if (!hasCollider)
        {
            // Añadir tanto 2D como 3D para estar seguros
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            
            BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
            
            Debug.Log("Añadidos nuevos colliders a " + gameObject.name);
        }
        
        // Verificar/configurar Rigidbody
        ConfigurarRigidbody();
    }
    
    void ConfigurarRigidbody()
    {
        // Verificar Rigidbody 3D
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Permitir que caiga
            rb.useGravity = true;   // Activar gravedad
            Debug.Log("Configurado Rigidbody 3D para " + gameObject.name);
        }
        
        // Verificar Rigidbody 2D
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.isKinematic = false;   // Permitir que caiga
            rb2d.gravityScale = 1f;     // Activar gravedad
            Debug.Log("Configurado Rigidbody 2D para " + gameObject.name);
        }
    }
    
    // Prueba múltiples métodos de detección de colisiones
    
    // Trigger 3D
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Esbirro: OnTriggerEnter con " + other.name + ", Tag: " + other.tag);
        ProcesarColision(other.gameObject);
    }
    
    // Trigger 2D
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Esbirro: OnTriggerEnter2D con " + other.name + ", Tag: " + other.tag);
        ProcesarColision(other.gameObject);
    }
    
    // Colisión 3D
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Esbirro: OnCollisionEnter con " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);
        ProcesarColision(collision.gameObject);
    }
    
    // Colisión 2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Esbirro: OnCollisionEnter2D con " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);
        ProcesarColision(collision.gameObject);
    }
    
    void ProcesarColision(GameObject other)
    {
        // Verificar si ya fue atrapado
        if (isCaught) return;
        
        // Si colisiona con el jugador, intentar hacerle daño
        if (other.CompareTag("Player"))
        {
            Debug.Log("Esbirro intentando hacer daño al jugador");
            
            // Intentar obtener el componente ControladorSubmarino
            ControladorSubmarino submarino = other.GetComponent<ControladorSubmarino>();
            if (submarino != null)
            {
                Debug.Log("Componente ControladorSubmarino encontrado, aplicando daño");
                submarino.RecibirDaño(1);
            }
            else
            {
                Debug.Log("¡ERROR! No se encontró el componente ControladorSubmarino en el objeto Player");
            }
        }
        
        // Check if collided with net or player (para ser atrapado)
        if (other.CompareTag("Net") || other.CompareTag("Player"))
        {
            isCaught = true;
            Debug.Log("Esbirro atrapado por " + other.name);
            
            // Notificar al BossManager que se atrapó un esbirro
            BossManager bossManager = FindObjectOfType<BossManager>();
            if (bossManager != null)
            {
                bossManager.RegistrarEsbirroAtrapado();
                Debug.Log("Notificada captura al BossManager");
            }
            
            // Get caught by the net or player
            Transform parent = other.transform;
            
            // Disable physics when caught (for both 2D and 3D)
            DisablePhysics();
            
            // Attach to the catcher
            transform.SetParent(parent);
            
            // Try to access GameManager and add score
            var gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(10);
                Debug.Log("Puntuación añadida: 10 puntos");
            }
            else
            {
                Debug.Log("GameManager no encontrado. No se pudo añadir puntuación.");
            }
        }
    }
    
    void DisablePhysics()
    {
        // Disable 3D physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            Debug.Log("Física 3D desactivada para " + gameObject.name);
        }
        
        // Disable 2D physics
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.isKinematic = true;
            rb2d.gravityScale = 0;
            Debug.Log("Física 2D desactivada para " + gameObject.name);
        }
    }
}