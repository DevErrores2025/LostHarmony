using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [Header("Referencias")]
    public Image imagenBarraVida; // Referencia a la imagen que muestra la vida
    public ControladorSubmarino controladorSubmarino; // Referencia al submarino
    
    [Header("Configuración")]
    public Color colorVidaLlena = Color.green;
    public Color colorVidaBaja = Color.red;
    public float umbralVidaBaja = 0.3f; // Porcentaje (30%) para cambiar a color rojo
    
    private int vidaMaxima;
    
    void Start()
    {
        // Verificar que tenemos las referencias necesarias
        if (imagenBarraVida == null)
        {
            Debug.LogError("¡Error! No se ha asignado la imagen de la barra de vida");
            enabled = false;
            return;
        }
        
        if (controladorSubmarino == null)
        {
            Debug.LogError("¡Error! No se ha asignado el controlador del submarino");
            enabled = false;
            return;
        }
        
        // Obtener la vida máxima del submarino al inicio
        vidaMaxima = controladorSubmarino.vidaInicial;
        
        // Inicializar la barra de vida
        ActualizarBarraVida();
        
        Debug.Log("BarraVida inicializada. Vida máxima: " + vidaMaxima);
    }
    
    void Update()
    {
        // Actualizar la barra de vida en cada frame
        ActualizarBarraVida();
    }
    
    void ActualizarBarraVida()
    {
        // Obtener la vida actual del submarino
        int vidaActual = controladorSubmarino.ObtenerVidaActual();
        
        // Calcular el porcentaje de vida (entre 0 y 1)
        float porcentajeVida = (float)vidaActual / vidaMaxima;
        
        // Actualizar el fill amount de la imagen
        imagenBarraVida.fillAmount = porcentajeVida;
        
        // Cambiar el color dependiendo del nivel de vida
        if (porcentajeVida <= umbralVidaBaja)
        {
            imagenBarraVida.color = colorVidaBaja; // Rojo para vida baja
        }
        else
        {
            imagenBarraVida.color = colorVidaLlena; // Verde para vida normal
        }
    }
}