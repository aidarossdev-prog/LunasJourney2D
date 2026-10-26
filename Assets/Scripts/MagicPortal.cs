using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicPortal : MonoBehaviour
{
    [Header("Portal Requirements")]
    [SerializeField] private int requiredCoins = 8;

    [Header("Next Level")]
    [SerializeField] private string nextSceneName = "Level2";
    [SerializeField] private int nextLevelNumber = 2;

    private bool portalActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar que Luna entró al portal.
        if (!other.CompareTag("Player") || portalActivated)
        {
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("No se encontró el GameManager.");
            return;
        }

        // Comprobar si Luna tiene suficientes monedas.
        if (GameManager.Instance.TotalCoins >= requiredCoins)
        {
            ActivatePortal();
        }
        else
        {
            int missingCoins =
                requiredCoins - GameManager.Instance.TotalCoins;

            Debug.Log(
                "Portal bloqueado. Faltan " +
                missingCoins +
                " monedas mágicas."
            );
        }
    }

    private void ActivatePortal()
    {
        portalActivated = true;

        // Registrar el avance del jugador.
        GameManager.Instance.UnlockLevel(nextLevelNumber);
        GameManager.Instance.SetCurrentLevel(nextLevelNumber);

        Debug.Log(
            "¡Portal activado! Viajando al Bosque de las Sombras."
        );

        // Cargar el siguiente nivel.
        SceneManager.LoadScene(nextSceneName);
    }
}