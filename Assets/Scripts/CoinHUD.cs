using UnityEngine;
using TMPro;

public class CoinHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text coinCounterText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ConnectHUD(coinCounterText);
        }
        else
        {
            Debug.LogWarning("No se encontró el GameManager para conectar el HUD.");
        }
    }
}