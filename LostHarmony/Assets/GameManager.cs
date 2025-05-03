using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    public int scorePerCatch = 10;
    public Text scoreText;
    public Text lifeText;
    public GameObject panelGameOver;
    
    private int currentScore = 0;
    private ControladorSubmarino playerSubmarino;
    
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Buscar al jugador
        playerSubmarino = FindObjectOfType<ControladorSubmarino>();
        
        // Ocultar panel de game over si existe
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
        
        Debug.Log("GameManager inicializado");
    }
    
    void Update()
    {
        // Actualizar UI
        UpdateUI();
        
        // Comprobar si el jugador está muerto
        if (playerSubmarino != null && playerSubmarino.ObtenerVidaActual() <= 0)
        {
            GameOver();
        }
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        Debug.Log("Puntuación aumentada: " + points + " puntos. Total: " + currentScore);
        UpdateUI();
    }
    
    void UpdateUI()
    {
        // Actualizar texto de puntuación
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + currentScore;
        }
        
        // Actualizar texto de vida
        if (lifeText != null && playerSubmarino != null)
        {
            lifeText.text = "Vida: " + playerSubmarino.ObtenerVidaActual();
        }
    }
    
    void GameOver()
    {
        Debug.Log("¡GAME OVER!");
        
        // Mostrar panel de game over si existe
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
        
        // Opcional: pausar el juego
        Time.timeScale = 0;
    }
    
    // Método para reiniciar el juego
    public void RestartGame()
    {
        // Restaurar timeScale
        Time.timeScale = 1;
        
        // Reiniciar nivel
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}