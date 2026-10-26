using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Progress")]
    [SerializeField] private int totalCoins = 0;

[SerializeField] private int currentLevel = 1;
[SerializeField] private int highestUnlockedLevel = 1;

public int CurrentLevel => currentLevel;
public int HighestUnlockedLevel => highestUnlockedLevel;

    [Header("User Interface")]
    [SerializeField] private TMP_Text coinCounterText;

    // Guarda los identificadores de las monedas recolectadas.
    private HashSet<string> collectedCoinIDs = new HashSet<string>();

    public int TotalCoins => totalCoins;
    public List<string> CollectedCoinIDs =>
    new List<string>(collectedCoinIDs);
    public bool IsProgressLoaded { get; private set; } = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateCoinCounter();
    }

    public bool HasCollectedCoin(string coinID)
    {
        return collectedCoinIDs.Contains(coinID);
    }

    public void AddCoin(string coinID)
    {

        if (!IsProgressLoaded)
{
            Debug.LogWarning(
             "Esperando a que Firebase cargue el progreso."
             );
             return;
            }

        // Evita registrar monedas sin identificador.
        if (string.IsNullOrWhiteSpace(coinID))
        {
            Debug.LogWarning("La moneda no tiene un Coin ID asignado.");
            return;
        }

        // Evita contar dos veces la misma moneda.
        if (!collectedCoinIDs.Add(coinID))
        {
            Debug.LogWarning("Esta moneda ya fue recolectada: " + coinID);
            return;
        }

        totalCoins = collectedCoinIDs.Count;

        Debug.Log("Monedas recolectadas: " + totalCoins);
        Debug.Log("Moneda registrada: " + coinID);

        UpdateCoinCounter();
        if (FirebaseInitializer.Instance != null)
            {
            FirebaseInitializer.Instance.GuardarProgreso();
            }

    }

public void ConnectHUD(TMP_Text newCoinCounterText)
{
    coinCounterText = newCoinCounterText;
    UpdateCoinCounter();
}

    private void UpdateCoinCounter()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = "Monedas: " + totalCoins;
        }
    }

    public void SetCurrentLevel(int level)
{
    if (level < 1)
        return;

    currentLevel = level;

    Debug.Log("Nivel actual: " + currentLevel);

    if (IsProgressLoaded && FirebaseInitializer.Instance != null)
    {
        FirebaseInitializer.Instance.GuardarProgreso();
    }
}

public void UnlockLevel(int level)
{
    if (level > highestUnlockedLevel)
    {
        highestUnlockedLevel = level;

        Debug.Log("Nuevo nivel desbloqueado: " + highestUnlockedLevel);

        if (IsProgressLoaded && FirebaseInitializer.Instance != null)
        {
            FirebaseInitializer.Instance.GuardarProgreso();
        }
    }
}

public void LoadProgress(
    int savedLevel,
    int savedHighestLevel,
    List<string> savedCoinIDs)
{
    currentLevel = Mathf.Max(1, savedLevel);

    highestUnlockedLevel = Mathf.Max(
        currentLevel,
        savedHighestLevel
    );

    collectedCoinIDs = new HashSet<string>(
        savedCoinIDs ?? new List<string>()
    );

    totalCoins = collectedCoinIDs.Count;
    IsProgressLoaded = true;
    UpdateCoinCounter();

    Debug.Log("Progreso recuperado correctamente.");
    Debug.Log("Monedas recuperadas: " + totalCoins);
    Debug.Log("Nivel actual recuperado: " + currentLevel);
    Debug.Log("Nivel máximo desbloqueado: " + highestUnlockedLevel);
}

public void NewGame()
{
    if (!IsProgressLoaded)
    {
        Debug.LogWarning(
            "No se puede reiniciar: Firebase aún no carga el progreso."
        );
        return;
    }

    collectedCoinIDs.Clear();
    totalCoins = 0;
    currentLevel = 1;
    highestUnlockedLevel = 1;

    UpdateCoinCounter();

    Debug.Log("Progreso restablecido para una nueva partida.");
}


}