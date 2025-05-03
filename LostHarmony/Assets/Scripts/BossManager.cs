using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject prefabBoss; // Prefab de la villana
    public GameObject prefabEsbirroEnojado; // Prefab del esbirro enojado
    public Transform redDelJugador; // Referencia a la red del jugador

    [Header("Configuración Boss")]
    public Vector3 posicionSpawnBoss = new Vector3(-11.12f, -4.14f, 0f); // Posición donde aparecerá la villana
    public int esbirrosParaInvocarBoss = 10; // Cantidad de esbirros que se deben atrapar
    public float duracionAtaqueBoss = 120f; // Duración del ataque en segundos (2 minutos)

    [Header("Configuración Esbirros Enojados")]
    public float minY = -4.18f; // Posición Y mínima para esbirros enojados
    public float maxY = 2.62f; // Posición Y máxima para esbirros enojados
    public float intervalSpawnEsbirroEnojado = 1.5f; // Cada cuántos segundos spawneará un esbirro enojado
    public float velocidadEsbirroEnojado = 7f; // Qué tan rápido se moverán los esbirros enojados

    // Variables privadas
    private int esbirrosAtrapados = 0;
    private bool bossActivo = false;
    private GameObject bossInstance;
    private List<GameObject> esbirrosEnojadosActivos = new List<GameObject>();

    void Start()
    {
        // Verificar que tenemos las referencias necesarias
        if (prefabBoss == null || prefabEsbirroEnojado == null)
        {
            Debug.LogError("Faltan asignar prefabs en BossManager");
            enabled = false;
            return;
        }

        Debug.Log("BossManager inicializado. Esbirros para invocar boss: " + esbirrosParaInvocarBoss);
    }

    // Llamar a este método cuando un esbirro sea atrapado
    public void RegistrarEsbirroAtrapado()
    {
        esbirrosAtrapados++;
        Debug.Log("Esbirro atrapado! Total: " + esbirrosAtrapados + "/" + esbirrosParaInvocarBoss);

        // Verificar si es hora de invocar al boss
        if (esbirrosAtrapados >= esbirrosParaInvocarBoss && !bossActivo)
        {
            InvocarBoss();
        }
    }

    void InvocarBoss()
    {
        Debug.Log("¡INVOCANDO BOSS!");
        bossActivo = true;

        // Instanciar al boss
        bossInstance = Instantiate(prefabBoss, posicionSpawnBoss, Quaternion.identity);

        // Iniciar coroutine para el ataque del boss
        StartCoroutine(AtaqueBoss());
    }

    IEnumerator AtaqueBoss()
    {
        float tiempoInicio = Time.time;
        float tiempoFinal = tiempoInicio + duracionAtaqueBoss;

        // Continuar lanzando esbirros enojados durante el tiempo especificado
        while (Time.time < tiempoFinal && bossActivo)
        {
            // Genera un esbirro enojado en una posición Y aleatoria
            SpawnEsbirroEnojado();

            // Esperar antes de generar el siguiente
            yield return new WaitForSeconds(intervalSpawnEsbirroEnojado);
        }

        // Finalizar el ataque del boss
        FinalizarAtaqueBoss();
    }

    void SpawnEsbirroEnojado()
    {
        // Generar posición aleatoria en Y dentro del rango
        float randomY = Random.Range(minY, maxY);
        Vector3 posicionSpawn = new Vector3(posicionSpawnBoss.x, randomY, posicionSpawnBoss.z);

        // Instanciar esbirro enojado
        GameObject nuevoEsbirro = Instantiate(prefabEsbirroEnojado, posicionSpawn, Quaternion.identity);
        
        // Añadir a la lista para seguimiento
        esbirrosEnojadosActivos.Add(nuevoEsbirro);

        // Configurar comportamiento del esbirro enojado
        ConfigurarEsbirroEnojado(nuevoEsbirro);

        Debug.Log("Esbirro enojado generado en Y: " + randomY);
    }

    void ConfigurarEsbirroEnojado(GameObject esbirro)
    {
        // Asegurarse de que tiene el tag correcto
        esbirro.tag = "Esbirro";
        
        // Agregar rigidbody si no tiene
        Rigidbody2D rb = esbirro.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = esbirro.AddComponent<Rigidbody2D>();
        }
        
        // Configurar rigidbody
        rb.gravityScale = 0f; // Sin gravedad
        rb.isKinematic = false;
        
        // Agregar collider si no tiene
        if (esbirro.GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = esbirro.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
        
        // Hacer que se mueva horizontalmente hacia la derecha
        rb.linearVelocity = new Vector2(velocidadEsbirroEnojado, 0);
        
        // Añadir script para manejar la captura (reutilizamos CatchableObject)
        if (esbirro.GetComponent<CatchableObject>() == null)
        {
            esbirro.AddComponent<CatchableObject>();
        }
        
        // Destruir automáticamente después de cierto tiempo si sale de la pantalla
        Destroy(esbirro, 15f);
    }

    void FinalizarAtaqueBoss()
    {
        Debug.Log("Finalizando ataque del boss");
        
        // Destruir el boss
        if (bossInstance != null)
        {
            Destroy(bossInstance);
        }
        
        // Limpiar los esbirros enojados restantes
        foreach (GameObject esbirro in esbirrosEnojadosActivos)
        {
            if (esbirro != null)
            {
                Destroy(esbirro);
            }
        }
        
        esbirrosEnojadosActivos.Clear();
        bossActivo = false;
        
        // Reiniciar contador para la próxima aparición del boss
        esbirrosAtrapados = 0;
    }

    // Para detener el ataque manualmente (por ejemplo, si el jugador muere)
    public void DetenerAtaqueBoss()
    {
        StopAllCoroutines();
        FinalizarAtaqueBoss();
    }
}