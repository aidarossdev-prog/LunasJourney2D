using UnityEngine;
using System.Collections;

public class MagicCoin : MonoBehaviour
{
    [Header("Coin Identification")]
    [SerializeField] private string coinID;

    private bool collected = false;
    private bool progressReady = false;

    private Collider2D coinCollider;

    private void Awake()
    {
        coinCollider = GetComponent<Collider2D>();

        // Evitar que Luna recolecte monedas antes de cargar Firebase.
        if (coinCollider != null)
        {
            coinCollider.enabled = false;
        }
    }

    private IEnumerator Start()
    {
        // Esperar a que Firebase termine de recuperar el progreso.
        yield return new WaitUntil(() =>
            GameManager.Instance != null &&
            GameManager.Instance.IsProgressLoaded
        );

        // Comprobar si esta moneda ya fue recolectada.
        if (GameManager.Instance.HasCollectedCoin(coinID))
        {
            collected = true;
            gameObject.SetActive(false);
            yield break;
        }

        // La moneda está disponible para recolectarse.
        progressReady = true;

        if (coinCollider != null)
        {
            coinCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!progressReady || collected)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        if (GameManager.Instance == null ||
            !GameManager.Instance.IsProgressLoaded)
        {
            Debug.LogWarning(
                "La moneda está esperando la carga del progreso."
            );
            return;
        }

        if (string.IsNullOrWhiteSpace(coinID))
        {
            Debug.LogWarning(
                "Esta moneda no tiene un Coin ID asignado."
            );
            return;
        }

        // Evitar recolectar nuevamente una moneda registrada.
        if (GameManager.Instance.HasCollectedCoin(coinID))
        {
            collected = true;
            gameObject.SetActive(false);
            return;
        }

        // Registrar la moneda.
        GameManager.Instance.AddCoin(coinID);

        // Confirmar que GameManager la registró.
        if (!GameManager.Instance.HasCollectedCoin(coinID))
        {
            Debug.LogWarning(
                "No se pudo registrar la moneda: " + coinID
            );
            return;
        }

        collected = true;

        Debug.Log("Moneda recolectada: " + coinID);

        gameObject.SetActive(false);
    }
}