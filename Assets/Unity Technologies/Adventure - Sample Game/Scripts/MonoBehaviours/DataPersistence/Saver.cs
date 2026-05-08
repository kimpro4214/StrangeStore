using UnityEngine;

public abstract class Saver : MonoBehaviour
{
    public string uniqueIdentifier;
    public SaveData saveData;

    protected string key;
    private SceneController sceneController;

    private void Awake()
    {
        sceneController = Object.FindFirstObjectByType<SceneController>();

        if (!sceneController)
        {
            Debug.LogWarning($"[Saver] SceneController no encontrado aún en {gameObject.name}. Se reintentará en OnEnable.");
        }
        
        key = SetKey();
    }

    private void OnEnable()
    {
        if (!sceneController)
        {
            sceneController = Object.FindFirstObjectByType<SceneController>();
        }

        if (sceneController != null)
        {
            sceneController.BeforeSceneUnload += Save;
            sceneController.AfterSceneLoad += Load;
        }
        else
        {
            Debug.LogError($"[Saver] Crítico: No se pudo encontrar SceneController para {gameObject.name}. " +
                           "Asegúrate de que la escena 'Persistent' esté cargada.");
        }
    }

    private void OnDisable()
    {
        if (sceneController != null)
        {
            sceneController.BeforeSceneUnload -= Save;
            sceneController.AfterSceneLoad -= Load;
        }
    }

    protected abstract string SetKey();
    protected abstract void Save();
    protected abstract void Load();
}