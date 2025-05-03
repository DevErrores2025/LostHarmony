using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void BtnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void BtnSalir()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
