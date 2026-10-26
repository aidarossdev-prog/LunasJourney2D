using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseInitializer Instance { get; private set; }

    
    private bool guardadoEnProceso = false;
private bool guardadoPendiente = false;

    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;

    DontDestroyOnLoad(gameObject);
}
    private FirebaseAuth auth;

    private void Start()
    {
        Debug.Log("Luna's Journey: iniciando Firebase...");

        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError(
                        "Error al comprobar Firebase: " + task.Exception
                    );
                    return;
                }

                if (task.Result == DependencyStatus.Available)
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;

                    Debug.Log("Firebase inicializado correctamente.");

                    IniciarSesionAnonima();
                }
                else
                {
                    Debug.LogError(
                        "Firebase no está disponible: " + task.Result
                    );
                }
            });
    }

    

    private void IniciarSesionAnonima()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null)
        {
            Debug.Log(
                "Jugador autenticado. UID: " + auth.CurrentUser.UserId
            );

            CargarProgreso(auth.CurrentUser.UserId);

            // GuardarDatosDePrueba(auth.CurrentUser.UserId);
            

            return;
        }

        auth.SignInAnonymouslyAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError(
                        "Error en la autenticación anónima: " + task.Exception
                    );

                    return;
                }

                FirebaseUser usuario = task.Result.User;

                Debug.Log(
                    "Autenticación anónima exitosa. UID: " + usuario.UserId
                );

                CargarProgreso(usuario.UserId);

                // GuardarDatosDePrueba(usuario.UserId);
            });
    }

    private void GuardarDatosDePrueba(string userId)
{
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    DocumentReference jugadorRef =
        db.Collection("players").Document(userId);

    Dictionary<string, object> datosJugador =
        new Dictionary<string, object>
        {
            { "totalCoins", 0 },
            { "currentLevel", 1 },
            { "highestUnlockedLevel", 1 },
            { "collectedCoinIDs", new List<string>() }
        };

    jugadorRef.SetAsync(datosJugador)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(
                    "Error al guardar en Firestore: " + task.Exception
                );
                return;
            }

            Debug.Log(
                "¡Datos del jugador guardados correctamente en Firestore!"
            );
        });
}
private void CargarProgreso(string userId)
{
    Debug.Log("DIAGNOSTICO 1: Entrando a CargarProgreso.");

    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    Debug.Log("DIAGNOSTICO 2: Firestore DefaultInstance obtenido.");
    
    DocumentReference jugadorRef =
        db.Collection("players").Document(userId);

    jugadorRef.GetSnapshotAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(
                    "Error al consultar Firestore: " + task.Exception
                );
                return;
            }

            DocumentSnapshot documento = task.Result;

            if (documento.Exists)
                {
                 Debug.Log(
                         "¡Documento del jugador encontrado en Firestore!"
                        );

                 int nivelActual = documento.ContainsField("currentLevel")
                  ? (int)documento.GetValue<long>("currentLevel")
                  : 1;

                 int nivelDesbloqueado = documento.ContainsField("highestUnlockedLevel")
                    ? (int)documento.GetValue<long>("highestUnlockedLevel")
                   : 1;

                 List<string> monedasRecolectadas =
                 documento.ContainsField("collectedCoinIDs")
                   ? documento.GetValue<List<string>>("collectedCoinIDs")
                  : new List<string>();

                 if (GameManager.Instance != null)
                {
                    GameManager.Instance.LoadProgress(
                      nivelActual,
                      nivelDesbloqueado,
                     monedasRecolectadas
                      );
                      if (nivelActual == 2)
                        {
                        Debug.Log("Restaurando partida en el nivel 2...");

                        if (SceneManager.GetActiveScene().name != "Level2")
                        {
                        SceneManager.LoadScene("Level2");
                        }
                        }
                    }
                else
                 {
                    Debug.LogError(
                     "No se encontró GameManager para cargar el progreso."
                     );
                  }
                }
                 else
                {
                    Debug.Log(
                   "El jugador todavía no tiene progreso guardado."
                    );

                 if (GameManager.Instance != null)
                      {
                   GameManager.Instance.LoadProgress(
                     1,
                       1,
                       new List<string>()
                     );

                  Debug.Log(
                      "Nueva partida iniciada correctamente."
                     );
                    }
                 else
                 {
                    Debug.LogError(
                   "No se encontró GameManager para iniciar la partida."
                 );
                 }
                    }
                });
    }
public bool GuardadoEnProceso => guardadoEnProceso;
   public void GuardarProgreso()
{
    if (auth == null || auth.CurrentUser == null)
    {
        Debug.LogWarning(
            "No se puede guardar: el jugador no está autenticado."
        );
        return;
    }

    if (GameManager.Instance == null ||
        !GameManager.Instance.IsProgressLoaded)
    {
        Debug.LogWarning(
            "No se puede guardar: el progreso todavía no está listo."
        );
        return;
    }

    // Si hay un guardado en curso, solicitar otro al finalizar.
    if (guardadoEnProceso)
    {
        guardadoPendiente = true;
        return;
    }

    guardadoEnProceso = true;

    GameManager manager = GameManager.Instance;

    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    DocumentReference jugadorRef = db
        .Collection("players")
        .Document(auth.CurrentUser.UserId);

    Dictionary<string, object> datosJugador =
        new Dictionary<string, object>
        {
            { "totalCoins", manager.TotalCoins },
            { "currentLevel", manager.CurrentLevel },
            { "highestUnlockedLevel", manager.HighestUnlockedLevel },
            { "collectedCoinIDs", manager.CollectedCoinIDs }
        };

    jugadorRef.SetAsync(datosJugador)
        .ContinueWithOnMainThread(task =>
        {
            guardadoEnProceso = false;

            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(
                    "Error al guardar el progreso: " + task.Exception
                );
            }
            else
            {
                Debug.Log(
                    "¡Progreso guardado correctamente en Firestore!"
                );
            }

            // Si hubo cambios mientras guardábamos, enviar
            // el estado más reciente después del primer guardado.
            if (guardadoPendiente)
            {
                guardadoPendiente = false;
                GuardarProgreso();
            }
        });
}

public void GuardarNuevaPartida(System.Action alTerminar)
{
    if (auth == null || auth.CurrentUser == null ||
        GameManager.Instance == null ||
        !GameManager.Instance.IsProgressLoaded)
    {
        Debug.LogError("No se puede guardar la nueva partida.");
        return;
    }

    // Evitar que un guardado anterior sobrescriba la nueva partida.
    if (guardadoEnProceso)
    {
        Debug.LogWarning(
            "Hay un guardado en curso. Espera unos segundos e inténtalo de nuevo."
        );
        return;
    }

    guardadoEnProceso = true;

    guardadoPendiente = false;

    GameManager manager = GameManager.Instance;

    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    DocumentReference jugadorRef = db
        .Collection("players")
        .Document(auth.CurrentUser.UserId);

    Dictionary<string, object> datosJugador =
        new Dictionary<string, object>
        {
            { "totalCoins", manager.TotalCoins },
            { "currentLevel", manager.CurrentLevel },
            { "highestUnlockedLevel", manager.HighestUnlockedLevel },
            { "collectedCoinIDs", manager.CollectedCoinIDs }
        };

    jugadorRef.SetAsync(datosJugador)
        .ContinueWithOnMainThread(task =>
        {
            guardadoEnProceso = false;

            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(
                    "Error al guardar la nueva partida: " + task.Exception
                );
                return;
            }

            Debug.Log("Nueva partida guardada correctamente en Firebase.");

            alTerminar?.Invoke();
        });
}

}