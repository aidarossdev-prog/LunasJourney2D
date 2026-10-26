using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameExit : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("No se ha asignado el menú de pausa.");
            return;
        }

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ContinueGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
{
    Time.timeScale = 1f;
    isPaused = false;

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

public void StartNewGame()
{
    if (GameManager.Instance == null ||
        !GameManager.Instance.IsProgressLoaded ||
        FirebaseInitializer.Instance == null)
    {
        Debug.LogWarning("El progreso todavía no está disponible.");
        return;
    }

    // Restablecer monedas y niveles en memoria.
    GameManager.Instance.NewGame();

    // Esperar a que Firebase confirme el guardado.
    FirebaseInitializer.Instance.GuardarNuevaPartida(() =>
    {
        Debug.Log("Nueva partida guardada. Regresando al nivel 1.");

        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainScene");
    });
}


public void ExitGame()
{
    Time.timeScale = 1f;

    Debug.Log("Cerrando Luna's Journey...");

    Application.Quit();
}

}